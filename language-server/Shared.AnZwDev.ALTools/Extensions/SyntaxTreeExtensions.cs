using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxTreeExtensions
    {

        public static SyntaxTree SafeParseObjectText(string source)
        {
            SyntaxTree syntaxTree;

            try
            {
                syntaxTree = ParseObjectText(source);
            }
            catch (MissingMethodException)
            {
                syntaxTree = ParseObjectTextNav2018(source);
            }

            return syntaxTree;
        }

        private static SyntaxTree ParseObjectText(string source)
        {
            return SyntaxTree.ParseObjectText(source);
        }

        private static SyntaxTree ParseObjectTextNav2018(string source)
        {
            return typeof(SyntaxTree).CallStaticMethod<SyntaxTree>("ParseObjectText", source,
                Type.Missing, Type.Missing, Type.Missing);
        }

        public static SyntaxNode FindNodeByPositionInSpan(this SyntaxTree syntaxTree, int position)
        {
            return FindNodeByPositionInSpan(syntaxTree.GetRoot(), position);
        }

        private static SyntaxNode FindNodeByPositionInSpan(SyntaxNode node, int position)
        {            
            if ((node.Span.Start <= position) && (node.Span.End >= position))
            {
                var childNodes = node.ChildNodes();
                if (childNodes != null)
                    foreach (var childNode in childNodes)
                    {
                        var nodeAtPosition = FindNodeByPositionInSpan(childNode, position);
                        if (nodeAtPosition != null)
                            return nodeAtPosition;
                    }
                return node;
            }
            return null;
        }

        public static SyntaxNode FindNodeByPositionInFullSpan(this SyntaxTree syntaxTree, int position)
        {
            return FindNodeByPositionInFullSpan(syntaxTree.GetRoot(), position);
        }

        private static SyntaxNode FindNodeByPositionInFullSpan(SyntaxNode node, int position)
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

        public static Range GetLineRange(this SyntaxTree syntaxTree, TextSpan textSpan)
        {
            var lineSpan = syntaxTree.GetLineSpan(textSpan);
            return new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
        }

    }
}
