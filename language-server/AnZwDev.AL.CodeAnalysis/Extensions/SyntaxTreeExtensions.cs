using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class SyntaxTreeExtensions
    {

        public static TextRange GetLineRange(this SyntaxTree syntaxTree, TextSpan textSpan)
        {
            var lineSpan = syntaxTree.GetLineSpan(textSpan);
            return new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
        }

        public static SyntaxNode? FindNodeByPositionInFullSpan(this SyntaxTree syntaxTree, int position)
        {
            return FindNodeByPositionInFullSpan(syntaxTree.GetRoot(), position);
        }

        private static SyntaxNode? FindNodeByPositionInFullSpan(SyntaxNode node, int position)
        {
            if ((node.FullSpan.Start <= position) && (node.FullSpan.End > position))
            {
                var childNodes = node.ChildNodes();
                if (childNodes != null)
                    foreach (var childNode in childNodes)
                    {
                        var nodeAtPosition = FindNodeByPositionInFullSpan(childNode, position);
                        if (nodeAtPosition != null)
                            return nodeAtPosition;
                    }
                return node;
            }
            return null;
        }


    }
}
