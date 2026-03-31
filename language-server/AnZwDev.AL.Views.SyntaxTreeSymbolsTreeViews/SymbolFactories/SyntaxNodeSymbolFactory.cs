using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class SyntaxNodeSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(
            SyntaxNode node, NameSyntax? nameNode,
            ALSyntaxNodeKind symbolTreeNodeKind,
            TreeViewNodeNameSetter nameSetter,
            string? namespaceName = null, HashSet<string>? usings = null)
        {
            return CreateSymbol(0, node, nameNode, symbolTreeNodeKind, nameSetter, namespaceName, usings);
        }
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(
            int id,
            SyntaxNode node, NameSyntax? nameNode,
            ALSyntaxNodeKind symbolTreeNodeKind,
            TreeViewNodeNameSetter nameSetter,
            string? namespaceName = null, HashSet<string>? usings = null)
        {
            var syntaxTree = node.SyntaxTree;
            var firstToken = node.GetFirstToken();
            var lastToken = node.GetLastToken();
            var tokenSpan = new TextSpan(firstToken.Span.Start, lastToken.Span.End - firstToken.Span.Start);
            var tokensRange = syntaxTree.GetLineRange(tokenSpan);

            var symbol = new SyntaxTreeSymbolsTreeViewNode()
            {
                Id = id,
                Kind = symbolTreeNodeKind,
                Range = syntaxTree.GetLineRange(node.FullSpan),
                SelectionRange = (nameNode != null) ? syntaxTree.GetLineRange(nameNode.Span) : syntaxTree.GetLineRange(node.Span),
                ContainsDiagnostics = node.ContainsDiagnostics,
                TokensRange = tokensRange,
                NamespaceName = namespaceName,
                Usings = usings
            };

            nameSetter.SetName(symbol, nameNode);

            return symbol;
        }

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(
            SyntaxNode node, NameSyntax? nameNode,
            ALSyntaxNodeKind symbolTreeNodeKind,
            TreeViewNodeNameSetter nameSetter,
            SyntaxToken openBraceToken, SyntaxToken closeBraceToken, 
            string? namespaceName = null, HashSet<string>? usings = null)
        {
            var symbol = CreateSymbol(node, nameNode, symbolTreeNodeKind, nameSetter, namespaceName, usings);
            if ((openBraceToken.Kind != SyntaxKind.None) && (closeBraceToken.Kind != SyntaxKind.None))
                symbol.ContentRange = node.SyntaxTree.GetLineRange(openBraceToken.Span.Union(closeBraceToken.Span));
            return symbol;
        }

    }
}
