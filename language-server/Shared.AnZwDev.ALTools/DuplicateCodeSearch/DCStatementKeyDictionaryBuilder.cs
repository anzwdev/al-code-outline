using Microsoft.Dynamics.Nav.CodeAnalysis;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementKeyDictionaryBuilder : SyntaxRewriter
    {

        protected string _sourceFilePath;
        public ConvertedObsoleteState SkipObsoleteCodeLevel { get; }
        public DCStatementKeyDictionary StatementsDictionary { get; } = new DCStatementKeyDictionary();

        public DCStatementKeyDictionaryBuilder(ConvertedObsoleteState skipObsoleteCodeLevel)
        {
            SkipObsoleteCodeLevel = skipObsoleteCodeLevel;
        }

        public void VisitProjectFolder(string filePath)
        {
            string[] files = Directory.GetFiles(filePath, "*.al", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
                VisitFile(files[i]);
        }

        public void VisitFile(string filePath)
        {
            _sourceFilePath = filePath;
            SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(File.ReadAllText(_sourceFilePath));
            Visit(syntaxTree.GetRoot());
        }

        public override SyntaxNode VisitTriggerDeclaration(TriggerDeclarationSyntax node)
        {
            VisitMethodOrTriggerDeclaration(node, DCCodeBlockType.Trigger);
            return base.VisitTriggerDeclaration(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            VisitMethodOrTriggerDeclaration(node, DCCodeBlockType.Method);
            return base.VisitMethodDeclaration(node);
        }

        protected void VisitMethodOrTriggerDeclaration(MethodOrTriggerDeclarationSyntax node, DCCodeBlockType codeBlockType)
        {
            if ((ValidNode(node)) && (node.Body != null))
            {
                DCStatementsBlock statementsBlock = new DCStatementsBlock(_sourceFilePath, codeBlockType);
                AppendMethodOrTriggerHeader(node, statementsBlock);
                AppendStatement(node.Body, statementsBlock);
            }
        }

        protected void AppendMethodOrTriggerHeader(MethodOrTriggerDeclarationSyntax node, DCStatementsBlock statementsBlock)
        {
            AppendMethodOrTriggerKeyword(node, statementsBlock);
            AppendFullNode(node.Name, statementsBlock, true);
            AppendParametersList(node.ParameterList, statementsBlock);
            AppendReturnValue(node.ReturnValue, statementsBlock);
            AppendVariablesList(node.Variables, statementsBlock);
        }

        protected void AppendMethodOrTriggerKeyword(MethodOrTriggerDeclarationSyntax node, DCStatementsBlock statementsBlock)
        {
            switch (node)
            {
                case MethodDeclarationSyntax methodDeclaration:
                    AppendMethodKeyword(methodDeclaration, statementsBlock);
                    break;
                case TriggerDeclarationSyntax triggerDeclaration:
                    AppendTriggerKeyword(triggerDeclaration, statementsBlock);
                    break;
            }
        }

        protected void AppendMethodKeyword(MethodDeclarationSyntax node, DCStatementsBlock statementsBlock)
        {
#if BC
            if ((node.AccessModifier != null) && (node.AccessModifier.Kind.ConvertToLocalType() != ConvertedSyntaxKind.None))
                AppendToken(node.AccessModifier, statementsBlock);
#else
            if ((node.LocalKeyword != null) && (node.LocalKeyword.Kind.ConvertToLocalType() == ConvertedSyntaxKind.LocalKeyword))
                AppendToken(node.LocalKeyword, statementsBlock);
#endif
            AppendToken(node.ProcedureKeyword, statementsBlock);
        }

        protected void AppendTriggerKeyword(TriggerDeclarationSyntax node, DCStatementsBlock statementsBlock)
        {
            AppendToken(node.TriggerKeyword, statementsBlock);
        }

        protected void AppendReturnValue(ReturnValueSyntax node, DCStatementsBlock statementsBlock)
        {
            if ((node != null) && (node.DataType != null))
                AppendFullNode(node, statementsBlock, true);
        }

        protected void AppendParametersList(ParameterListSyntax node, DCStatementsBlock statementsBlock)
        {
            if (IsBracket(node.OpenParenthesisToken))
                AppendToken(node.OpenParenthesisToken, statementsBlock);
            if ((node?.Parameters != null) && (node.Parameters.Count > 0))
            {
                foreach (ParameterSyntax parameterNode in node.Parameters)
                    AppendParameter(parameterNode, statementsBlock);
            }
            if (IsBracket(node.CloseParenthesisToken))
                AppendToken(node.CloseParenthesisToken, statementsBlock);
        }

        protected bool IsBracket(SyntaxToken token)
        {
            if (token != null)
            {
                var kind = token.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.OpenBraceToken:
                    case ConvertedSyntaxKind.CloseBraceToken:
                    case ConvertedSyntaxKind.CloseBracketToken:
                    case ConvertedSyntaxKind.OpenBracketToken:
                    case ConvertedSyntaxKind.OpenParenToken:
                    case ConvertedSyntaxKind.CloseParenToken:
                        return true;
                }
            }
            return false;
        }

        protected void AppendParameter(ParameterSyntax node, DCStatementsBlock statementsBlock)
        {
            AppendFullNode(node, statementsBlock, true);
        }

        protected void AppendVariablesList(VarSectionSyntax node, DCStatementsBlock statementsBlock)
        {
            if ((node?.Variables != null) && (node.Variables.Count > 0))
            {
                AppendToken(node.VarKeyword, statementsBlock);
                foreach (var variable in node.Variables)
                {
                    AppendFullNode(node, statementsBlock, true);
                }
            }
        }

        protected void AppendStatement(StatementSyntax node, DCStatementsBlock statementsBlock)
        {
            switch (node)
            {
                case BlockSyntax blockSyntax:
                    AppendBlockStatement(blockSyntax, statementsBlock);
                    break;
                case RepeatStatementSyntax repeatStatementSyntax:
                    AppendRepeatStatement(repeatStatementSyntax, statementsBlock);
                    break;
                case WhileStatementSyntax whileStatementSyntax:
                    AppendWhileStatement(whileStatementSyntax, statementsBlock);
                    break;
                case ForStatementSyntax forStatementSyntax:
                    AppendForStatement(forStatementSyntax, statementsBlock);
                    break;
                case IfStatementSyntax ifStatementSyntax:
                    AppendIfStatement(ifStatementSyntax, statementsBlock);
                    break;
                case CaseStatementSyntax caseStatementSyntax:
                    AppendCaseStatement(caseStatementSyntax, statementsBlock);
                    break;
                case WithStatementSyntax withStatementSyntax:
                    AppendWithStatement(withStatementSyntax, statementsBlock);
                    break;
                default:
                    AppendFullStatement(node, statementsBlock);
                    break;
            }
        }

        protected void AppendBlockStatement(BlockSyntax blockSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(blockSyntax.BeginKeywordToken, statementsBlock);
            foreach (StatementSyntax statementSyntax in blockSyntax.Statements)
                AppendStatement(statementSyntax, statementsBlock);
            AppendToken(blockSyntax.EndKeywordToken, statementsBlock);
        }

        protected void AppendRepeatStatement(RepeatStatementSyntax repeatStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(repeatStatementSyntax.RepeatKeywordToken, statementsBlock);
            foreach (StatementSyntax statementSyntax in repeatStatementSyntax.Statements)
                AppendStatement(statementSyntax, statementsBlock);
            AppendToken(repeatStatementSyntax.UntilKeywordToken, statementsBlock);
            AppendExpression(repeatStatementSyntax.Condition, statementsBlock);
        }

        protected void AppendWhileStatement(WhileStatementSyntax whileStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(whileStatementSyntax.WhileKeywordToken, statementsBlock);
            AppendExpression(whileStatementSyntax.Condition, statementsBlock);
            AppendToken(whileStatementSyntax.DoKeywordToken, statementsBlock);
            AppendStatement(whileStatementSyntax.Statement, statementsBlock);
        }

        protected void AppendForStatement(ForStatementSyntax forStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(forStatementSyntax.ForKeywordToken, statementsBlock);
            AppendExpression(forStatementSyntax.LoopVariable, statementsBlock);
            AppendToken(forStatementSyntax.AssignToken, statementsBlock);
            AppendExpression(forStatementSyntax.InitialValue, statementsBlock);
            AppendToken(forStatementSyntax.OperatorKeywordToken, statementsBlock);
            AppendExpression(forStatementSyntax.EndValue, statementsBlock);
            AppendToken(forStatementSyntax.DoKeywordToken, statementsBlock);
            AppendStatement(forStatementSyntax.Statement, statementsBlock);
        }

        protected void AppendIfStatement(IfStatementSyntax ifStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(ifStatementSyntax.IfKeywordToken, statementsBlock);
            AppendExpression(ifStatementSyntax.Condition, statementsBlock);
            AppendToken(ifStatementSyntax.ThenKeywordToken, statementsBlock);
            AppendStatement(ifStatementSyntax.Statement, statementsBlock);
            if ((ifStatementSyntax.ElseKeywordToken != null) && (ifStatementSyntax.ElseKeywordToken.Kind.ConvertToLocalType() == ALSymbols.Internal.ConvertedSyntaxKind.ElseKeyword))
                AppendToken(ifStatementSyntax.ElseKeywordToken, statementsBlock);
            if (ifStatementSyntax.ElseStatement != null)
                AppendStatement(ifStatementSyntax.ElseStatement, statementsBlock);
        }

        protected void AppendCaseStatement(CaseStatementSyntax caseStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(caseStatementSyntax.CaseKeywordToken, statementsBlock);
            AppendExpression(caseStatementSyntax.Expression, statementsBlock);
            AppendToken(caseStatementSyntax.OfKeywordToken, statementsBlock);
            foreach (CaseLineSyntax caseLineSyntax in caseStatementSyntax.CaseLines)
                AppendCaseLineStatement(caseLineSyntax, statementsBlock);
            if (caseStatementSyntax.CaseElse != null)
                AppendCaseElseStatement(caseStatementSyntax.CaseElse, statementsBlock);
            AppendToken(caseStatementSyntax.EndKeywordToken, statementsBlock);
        }

        protected void AppendCaseLineStatement(CaseLineSyntax caseLineSyntax, DCStatementsBlock statementsBlock)
        {
            foreach (CodeExpressionSyntax codeExpressionSyntax in caseLineSyntax.Expressions)
                AppendExpression(codeExpressionSyntax, statementsBlock);
            AppendStatement(caseLineSyntax.Statement, statementsBlock);
        }

        protected void AppendCaseElseStatement(CaseElseSyntax caseElseSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(caseElseSyntax.ElseKeywordToken, statementsBlock);
            foreach (StatementSyntax statementSyntax in caseElseSyntax.ElseStatements)
                AppendStatement(statementSyntax, statementsBlock);
        }

        protected void AppendWithStatement(WithStatementSyntax withStatementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendToken(withStatementSyntax.WithKeywordToken, statementsBlock);
            AppendExpression(withStatementSyntax.WithId, statementsBlock);
            AppendToken(withStatementSyntax.DoKeywordToken, statementsBlock);
            AppendStatement(withStatementSyntax.Statement, statementsBlock);
        }

        protected void AppendFullStatement(StatementSyntax statementSyntax, DCStatementsBlock statementsBlock)
        {
            AppendFullNode(statementSyntax, statementsBlock);
        }

        protected void AppendExpression(ExpressionSyntax expressionSyntax, DCStatementsBlock statementsBlock)
        {
            AppendFullNode(expressionSyntax, statementsBlock);
        }

        protected void AppendToken(SyntaxToken token, DCStatementsBlock statementsBlock)
        {
            AppendStatementInstance(
                token.ToString().ToLower(),
                true,
                new Range(token.SyntaxTree.GetLineSpan(token.Span)), 
                statementsBlock);
        }

        protected void AppendFullNode(SyntaxNode node, DCStatementsBlock statementsBlock, bool ignore = false)
        {
            if (node != null)
            {
                StringBuilder keyBuilder = new StringBuilder();
                IEnumerable<SyntaxToken> tokens = node.DescendantTokens();
                if (tokens == null)
                    keyBuilder.Append(node.ToString());
                else
                    foreach (SyntaxToken token in node.DescendantTokens())
                        keyBuilder.Append(token.ToString());

                AppendStatementInstance(
                    keyBuilder.ToString().ToLower(),
                    ignore,
                    new Range(node.SyntaxTree.GetLineSpan(node.Span)),
                    statementsBlock);
            }
        }

        protected void AppendStatementInstance(string keyValue, bool ignore, Range range, DCStatementsBlock statementsBlock)
        {
            DCStatementKey key = this.StatementsDictionary.GetOrCreate(keyValue, ignore);
            DCStatementInstance statementInstance = new DCStatementInstance(statementsBlock, key, range, statementsBlock.Statements.Count);
            statementsBlock.Statements.Add(statementInstance);
            key.StatementInstances.Add(statementInstance);
        }

        private bool ValidNode(SyntaxNode node)
        {
            return
                (SkipObsoleteCodeLevel == ConvertedObsoleteState.None) ||
                (!node.IsInsideObsoleteSyntaxTreeBranch(SkipObsoleteCodeLevel));
        }

    }
}
