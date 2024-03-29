﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
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

        public static SyntaxTree SafeParseObjectText(string source, ALProject project)
        {
            SyntaxTree syntaxTree;

            try
            {
                syntaxTree = ParseObjectText(source, project);
            }
            catch (MissingMethodException)
            {
                syntaxTree = ParseObjectTextNav2018(source);
            }

            return syntaxTree;
        }

        private static SyntaxTree ParseObjectText(string source, ALProject project)
        {
#if BC
            return SyntaxTree.ParseObjectText(source, null, null, project?.GetSyntaxTreeParseOptions());
#else
            return SyntaxTree.ParseObjectText(source);
#endif
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

        public static TextRange GetLineRange(this SyntaxTree syntaxTree, TextSpan textSpan)
        {
            var lineSpan = syntaxTree.GetLineSpan(textSpan);
            return new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);
        }

        public static SyntaxNode FindNodeByPositionInFullSpan(this SyntaxTree syntaxTree, Position position)
        {
            return FindNodeByPositionInFullSpan(syntaxTree, syntaxTree.GetRoot(), position);
        }

        private static SyntaxNode FindNodeByPositionInFullSpan(SyntaxTree syntaxTree, SyntaxNode node, Position position)
        {
            var lineSpan = syntaxTree.GetLineSpan(node.FullSpan);

            if ((position.IsGreaterOrEqual(lineSpan.StartLinePosition)) && (position.IsLower(lineSpan.EndLinePosition)))
            {
                var childNodes = node.ChildNodes();
                if (childNodes != null)
                    foreach (var childNode in childNodes)
                    {
                        var nodeAtPosition = FindNodeByPositionInFullSpan(syntaxTree, childNode, position);
                        if (nodeAtPosition != null)
                            return nodeAtPosition;
                    }
                return node;
            }
            return null;
        }

    }
}
