﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devsense.PHP.Syntax.Ast;
using System.Diagnostics;

namespace Devsense.PHP.Syntax
{
    /// <summary>
    /// Options specifying how <see cref="TokenVisitor"/> synthesizes tokens from the syntax tree.
    /// </summary>
    public interface ITokenVisitorOptions
    {
        /// <summary>
        /// Whether to tokenize according to the old syntax <code>array(...)</code> or the new syntax <code>[...]</code>
        /// </summary>
        bool OldArraySyntax { get; }
    }

    public class TokenVisitor : TreeContextVisitor
    {
        #region DefaultTokenVisitorOptions

        sealed class DefaultTokenVisitorOptions : ITokenVisitorOptions
        {
            public static ITokenVisitorOptions Instance = new DefaultTokenVisitorOptions();

            private DefaultTokenVisitorOptions() { }

            public bool OldArraySyntax => false;
        }

        #endregion

        readonly ITokenVisitorOptions _options;

        public TokenVisitor(TreeContext initialContext, ITokenVisitorOptions options = null) : base(initialContext)
        {
            _options = options ?? DefaultTokenVisitorOptions.Instance;
        }

        /// <summary>
        /// Invoked when a token is visited.
        /// </summary>
        /// <param name="token">Token id.</param>
        /// <param name="text">Textual representation of <paramref name="token"/>.</param>
        /// <param name="semantic">Optional token semantic value.
        /// In case of string literals, numbers or comments, this specifies its original representation in source code.</param>
        protected virtual void VisitToken(Tokens token, string text, object semantic = null)
        {

        }

        #region Single Nodes Overrides

        public override void VisitElement(LangElement element)
        {
            base.VisitElement(element);
        }

        public override void VisitActualParam(ActualParam x)
        {
            if (x.IsUnpack) VisitToken(Tokens.T_ELLIPSIS, "...");
            if (x.Ampersand) VisitToken(Tokens.T_AMP, "&");
            VisitElement(x.Expression);
        }

        public override void VisitAnonymousTypeDecl(AnonymousTypeDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitAnonymousTypeRef(AnonymousTypeRef x)
        {
            base.VisitAnonymousTypeRef(x);
        }

        public override void VisitArrayEx(ArrayEx x)
        {
            if (_options.OldArraySyntax)
            {
                VisitToken(Tokens.T_ARRAY, "array");
                VisitToken(Tokens.T_LPAREN, "(");
                VisitElementList(x.Items, VisitArrayItem, Tokens.T_COMMA, ",");
                VisitToken(Tokens.T_RPAREN, ")");
            }
            else
            {
                VisitToken(Tokens.T_LBRACKET, "[");
                VisitElementList(x.Items, VisitArrayItem, Tokens.T_COMMA, ",");
                VisitToken(Tokens.T_RBRACKET, "]");
            }
        }

        public override void VisitArrayItem(Item item)
        {
            throw new NotImplementedException();
        }

        public override void VisitAssertEx(AssertEx x)
        {
            VisitToken(Tokens.T_STRING, "assert");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElement(x.CodeEx);
            VisitToken(Tokens.T_RPAREN, ")");
        }

        public sealed override void VisitAssignEx(AssignEx x) { throw new InvalidOperationException(); }

        public override void VisitBinaryEx(BinaryEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitBinaryStringLiteral(BinaryStringLiteral x)
        {
            throw new NotImplementedException();
        }

        public override void VisitBlockStmt(BlockStmt x)
        {
            VisitToken(Tokens.T_LBRACE, "{");
            base.VisitBlockStmt(x);
            VisitToken(Tokens.T_RBRACE, "}");
        }

        public override void VisitBoolLiteral(BoolLiteral x)
        {
            VisitToken(Tokens.T_STRING, x.Value.ToString().ToLowerInvariant());
        }

        public override void VisitCaseItem(CaseItem x)
        {
            VisitToken(Tokens.T_CASE, "case");
            VisitElement(x.CaseVal);
            VisitToken(Tokens.T_COLON, ":");

            base.VisitCaseItem(x);
        }

        public override void VisitCatchItem(CatchItem x)
        {
            // catch (TYPE VARIABLE) BLOCK
            using (new ScopeHelper(this, x))
            {
                VisitToken(Tokens.T_CATCH, "catch");
                VisitToken(Tokens.T_LPAREN, "(");
                VisitElement(x.TargetType);
                VisitToken(Tokens.T_RPAREN, ")");
                VisitElement(x.Body);
            }
        }

        public override void VisitClassConstantDecl(ClassConstantDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitClassConstUse(ClassConstUse x)
        {
            throw new NotImplementedException();
        }

        public override void VisitClassTypeRef(ClassTypeRef x)
        {
            throw new NotImplementedException();
        }

        public override void VisitConcatEx(ConcatEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitConditionalEx(ConditionalEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitConditionalStmt(ConditionalStmt x)
        {
            throw new NotImplementedException();
        }

        public override void VisitConstantDecl(ConstantDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitConstantUse(ConstantUse x)
        {
            throw new NotImplementedException();
        }

        public override void VisitConstDeclList(ConstDeclList x)
        {
            throw new NotImplementedException();
        }

        public override void VisitCustomAttribute(CustomAttribute x)
        {
            throw new NotImplementedException();
        }

        public override void VisitDeclareStmt(DeclareStmt x)
        {
            throw new NotImplementedException();
        }

        public override void VisitDefaultItem(DefaultItem x)
        {
            VisitToken(Tokens.T_DEFAULT, "default");
            VisitToken(Tokens.T_COLON, ":");
            VisitList(x.Statements);
        }

        public override void VisitDirectFcnCall(DirectFcnCall x)
        {
            VisitIsMemberOf(x.IsMemberOf);
            VisitQualifiedName(x.FullName.OriginalName);
            VisitCallSignature(x.CallSignature);
        }

        public override void VisitDirectStFldUse(DirectStFldUse x)
        {
            VisitElement(x.TargetType);
            VisitToken(Tokens.T_DOUBLE_COLON, "::");
            VisitVariableName(x.PropertyName);  // $name
        }

        public override void VisitDirectStMtdCall(DirectStMtdCall x)
        {
            VisitElement(x.TargetType);
            VisitToken(Tokens.T_DOUBLE_COLON, "::");
            VisitToken(Tokens.T_STRING, x.MethodName.Name.Value);
            VisitCallSignature(x.CallSignature);
        }

        public override void VisitDirectVarUse(DirectVarUse x)
        {
            VisitIsMemberOf(x.IsMemberOf);
            VisitVariableName(x.VarName);
        }

        public override void VisitDoubleLiteral(DoubleLiteral x)
        {
            throw new NotImplementedException();
        }

        public override void VisitEchoStmt(EchoStmt x)
        {
            if (x.IsHtmlCode)
            {
                VisitToken(Tokens.T_INLINE_HTML, ((StringLiteral)x.Parameters[0]).Value);
            }
            else
            {
                // echo PARAMETERS;
                VisitToken(Tokens.T_ECHO, "echo");
                VisitElementList(x.Parameters, Tokens.T_COMMA, ",");
                VisitToken(Tokens.T_SEMI, ";");
            }
        }

        public override void VisitEmptyEx(EmptyEx x)
        {
            // empty(OPERAND)
            VisitToken(Tokens.T_EMPTY, "empty");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElement(x.Expression);
            VisitToken(Tokens.T_RPAREN, ")");
        }

        public override void VisitEmptyStmt(EmptyStmt x)
        {
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitEvalEx(EvalEx x)
        {
            VisitToken(Tokens.T_EVAL, "eval");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElement(x.Code);
            VisitToken(Tokens.T_RPAREN, ")");
        }

        public override void VisitExitEx(ExitEx x)
        {
            VisitToken(Tokens.T_EXIT, "exit");
            VisitElement(x.ResulExpr);
        }

        public override void VisitExpressionStmt(ExpressionStmt x)
        {
            base.VisitExpressionStmt(x);
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitFieldDecl(FieldDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitFieldDeclList(FieldDeclList x)
        {
            throw new NotImplementedException();
        }

        public override void VisitFinallyItem(FinallyItem x)
        {
            // finally BLOCK
            using (new ScopeHelper(this, x))
            {
                VisitToken(Tokens.T_FINAL, "finally");
                VisitElement(x.Body);
            }
        }

        public override void VisitForeachStmt(ForeachStmt x)
        {
            throw new NotImplementedException();
        }

        public override void VisitForeachVar(ForeachVar x)
        {
            throw new NotImplementedException();
        }

        public override void VisitFormalParam(FormalParam x)
        {
            VisitElement(x.TypeHint);
            if (x.PassedByRef)
            {
                VisitToken(Tokens.T_AMP, "&");
            }
            if (x.IsVariadic)
            {
                VisitToken(Tokens.T_ELLIPSIS, "...");
            }

            VisitVariableName(x.Name.Name);
        }

        public override void VisitFormalTypeParam(FormalTypeParam x)
        {
            throw new NotImplementedException();
        }

        public override void VisitForStmt(ForStmt x)
        {
            using (new ScopeHelper(this, x))
            {
                VisitToken(Tokens.T_FOR, "for");
                VisitToken(Tokens.T_LPAREN, "(");

                VisitElementList(x.InitExList, Tokens.T_COMMA, ",");
                VisitToken(Tokens.T_SEMI, ";");
                VisitElementList(x.CondExList, Tokens.T_COMMA, ",");
                VisitToken(Tokens.T_SEMI, ";");
                VisitElementList(x.ActionExList, Tokens.T_COMMA, ",");

                VisitToken(Tokens.T_LPAREN, ")");

                VisitElement(x.Body);
            }
        }

        public override void VisitFunctionCall(FunctionCall x)
        {
            throw new NotImplementedException();
        }

        public override void VisitFunctionDecl(FunctionDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitGenericTypeRef(GenericTypeRef x)
        {
            throw new NotImplementedException();
        }

        public override void VisitGlobalCode(GlobalCode x)
        {
            base.VisitGlobalCode(x);
        }

        public override void VisitGlobalConstantDecl(GlobalConstantDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitGlobalConstDeclList(GlobalConstDeclList x)
        {
            throw new NotImplementedException();
        }

        public override void VisitGlobalConstUse(GlobalConstUse x)
        {
            throw new NotImplementedException();
        }

        public override void VisitGlobalStmt(GlobalStmt x)
        {
            throw new NotImplementedException();
        }

        public override void VisitGotoStmt(GotoStmt x)
        {
            VisitToken(Tokens.T_GOTO, "goto");
            VisitToken(Tokens.T_STRING, x.LabelName.Name.Value);
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitHaltCompiler(HaltCompiler x)
        {
            VisitToken(Tokens.T_HALT_COMPILER, "__halt_compiler");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitToken(Tokens.T_RPAREN, ")");
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitIfStmt(IfStmt x)
        {
            throw new NotImplementedException();
        }

        public override void VisitIncDecEx(IncDecEx x)
        {
            if (x.Post == true)
            {
                VisitElement(x.Variable);
            }

            // ++/--
            VisitToken(x.Inc ? Tokens.T_INC : Tokens.T_DEC, x.Inc ? "++" : "--");

            if (x.Post == false)
            {
                VisitElement(x.Variable);
            }
        }

        public override void VisitIncludingEx(IncludingEx x)
        {
            switch (x.InclusionType)
            {
                case InclusionTypes.Include:
                    VisitToken(Tokens.T_INCLUDE, "include");
                    break;
                case InclusionTypes.IncludeOnce:
                    VisitToken(Tokens.T_INCLUDE_ONCE, "include_once");
                    break;
                case InclusionTypes.Require:
                    VisitToken(Tokens.T_REQUIRE, "require");
                    break;
                case InclusionTypes.RequireOnce:
                    VisitToken(Tokens.T_REQUIRE_ONCE, "require_once");
                    break;

                default:
                    throw new NotImplementedException();// ??
            }

            VisitElement(x.Target);
        }

        public override void VisitIndirectFcnCall(IndirectFcnCall x)
        {
            VisitIsMemberOf(x.IsMemberOf);
            VisitElement(x.NameExpr);
            VisitCallSignature(x.CallSignature);
        }

        public virtual void VisitVariableName(VariableName name)
        {
            VisitToken(Tokens.T_VARIABLE, "$" + name.Value);
        }

        public virtual void VisitQualifiedName(QualifiedName qname)
        {
            if (qname.IsFullyQualifiedName)
            {
                VisitToken(Tokens.T_NS_SEPARATOR, QualifiedName.Separator.ToString());
            }

            var ns = qname.Namespaces;
            for (int i = 0; i < ns.Length; i++)
            {
                VisitToken(Tokens.T_STRING, ns[i].Value);
                VisitToken(Tokens.T_NS_SEPARATOR, QualifiedName.Separator.ToString());
            }
        }

        public virtual void VisitCallSignature(CallSignature signature)
        {
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElementList(signature.Parameters, Tokens.T_COMMA, ",");
            VisitToken(Tokens.T_RPAREN, ")");
        }

        public override void VisitIndirectStFldUse(IndirectStFldUse x)
        {
            VisitElement(x.TargetType);
            VisitToken(Tokens.T_DOUBLE_COLON, "::");
            VisitElement(x.FieldNameExpr);  // TODO: { ... } ?
        }

        public override void VisitIndirectStMtdCall(IndirectStMtdCall x)
        {
            VisitElement(x.TargetType);
            VisitToken(Tokens.T_DOUBLE_COLON, "::");
            VisitElement(x.MethodNameVar);  // TODO: { ... } ?
            VisitCallSignature(x.CallSignature);
        }

        public override void VisitIndirectTypeRef(IndirectTypeRef x)
        {
            VisitElement(x.ClassNameVar);
        }

        public override void VisitIndirectVarUse(IndirectVarUse x)
        {
            VisitIsMemberOf(x.IsMemberOf);
            VisitElement(x.VarNameEx);    // TODO: { ... } ?
        }

        public override void VisitInstanceOfEx(InstanceOfEx x)
        {
            VisitElement(x.Expression);
            VisitToken(Tokens.T_INSTANCEOF, "instanceof");
            VisitElement(x.ClassNameRef);
        }

        public override void VisitIssetEx(IssetEx x)
        {
            VisitToken(Tokens.T_ISSET, "isset");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElementList(x.VarList, Tokens.T_COMMA, ",");
            VisitToken(Tokens.T_RPAREN, ")");
        }

        public override void VisitItemUse(ItemUse x)
        {
            throw new NotImplementedException();
        }

        public virtual void VisitIsMemberOf(Expression isMemberOf)
        {
            if (isMemberOf != null)
            {
                VisitElement(isMemberOf);
                VisitToken(Tokens.T_OBJECT_OPERATOR, "->");
            }
        }

        public override void VisitJumpStmt(JumpStmt x)
        {
            switch (x.Type)
            {
                case JumpStmt.Types.Return:
                    VisitToken(Tokens.T_RETURN, "return");
                    break;
                case JumpStmt.Types.Continue:
                    VisitToken(Tokens.T_CONTINUE, "continue");
                    break;
                case JumpStmt.Types.Break:
                    VisitToken(Tokens.T_BREAK, "break");
                    break;
            }

            VisitElement(x.Expression);

            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitLabelStmt(LabelStmt x)
        {
            VisitToken(Tokens.T_STRING, x.Name.Name.Value);
            VisitToken(Tokens.T_COLON, ":");
        }

        public override void VisitLambdaFunctionExpr(LambdaFunctionExpr x)
        {
            throw new NotImplementedException();
        }

        public override void VisitListEx(ListEx x)
        {
            throw new NotImplementedException();
        }

        protected virtual void VisitElementList<TElement>(IList<TElement> list, Tokens separatorToken, string separatorTokenText) where TElement : LangElement
        {
            VisitElementList(list, VisitElement, separatorToken, separatorTokenText);
        }

        protected virtual void VisitElementList<TElement>(IList<TElement> list, Action<TElement> action, Tokens separatorToken, string separatorTokenText)
        {
            Debug.Assert(list != null, nameof(list));
            Debug.Assert(action != null, nameof(action));

            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0) VisitToken(separatorToken, separatorTokenText);
                action(list[i]);
            }
        }

        public override void VisitLongIntLiteral(LongIntLiteral x)
        {
            //VisitToken(Tokens.T_LNUMBER, x.Value, ...)
            throw new NotImplementedException();
        }

        public override void VisitMethodDecl(MethodDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitMultipleTypeRef(MultipleTypeRef x)
        {
            throw new NotImplementedException();
        }

        public override void VisitNamedActualParam(NamedActualParam x)
        {
            throw new NotImplementedException();
        }

        public override void VisitNamedTypeDecl(NamedTypeDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitNamespaceDecl(NamespaceDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitNewEx(NewEx x)
        {
            VisitToken(Tokens.T_NEW, "new");
            VisitElement(x.ClassNameRef);
            VisitCallSignature(x.CallSignature);
        }

        public override void VisitNullableTypeRef(NullableTypeRef x)
        {
            VisitToken(Tokens.T_QUESTION, "?");
            VisitElement(x.TargetType);
        }

        public override void VisitNullLiteral(NullLiteral x)
        {
            VisitToken(Tokens.T_STRING, "null");
        }

        public override void VisitPHPDocBlock(PHPDocBlock x)
        {
            throw new NotImplementedException();
        }

        public override void VisitPHPDocStmt(PHPDocStmt x)
        {
            base.VisitPHPDocStmt(x);
        }

        public override void VisitPrimitiveTypeRef(PrimitiveTypeRef x)
        {
            throw new NotImplementedException();
        }

        public override void VisitPseudoClassConstUse(PseudoClassConstUse x)
        {
            throw new NotImplementedException();
        }

        public override void VisitPseudoConstUse(PseudoConstUse x)
        {
            switch (x.Type)
            {
                case PseudoConstUse.Types.Class: VisitToken(Tokens.T_CLASS_C, "__CLASS__"); break;
                case PseudoConstUse.Types.Trait: VisitToken(Tokens.T_TRAIT_C, "__TRAIT__"); break;
                case PseudoConstUse.Types.Namespace: VisitToken(Tokens.T_NS_C, "__NAMESPACE__"); break;
                case PseudoConstUse.Types.Function: VisitToken(Tokens.T_FUNC_C, "__FUNCTION__"); break;
                case PseudoConstUse.Types.Method: VisitToken(Tokens.T_METHOD_C, "__METHOD__"); break;
                case PseudoConstUse.Types.File: VisitToken(Tokens.T_FILE, "__FILE__"); break;
                case PseudoConstUse.Types.Line: VisitToken(Tokens.T_LINE, "__LINE__"); break;
                case PseudoConstUse.Types.Dir: VisitToken(Tokens.T_DIR, "__DIR__"); break;
                default:
                    throw new ArgumentException();
            }
        }

        public override void VisitRefAssignEx(RefAssignEx x)
        {
            // L =& R
            VisitElement(x.LValue);
            VisitToken(Tokens.T_EQ, "=");
            VisitToken(Tokens.T_AMP, "&");
            VisitElement(x.RValue);
        }

        public override void VisitRefItem(RefItem x)
        {
            throw new NotImplementedException();
        }

        public override void VisitReservedTypeRef(ReservedTypeRef x)
        {
            VisitQualifiedName(x.QualifiedName.Value);
        }

        public override void VisitShellEx(ShellEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitStaticStmt(StaticStmt x)
        {
            VisitToken(Tokens.T_STATIC, "static");
            VisitElementList(x.StVarList, Tokens.T_COMMA, ",");
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitStaticVarDecl(StaticVarDecl x)
        {
            VisitVariableName(x.Variable);

            if (x.Initializer != null)
            {
                VisitToken(Tokens.T_EQ, "=");
                VisitElement(x.Initializer);
            }
        }

        public override void VisitStringLiteral(StringLiteral x)
        {
            throw new NotImplementedException();
        }

        public override void VisitStringLiteralDereferenceEx(StringLiteralDereferenceEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitSwitchItem(SwitchItem x)
        {
            throw new NotImplementedException();
        }

        public override void VisitSwitchStmt(SwitchStmt x)
        {
            // switch(VALUE){CASES}
            VisitToken(Tokens.T_SWITCH, "switch");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElement(x.SwitchValue);
            VisitToken(Tokens.T_RPAREN, ")");
            VisitToken(Tokens.T_LBRACE, "{");
            VisitList(x.SwitchItems);
            VisitToken(Tokens.T_RBRACE, "}");
        }

        public override void VisitThrowStmt(ThrowStmt x)
        {
            // throw EXPR;
            VisitToken(Tokens.T_THROW, "throw");
            VisitElement(x.Expression);
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitTraitAdaptationAlias(TraitsUse.TraitAdaptationAlias x)
        {
            throw new NotImplementedException();
        }

        public override void VisitTraitAdaptationBlock(TraitAdaptationBlock x)
        {
            throw new NotImplementedException();
        }

        public override void VisitTraitAdaptationPrecedence(TraitsUse.TraitAdaptationPrecedence x)
        {
            throw new NotImplementedException();
        }

        public override void VisitTraitsUse(TraitsUse x)
        {
            throw new NotImplementedException();
        }

        public override void VisitTranslatedTypeRef(TranslatedTypeRef x)
        {
            VisitElement(x.OriginalType);
        }

        public override void VisitTryStmt(TryStmt x)
        {
            using (new ScopeHelper(this, x))
            {
                VisitToken(Tokens.T_TRY, "try");
                VisitElement(x.Body);
                VisitList(x.Catches);
                VisitElement(x.FinallyItem);
            }
        }

        public override void VisitTypeDecl(TypeDecl x)
        {
            throw new NotImplementedException();
        }

        public override void VisitTypeOfEx(TypeOfEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitUnaryEx(UnaryEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitUnsetStmt(UnsetStmt x)
        {
            VisitToken(Tokens.T_UNSET, "unset");
            VisitToken(Tokens.T_LPAREN, "(");
            VisitElementList(x.VarList, Tokens.T_COMMA, ",");
            VisitToken(Tokens.T_RPAREN, ")");
            VisitToken(Tokens.T_SEMI, ";");
        }

        public override void VisitUseStatement(UseStatement x)
        {
            throw new NotImplementedException();
        }

        public override void VisitValueAssignEx(ValueAssignEx x)
        {
            // L = R
            VisitElement(x.LValue);
            VisitToken(Tokens.T_EQ, "=");
            VisitElement(x.RValue);
        }

        public override void VisitValueItem(ValueItem x)
        {
            throw new NotImplementedException();
        }

        public override void VisitVarLikeConstructUse(VarLikeConstructUse x)
        {
            throw new NotImplementedException();
        }

        public override void VisitWhileStmt(WhileStmt x)
        {
            using (new ScopeHelper(this, x))
            {
                VisitToken(Tokens.T_WHILE, "while");
                VisitToken(Tokens.T_LPAREN, "(");
                VisitElement(x.CondExpr);
                VisitToken(Tokens.T_RPAREN, ")");
                VisitElement(x.Body);
            }
        }

        public override void VisitYieldEx(YieldEx x)
        {
            throw new NotImplementedException();
        }

        public override void VisitYieldFromEx(YieldFromEx x)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
