using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class WithRemoveSyntaxRewriter : ALSyntaxRewriter
    {

        public WithRemoveSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            //make sure that we don't have begin/else inside block of statements after removing "with"
            SyntaxList<StatementSyntax> statements = node.Statements;
            SyntaxList<StatementSyntax> newStatements;
            if (ExpandWithStatements(statements, out newStatements))
                return node.WithStatements(newStatements);

            return base.VisitBlock(node);
        }

        public override SyntaxNode VisitRepeatStatement(RepeatStatementSyntax node)
        {
            //make sure that we don't have begin/else inside block of statements after removing "with"
            SyntaxList<StatementSyntax> statements = node.Statements;
            SyntaxList<StatementSyntax> newStatements;
            if (ExpandWithStatements(statements, out newStatements))
                return node.WithStatements(newStatements);

            return base.VisitRepeatStatement(node);
        }

        public override SyntaxNode VisitCaseElse(CaseElseSyntax node)
        {
            //make sure that we don't have begin/else inside block of statements after removing "with"
            SyntaxList<StatementSyntax> statements = node.ElseStatements;
            SyntaxList<StatementSyntax> newStatements;
            if (ExpandWithStatements(statements, out newStatements))
                return node.WithElseStatements(newStatements);

            return base.VisitCaseElse(node);
        }

        public override SyntaxNode VisitWithStatement(WithStatementSyntax node)
        {
            //remove "with" but keep trivia as they might contain important information
            this.NoOfChanges++;
            SyntaxNode newNode = node.Statement //.WithTriviaFrom(node);
                .WithLeadingTrivia(
                    node.GetLeadingTrivia()
                    .AddRange(node.Statement.GetLeadingTrivia())
                    .NormalizeSyntaxTriviaList())
                .WithTrailingTrivia(
                    node.Statement.GetTrailingTrivia()
                    .NormalizeSyntaxTriviaList());// .AddRange(node.GetTrailingTrivia()));
            return this.Visit(newNode);
        }

        protected bool ExpandWithStatements(SyntaxList<StatementSyntax> statements, out SyntaxList<StatementSyntax> newStatements)
        {
            //check if list contains "with" syntax
            if (!statements.Where(p => (p.Kind.ConvertToLocalType() == ConvertedSyntaxKind.WithStatement)).Any())
            {
                newStatements = statements;
                return false;
            }

            //collect new list of statements
            List<StatementSyntax> newStatementsList = new List<StatementSyntax>();
            foreach (StatementSyntax statementSyntax in statements)
            {
                if (statementSyntax.Kind.ConvertToLocalType() == ConvertedSyntaxKind.WithStatement)
                {
                    SyntaxNode newWithNode = this.Visit(statementSyntax);
                    if (newWithNode.Kind.ConvertToLocalType() == ConvertedSyntaxKind.Block)
                    {
                        BlockSyntax newWithBlock = (BlockSyntax)newWithNode;

                        if (newWithBlock.Statements.Count > 0)
                        {
                            int newStartIdx = newStatementsList.Count;
                            newStatementsList.AddRange(newWithBlock.Statements);

                            //copy leading and trailing trivia from block to the first and last block statements
                            //as they might contain important information
                            newStatementsList[newStartIdx] = newStatementsList[newStartIdx]
                                .WithLeadingTrivia(
                                    newWithBlock.GetLeadingTrivia()
                                    .AddRange(newStatementsList[newStartIdx].GetLeadingTrivia())
                                    .NormalizeSyntaxTriviaList());
                            int newEndIdx = newStatementsList.Count - 1;

                            newStatementsList[newEndIdx] = newStatementsList[newEndIdx]
                                .WithTrailingTrivia(
                                    newStatementsList[newEndIdx].GetTrailingTrivia()
                                    .AddRange(newWithBlock.EndKeywordToken.LeadingTrivia)
                                    .AddRange(newWithBlock.GetTrailingTrivia())
                                    .NormalizeSyntaxTriviaList());
                        }
                    }
                    else
                        newStatementsList.Add((StatementSyntax)newWithNode);
                }
                else
                    newStatementsList.Add(statementSyntax);
            }

            this.NoOfChanges++;
            newStatements = SyntaxFactory.List<StatementSyntax>(newStatementsList);
            return true;
        }

    }
}
