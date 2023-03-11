using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveBeginEndSyntaxRewriter : ALSyntaxRewriter
    {

        public RemoveBeginEndSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            //run checks
            bool hasDiagnostics = node.ContainsDiagnostics;
            SyntaxNode parent = node.Parent;
            ConvertedSyntaxKind parentKind = parent.Kind.ConvertToLocalType();
            bool isIfInIfElse = this.CheckIsIfInIfElse(node, parentKind);
            bool isInIfWithElse = this.CheckIsInIfWithElse(node, parentKind);

            SyntaxNode updatedNode = base.VisitBlock(node);

            if ((updatedNode != null) && (updatedNode is BlockSyntax blockSyntax) && (!hasDiagnostics))
            {
                if ((parentKind != ConvertedSyntaxKind.TriggerDeclaration) &&
                    (parentKind != ConvertedSyntaxKind.MethodDeclaration) &&
                    (blockSyntax.Statements != null) &&
                    (blockSyntax.Statements.Count <= 1) &&
                    (!isIfInIfElse))
                {
                    this.NoOfChanges++;

                    StatementSyntax newStatement;

                    if (blockSyntax.Statements.Count == 1)
                        newStatement = blockSyntax.Statements[0];
                    else
                        newStatement = SyntaxFactory.EmptyStatement();


                    //remove semicolon if inside if statement with else                        
                    if (isInIfWithElse)
                    {
                        //it is not possible to remove semicolon from empty statement, we have to leave begin..end
                        if (newStatement.Kind.ConvertToLocalType() == ConvertedSyntaxKind.EmptyStatement)
                            return updatedNode;
                        newStatement = this.RemoveSemicolonToken(newStatement);
                    }
                    else if (this.IsSemicolon(blockSyntax.SemicolonToken))
                        newStatement = this.AddSemicolonToken(newStatement);

                    //trivia
                    IEnumerable<SyntaxTrivia> leadingTrivia = node.GetLeadingTrivia();
                    IEnumerable<SyntaxTrivia> trailingTrivia = node.GetTrailingTrivia();
#if BC
                    leadingTrivia = leadingTrivia.MergeWith(node.BeginKeywordToken.GetAllTrivia());
                    trailingTrivia = node.EndKeywordToken.GetAllTrivia().MergeWith(trailingTrivia);
#endif
                    leadingTrivia = leadingTrivia.MergeWith(newStatement.GetLeadingTrivia());
                    trailingTrivia = newStatement.GetTrailingTrivia().MergeWith(trailingTrivia).RemoveEmptyLines();

                    newStatement = newStatement
                        .WithLeadingTrivia(leadingTrivia)
                        .WithTrailingTrivia(trailingTrivia);

                    return newStatement;
                }
            }

            return updatedNode;
        }

        protected bool IsSemicolon(SyntaxToken semicolonToken)
        {
            return (semicolonToken != null) && (semicolonToken.Kind.ConvertToLocalType() == ConvertedSyntaxKind.SemicolonToken);
        }

        protected StatementSyntax AddSemicolonToken(StatementSyntax node)
        {
            IEnumerable<SyntaxTrivia> trailingTrivia = null;
            SyntaxKind semicolonSyntaxKind = ConvertedSyntaxKind.SemicolonToken.Convert<ConvertedSyntaxKind, SyntaxKind>();

            switch (node)
            {
                case BlockSyntax blockSyntax:
                    if (this.IsSemicolon(blockSyntax.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, blockSyntax.SemicolonToken);
                    node = blockSyntax.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case AssignmentStatementSyntax assignmentStatement:
                    if (this.IsSemicolon(assignmentStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, assignmentStatement.SemicolonToken);
                    node = assignmentStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case BreakStatementSyntax breakStatement:
                    if (this.IsSemicolon(breakStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, breakStatement.SemicolonToken);
                    node = breakStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case CaseStatementSyntax caseStatement:
                    if (this.IsSemicolon(caseStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, caseStatement.SemicolonToken);
                    node = caseStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case CompoundAssignmentStatementSyntax compoundAssignmentStatement:
                    if (this.IsSemicolon(compoundAssignmentStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, compoundAssignmentStatement.SemicolonToken);
                    node = compoundAssignmentStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case EmptyStatementSyntax emptyStatement:
                    if (this.IsSemicolon(emptyStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, emptyStatement.SemicolonToken);
                    node = emptyStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case ExitStatementSyntax exitStatement:
                    if (this.IsSemicolon(exitStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, exitStatement.SemicolonToken);
                    node = exitStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case ExpressionStatementSyntax expressionStatement:
                    if (this.IsSemicolon(expressionStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, expressionStatement.SemicolonToken);
                    node = expressionStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case RepeatStatementSyntax repeatStatement:
                    if (this.IsSemicolon(repeatStatement.SemicolonToken))
                        return node;
                    trailingTrivia = this.GetStatementTrailingTrivia(node, repeatStatement.SemicolonToken);
                    node = repeatStatement.WithTrailingTrivia().WithSemicolonToken(SyntaxFactory.Token(semicolonSyntaxKind));
                    break;
                case AssertErrorStatementSyntax assertErrorStatement:
                    node = assertErrorStatement.WithStatement(this.AddSemicolonToken(assertErrorStatement.Statement));
                    break;
                case ForEachStatementSyntax forEachStatement:
                    node = forEachStatement.WithStatement(this.AddSemicolonToken(forEachStatement.Statement));
                    break;
                case ForStatementSyntax forStatement:
                    node = forStatement.WithStatement(this.AddSemicolonToken(forStatement.Statement));
                    break;
                case IfStatementSyntax ifStatement:
                    if (ifStatement.ElseStatement != null)
                        node = ifStatement.WithElseStatement(this.AddSemicolonToken(ifStatement.ElseStatement));
                    else
                        node = ifStatement.WithStatement(this.AddSemicolonToken(ifStatement.Statement));
                    break;
                case OrphanedElseStatementSyntax orphanedElseStatement:
                    node = orphanedElseStatement.WithElseStatement(this.AddSemicolonToken(orphanedElseStatement.ElseStatement));
                    break;
                case WhileStatementSyntax whileStatement:
                    node = whileStatement.WithStatement(this.AddSemicolonToken(whileStatement.Statement));
                    break;
                case WithStatementSyntax withStatement:
                    node = withStatement.WithStatement(this.AddSemicolonToken(withStatement.Statement));
                    break;
            }
            if (trailingTrivia != null)
                node = node.WithTrailingTrivia(trailingTrivia);
            return node;
        }

        protected StatementSyntax RemoveSemicolonToken(StatementSyntax node)
        {
            IEnumerable<SyntaxTrivia> trailingTrivia = null;

            switch (node)
            {
                case BlockSyntax blockSyntax:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, blockSyntax.SemicolonToken);
                    node = blockSyntax.WithSemicolonToken(default(SyntaxToken));
                    break;
                case AssignmentStatementSyntax assignmentStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, assignmentStatement.SemicolonToken);
                    node = assignmentStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case BreakStatementSyntax breakStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, breakStatement.SemicolonToken);
                    node = breakStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case CaseStatementSyntax caseStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, caseStatement.SemicolonToken);
                    node = caseStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case CompoundAssignmentStatementSyntax compoundAssignmentStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, compoundAssignmentStatement.SemicolonToken);
                    node = compoundAssignmentStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case EmptyStatementSyntax emptyStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, emptyStatement.SemicolonToken);
                    node = emptyStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case ExitStatementSyntax exitStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, exitStatement.SemicolonToken);
                    node = exitStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case ExpressionStatementSyntax expressionStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, expressionStatement.SemicolonToken);
                    node = expressionStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case RepeatStatementSyntax repeatStatement:
                    trailingTrivia = this.GetStatementTrailingTrivia(node, repeatStatement.SemicolonToken);
                    node = repeatStatement.WithSemicolonToken(default(SyntaxToken));
                    break;
                case AssertErrorStatementSyntax assertErrorStatement:
                    node = assertErrorStatement.WithStatement(this.RemoveSemicolonToken(assertErrorStatement.Statement));
                    break;
                case ForEachStatementSyntax forEachStatement:
                    node = forEachStatement.WithStatement(this.RemoveSemicolonToken(forEachStatement.Statement));
                    break;
                case ForStatementSyntax forStatement:
                    node = forStatement.WithStatement(this.RemoveSemicolonToken(forStatement.Statement));
                    break;
                case IfStatementSyntax ifStatement:
                    if (ifStatement.ElseStatement != null)
                        node = ifStatement.WithElseStatement(this.RemoveSemicolonToken(ifStatement.ElseStatement));
                    else
                        node = ifStatement.WithStatement(this.RemoveSemicolonToken(ifStatement.Statement));
                    break;
                case OrphanedElseStatementSyntax orphanedElseStatement:
                    node = orphanedElseStatement.WithElseStatement(this.RemoveSemicolonToken(orphanedElseStatement.ElseStatement));
                    break;
                case WhileStatementSyntax whileStatement:
                    node = whileStatement.WithStatement(this.RemoveSemicolonToken(whileStatement.Statement));
                    break;
                case WithStatementSyntax withStatement:
                    node = withStatement.WithStatement(this.RemoveSemicolonToken(withStatement.Statement));
                    break;
            }
            if (trailingTrivia != null)
                node = node.WithTrailingTrivia(trailingTrivia);
            return node;
        }

        protected IEnumerable<SyntaxTrivia> GetStatementTrailingTrivia(SyntaxNode node, SyntaxToken semicolonToken)
        {
#if BC
            if (semicolonToken != null)
            {
                List<SyntaxTrivia> trailingTrivia = new List<SyntaxTrivia>();
                trailingTrivia.AddRange(semicolonToken.GetAllTrivia());
                trailingTrivia.AddUniqueRange(node.GetTrailingTrivia());
                //return semicolonToken.GetAllTrivia(); //.MergeWith(node.GetTrailingTrivia());
                return trailingTrivia;
            }
#endif
            return null;
        }

        private bool CheckIsInIfWithElse(BlockSyntax node, ConvertedSyntaxKind parentKind)
        {
            return (parentKind == ConvertedSyntaxKind.IfStatement) &&
                (node.Parent is IfStatementSyntax parentIf) &&
                (parentIf.Statement == node) &&
                (parentIf.ElseStatement != null);
        }

        private bool CheckIsIfInIfElse(BlockSyntax node, ConvertedSyntaxKind parentKind)
        {
            return (parentKind == ConvertedSyntaxKind.IfStatement) &&
                (node.Statements.Count == 1) &&
                (node.Statements[0].Kind.ConvertToLocalType() == ConvertedSyntaxKind.IfStatement) &&
                (node.Parent is IfStatementSyntax parentIf) &&
                (parentIf.Statement == node) &&
                (parentIf.ElseStatement != null);
        }

    }
}
