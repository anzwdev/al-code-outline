using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeViewerTreeViews
{

    /// <summary>
    /// Basic syntax tree view builder
    ///   uses only ChildNodes() to get child nodes of a node
    /// </summary>
    public class ChildNodesBasedSyntaxTreeViewerTreeViewBuilder : SyntaxTreeViewerTreeViewBuilder
    {

        protected override SyntaxTreeViewerTreeNode Create(SyntaxTree syntaxTree, SyntaxNode node)
        {
            var symbolTreeItem = new SyntaxTreeViewerTreeNode();
            symbolTreeItem.Name = node.Kind.ToString();
            symbolTreeItem.FullName = symbolTreeItem.Name + " " + node.FullSpan.ToString();
            symbolTreeItem.SyntaxNode = node;
            symbolTreeItem.Type = node.GetType().Name;

            if (node.ContainsDiagnostics)
                symbolTreeItem.ContainsDiagnostics = true;

            symbolTreeItem.Range = syntaxTree.GetLineRange(node.FullSpan);
            symbolTreeItem.SelectionRange = syntaxTree.GetLineRange(node.Span);

            IEnumerable<SyntaxNode> list = node.ChildNodes();
            if (list != null)
                foreach (SyntaxNode childNode in list)
                    symbolTreeItem.AddChildSymbol(Create(syntaxTree, childNode));

            symbolTreeItem.CalculateUid();

            return symbolTreeItem;
        }

    }
}
