﻿using PHP.Core.AST;
using PHP.Core.Text;
using PHP.Syntax;
using PhpParser;
using PhpParser.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestImplementation
{
    /// <summary>
    /// Nodes factory used by <see cref="Parser.Parser"/>.
    /// </summary>
    internal class TestNodesFactory : TestErrorSink, INodesFactory<LangElement, Span>
    {
        SourceUnit _sourceUnit;

        public TestNodesFactory(SourceUnit sourceUnit)
        {
            _sourceUnit = sourceUnit;
        }

        public LangElement ArrayItem(Span span, LangElement expression, LangElement indexOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Assert(Span span, LangElement assertion, LangElement failureOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Assignment(Span span, LangElement target, LangElement value, Operations assignOp)
        {
            throw new NotImplementedException();
        }

        public LangElement BinaryOperation(Span span, Operations operation, LangElement leftExpression, LangElement rightExpression)
        {
            throw new NotImplementedException();
        }

        public LangElement Block(Span span, IEnumerable<LangElement> statements)
        {
            throw new NotImplementedException();
        }

        public LangElement BlockComment(Span span, string content)
        {
            throw new NotImplementedException();
        }

        public LangElement Call(Span span, LangElement nameExpr, CallSignature signature, TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        public LangElement Call(Span span, LangElement nameExpr, CallSignature signature, LangElement memberOfOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Call(Span span, QualifiedName name, Span nameSpan, CallSignature signature, TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        public LangElement Call(Span span, QualifiedName name, QualifiedName? nameFallback, Span nameSpan, CallSignature signature, LangElement memberOfOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement ClassConstDecl(Span span, VariableName name, LangElement initializer)
        {
            throw new NotImplementedException();
        }

        public LangElement ColonBlock(Span span, IEnumerable<LangElement> statements, Tokens endToken)
        {
            throw new NotImplementedException();
        }

        public LangElement Concat(Span span, IEnumerable<LangElement> expressions)
        {
            throw new NotImplementedException();
        }

        public LangElement DeclList(Span span, PhpMemberAttributes attributes, IEnumerable<LangElement> decls)
        {
            throw new NotImplementedException();
        }

        public LangElement Do(Span span, LangElement body, LangElement cond)
        {
            throw new NotImplementedException();
        }

        public LangElement Echo(Span span, IEnumerable<LangElement> parameters)
        {
            throw new NotImplementedException();
        }

        public LangElement Eval(Span span, LangElement code)
        {
            throw new NotImplementedException();
        }

        public LangElement Exit(Span span, LangElement statusOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement FieldDecl(Span span, VariableName name, LangElement initializerOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement For(Span span, IEnumerable<LangElement> init, IEnumerable<LangElement> cond, IEnumerable<LangElement> action, LangElement body)
        {
            throw new NotImplementedException();
        }

        public LangElement Foreach(Span span, LangElement enumeree, ForeachVar keyOpt, ForeachVar value, LangElement body)
        {
            throw new NotImplementedException();
        }

        public LangElement Function(Span span, bool conditional, bool aliasReturn, PhpMemberAttributes attributes, QualifiedName? returnType, Span returnTypeSpan, Name name, Span nameSpan, IEnumerable<FormalTypeParam> typeParamsOpt, IEnumerable<FormalParam> formalParams, Span formalParamsSpan, LangElement body)
        {
            throw new NotImplementedException();
        }

        public LangElement GlobalCode(Span span, IEnumerable<LangElement> statements, NamingContext context)
        {
            Debug.Assert(statements.All(s => s is Statement), "Global code contains node that is not a statement!");
            return new GlobalCode(statements.Select(s => (Statement)s).ToList(), _sourceUnit);
        }

        public LangElement GlobalConstDecl(Span span, bool conditional, VariableName name, LangElement initializer)
        {
            throw new NotImplementedException();
        }

        public LangElement Goto(Span span, string label, Span labelSpan)
        {
            throw new NotImplementedException();
        }

        public LangElement HaltCompiler(Span span)
        {
            return new HaltCompiler(span);
        }

        public LangElement If(Span span, LangElement cond, LangElement body, LangElement elseOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Inclusion(Span span, bool conditional, InclusionTypes type, LangElement fileNameExpression)
        {
            throw new NotImplementedException();
        }

        public LangElement IncrementDecrement(Span span, LangElement refexpression, bool inc, bool post)
        {
            throw new NotImplementedException();
        }

        public LangElement InlineHtml(Span span, string html)
        {
            return new EchoStmt(span, html);
        }

        public LangElement InstanceOf(Span span, LangElement expression, TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        public LangElement Jump(Span span, JumpStmt.Types type, LangElement exprOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Label(Span span, string label, Span labelSpan)
        {
            throw new NotImplementedException();
        }

        public LangElement LineComment(Span span, string content)
        {
            throw new NotImplementedException();
        }

        public LangElement List(Span span, IEnumerable<LangElement> targets)
        {
            throw new NotImplementedException();
        }

        public LangElement Literal(Span span, object value)
        {
            throw new NotImplementedException();
        }

        public LangElement Namespace(Span span, QualifiedName? name, Span nameSpan, IEnumerable<LangElement> statements, NamingContext context)
        {
            throw new NotImplementedException();
        }

        public LangElement Namespace(Span span, QualifiedName? name, Span nameSpan, LangElement block, NamingContext context)
        {
            throw new NotImplementedException();
        }

        public LangElement New(Span span, TypeRef classNameRef, IEnumerable<ActualParam> argsOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement NewArray(Span span, IEnumerable<Item> itemsOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement ParenthesisExpression(Span span, LangElement expression)
        {
            throw new NotImplementedException();
        }

        public LangElement PHPDoc(Span span, string content)
        {
            throw new NotImplementedException();
        }

        public LangElement Shell(Span span, LangElement command)
        {
            throw new NotImplementedException();
        }

        public LangElement Switch(Span span, LangElement value, LangElement block)
        {
            throw new NotImplementedException();
        }

        public LangElement TraitUse(Span span, IEnumerable<QualifiedName> traits, IEnumerable<TraitsUse.TraitAdaptation> adaptations)
        {
            throw new NotImplementedException();
        }

        public LangElement TryCatch(Span span, LangElement body, IEnumerable<CatchItem> catches, LangElement finallyBlockOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Type(Span span, bool conditional, PhpMemberAttributes attributes, Name name, Span nameSpan, IEnumerable<FormalTypeParam> typeParamsOpt, Tuple<GenericQualifiedName, Span> baseClassOpt, IEnumerable<Tuple<GenericQualifiedName, Span>> implements, IEnumerable<LangElement> members, Span blockSpan)
        {
            throw new NotImplementedException();
        }

        public LangElement UnaryOperation(Span span, Operations operation, LangElement expression)
        {
            throw new NotImplementedException();
        }

        public LangElement Variable(Span span, LangElement nameExpr, TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        public LangElement Variable(Span span, LangElement nameExpr, LangElement memberOfOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement Variable(Span span, VariableName name, TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        public LangElement Variable(Span span, VariableName name, LangElement memberOfOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement While(Span span, LangElement cond, LangElement body)
        {
            throw new NotImplementedException();
        }

        public LangElement Yield(Span span, LangElement keyOpt, LangElement valueOpt)
        {
            throw new NotImplementedException();
        }

        public LangElement YieldFrom(Span span, LangElement fromExpr)
        {
            throw new NotImplementedException();
        }
    }
}
