using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class AddUsingRegionSyntaxRewriter : ALSyntaxRewriter
    {

        private string _title = "Usings";
        public string Title 
        { 
            get { return _title; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    _title = "Usings";
                else
                    _title = value;
            }
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            if ((node.Usings != null) && (node.Usings.Count != 0))
            {
                NoOfChanges++;

                var usings = node.Usings;
                var leadingTrivia = usings[0].GetLeadingTrivia();
                var trailingTrivia = usings[usings.Count - 1].GetTrailingTrivia();

                if ((!leadingTrivia.OpensRegion()) && (!trailingTrivia.ClosesRegion()))
                {
                    if (!leadingTrivia.EndsWithNewLine())
                        leadingTrivia = leadingTrivia.Add(SyntaxFactory.EndOfLine("\r\n"));
                    leadingTrivia = leadingTrivia.AddRange(SyntaxFactory.ParseLeadingTrivia("#region " + Title + "\r\n\r\n"));

                    if (!trailingTrivia.StartsWithNewLine())
                        trailingTrivia = trailingTrivia.Insert(0, SyntaxFactory.EndOfLine("\r\n"));
                    trailingTrivia = trailingTrivia.InsertRange(0, SyntaxFactory.ParseLeadingTrivia("\r\n\r\n#endregion"));

                    usings = usings.Replace(usings[0], usings[0].WithLeadingTrivia(leadingTrivia));
                    usings = usings.Replace(usings[usings.Count - 1], usings[usings.Count - 1].WithTrailingTrivia(trailingTrivia));

                    node = node.WithUsings(usings);
                }
            }

            return base.VisitCompilationUnit(node);
        }

    }

#endif

}
