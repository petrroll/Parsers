﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhpParser.Parser;
using System.IO;
using PHP.Core.Text;
using PHP.Syntax;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    [DeploymentItem("TestData.csv")]
    [DeploymentItem(@"..\..\Tokens.php")]
    public class LexerTests
    {
        public TestContext TestContext { get; set; }

        private string ParseByPhp(string path)
        {
            Process process = new Process();
            StringBuilder output = new StringBuilder();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"..\..\..\..\Tools\PHP v7.0\php.exe";
            process.StartInfo.Arguments = "-f tokens.php " + path;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            while (!process.HasExited)
                output.Append(process.StandardOutput.ReadToEnd());
            process.WaitForExit();// Waits here for the process to exit.
            return output.ToString();
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TestData.csv", "TestData#csv", DataAccessMethod.Sequential)]
        public void LexerConstructorTest()
        {
            string path = (string)TestContext.DataRow["files"];
            SourceUnit sourceUnit = new CodeSourceUnit(File.ReadAllText(path), path, new System.Text.ASCIIEncoding(), Lexer.LexicalStates.INITIAL);
            PhpParser.Parser.ITokenProvider<SemanticValueType, Span> lexer = new Lexer(new StreamReader(path), sourceUnit, null, null, null, LanguageFeatures.ShortOpenTags, 0);
            Assert.AreNotEqual(null, lexer);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\TestData.csv", "TestData#csv", DataAccessMethod.Sequential)]
        public void LexerGetNextTokenTest()
        {
            string path = (string)TestContext.DataRow["files"];

            SourceUnit sourceUnit = new CodeSourceUnit(File.ReadAllText(path), path, new System.Text.ASCIIEncoding(), Lexer.LexicalStates.INITIAL);
            PhpParser.Parser.ITokenProvider<SemanticValueType, Span> lexer = new Lexer(new StreamReader(path), sourceUnit, null, null, null, LanguageFeatures.ShortOpenTags, 0);

            string parsed = ParseByPhp(path);
            parsed = parsed.Substring(0, parsed.LastIndexOf('-'));
            parsed = Regex.Replace(parsed.Replace("\r", " ").Replace("\n", " "), @"\s+", " ");
            int i = 0;
            string[][] expectedTokens = (
                from s in parsed.Split('-')
                let num = i++
                group s by num / 3 into g
                select g.ToArray()
                ).ToArray();

            //List<KeyValuePair<Tokens, SemanticValueType>> l = new List<KeyValuePair<Tokens, SemanticValueType>>();
            //Tokens t = Tokens.END;
            //while ((t = (Tokens)lexer.GetNextToken()) != Tokens.END)
            //{
            //    l.Add(new KeyValuePair<Tokens, SemanticValueType>(t, lexer.TokenValue));
            //}

            foreach (var expectedToken in expectedTokens)
            {
                Tokens token = (Tokens)lexer.GetNextToken();
                Assert.AreEqual(int.Parse(expectedToken[0]), (int)token);
                if (token == Tokens.T_VARIABLE || token == Tokens.T_STRING || token == Tokens.T_END_HEREDOC)
                {
                    Assert.AreEqual(expectedToken[2].TrimStart('$'), lexer.TokenValue.Object.ToString());
                }
                if (token == Tokens.T_DNUMBER)
                {
                    Assert.AreEqual(double.Parse(expectedToken[2]), lexer.TokenValue.Double);
                }
                if (token == Tokens.T_LNUMBER)
                {
                    Assert.AreEqual(int.Parse(expectedToken[2]), lexer.TokenValue.Integer);
                }
            }
        }
    }
}