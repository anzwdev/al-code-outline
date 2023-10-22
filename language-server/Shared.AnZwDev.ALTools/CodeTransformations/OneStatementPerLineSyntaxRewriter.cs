using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class OneStatementPerLineSyntaxRewriter : ALSyntaxRewriter
    {

        private SyntaxTrivia _newLineTrivia = SyntaxFactory.EndOfLine("\r\n");

        public OneStatementPerLineSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            if (node.Statements.Count > 0)
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //add missing new line between statements
                bool modified = false;
                statements.Add(node.Statements[0]);
                for (int i = 1; i < node.Statements.Count; i++)
                {
                    if ((!node.Statements[i - 1].GetTrailingTrivia().HasNewLine()) && (!node.Statements[i].GetTrailingTrivia().HasNewLine()))
                    {
                        var newStatement = node.Statements[i].WithLeadingTrivia(
                            node.Statements[i].GetLeadingTrivia().Add(_newLineTrivia));
                        statements.Add(newStatement);
                        modified = true;
                        NoOfChanges++;
                    }
                    else
                        statements.Add(node.Statements[i]);
                }

                //add missing new line between begin and first statement
                if ((!node.BeginKeywordToken.TrailingTrivia.HasNewLine()) && (!statements[0].GetTrailingTrivia().HasNewLine()))
                {
                    modified = true;
                    NoOfChanges++;
                    statements[0] = statements[0].WithLeadingTrivia(
                        node.Statements[0].GetLeadingTrivia().Insert(0, _newLineTrivia));
                }

                //add missing new line between end and last statement                
                /* Disabled as it adds empty line
                if ((!statements[statements.Count - 1].GetTrailingTrivia().HasNewLine()) && (!node.EndKeywordToken.TrailingTrivia.HasNewLine()))
                {
                    modified = true;
                    NoOfChanges++;
                    statements[statements.Count - 1] = statements[statements.Count - 1].WithTrailingTrivia(
                        node.Statements[statements.Count - 1].GetTrailingTrivia().Add(_newLineTrivia));
                }
                */

                if (modified)
                    node = node.WithStatements(SyntaxFactory.List(statements));
            }

            return base.VisitBlock(node);
        }

        public override SyntaxNode VisitRepeatStatement(RepeatStatementSyntax node)
        {
            if (node.Statements.Count > 0)
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();
                bool modified = false;

                //add missing new line between statements
                statements.Add(node.Statements[0]);
                for (int i = 1; i < node.Statements.Count; i++)
                {
                    if ((!node.Statements[i - 1].GetTrailingTrivia().HasNewLine()) && (!node.Statements[i].GetTrailingTrivia().HasNewLine()))
                    {
                        var newStatement = node.Statements[i].WithLeadingTrivia(
                            node.Statements[i].GetLeadingTrivia().Insert(0, _newLineTrivia));
                        statements.Add(newStatement);
                        modified = true;
                        NoOfChanges++;
                    }
                    else
                        statements.Add(node.Statements[i]);
                }

                //add missing new line between repeat and first statement
                if ((!node.RepeatKeywordToken.TrailingTrivia.HasNewLine()) && (!statements[0].GetTrailingTrivia().HasNewLine()))
                {
                    modified = true;
                    NoOfChanges++;
                    statements[0] = statements[0].WithLeadingTrivia(
                        node.Statements[0].GetLeadingTrivia().Insert(0, _newLineTrivia));
                }

                //add missing new line between until and last statement
                if ((!statements[statements.Count - 1].GetTrailingTrivia().HasNewLine()) && (!node.UntilKeywordToken.LeadingTrivia.HasNewLine()))
                {
                    modified = true;
                    NoOfChanges++;
                    statements[statements.Count - 1] = statements[statements.Count - 1].WithTrailingTrivia(
                        node.Statements[statements.Count - 1].GetTrailingTrivia().Add(_newLineTrivia));
                }

                if (modified)
                    node = node.WithStatements(SyntaxFactory.List(statements));
            } 
            else if ((!node.RepeatKeywordToken.TrailingTrivia.HasNewLine()) && (!node.UntilKeywordToken.TrailingTrivia.HasNewLine()))
            {
                //add missing new line between empty repeat and until
                NoOfChanges++;
                node = node.WithRepeatKeywordToken(
                    node.RepeatKeywordToken.WithTrailingTrivia(
                        node.RepeatKeywordToken.TrailingTrivia.Add(_newLineTrivia)));
            }

            return base.VisitRepeatStatement(node);
        }

        public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
        {
            //then            
            if ((node.Statement != null) && (!(node.Statement is BlockSyntax)) && ((!node.ThenKeywordToken.TrailingTrivia.HasNewLine()) && (!node.Statement.GetLeadingTrivia().HasNewLine())))
            {
                var newStatement = node.Statement.WithLeadingTrivia(
                    node.Statement.GetLeadingTrivia().Insert(0, _newLineTrivia));
                node = node.WithStatement(newStatement);
                NoOfChanges++;
            }

            //else
            if (node.ElseStatement != null)
            {
                //add missing new line between if statement and else keyword
                if ((!node.Statement.GetTrailingTrivia().HasNewLine()) && (!node.ElseKeywordToken.LeadingTrivia.HasNewLine()) && (node.Statement != null) && (!(node.Statement is BlockSyntax)))
                {
                    node = node.WithElseKeywordToken(
                        node.ElseKeywordToken.WithLeadingTrivia(
                            node.ElseKeywordToken.LeadingTrivia.Insert(0, _newLineTrivia)));
                    NoOfChanges++;
                }

                //add missing new line between else keyword and else statement
                if ((!(node.ElseStatement is BlockSyntax)) && (!node.ElseKeywordToken.TrailingTrivia.HasNewLine()) && (!node.ElseStatement.GetTrailingTrivia().HasNewLine()))
                {
                    node = node.WithElseKeywordToken(
                        node.ElseKeywordToken.WithTrailingTrivia(
                            node.ElseKeywordToken.TrailingTrivia.Add(_newLineTrivia)));
                    NoOfChanges++;
                }
            }

            return base.VisitIfStatement(node);
        }

        public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node)
        {
            if ((node.Statement != null) && (!(node.Statement is BlockSyntax)) && ((!node.DoKeywordToken.TrailingTrivia.HasNewLine()) && (!node.Statement.GetLeadingTrivia().HasNewLine())))
            {
                var newStatement = node.Statement.WithLeadingTrivia(
                    node.Statement.GetLeadingTrivia().Insert(0, _newLineTrivia));
                node = node.WithStatement(newStatement);
                NoOfChanges++;
            }

            return base.VisitWhileStatement(node);
        }


    }
}
