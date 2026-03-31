using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
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
    /// Raw syntax tree view builder
    ///   builds a full syntax tree view including all nodes and tokens
    ///   gets child nodes and tokens using reflection to get all properties of syntax nodes
    /// </summary>
    public class PropertyBasedSyntaxTreeViewerTreeViewBuilder : SyntaxTreeViewerTreeViewBuilder
    {

        protected override SyntaxTreeViewerTreeNode Create(SyntaxTree syntaxTree, SyntaxNode node)
        {
            var symbolNode = ProcessSyntaxTreeNode(new HashSet<SyntaxNode>(), syntaxTree, node, null);
            if (symbolNode == null)
            {
                symbolNode = new SyntaxTreeViewerTreeNode()
                {
                    Name = "Empty",
                    FullName = "Empty",
                    Type = "Empty",
                    Range = new TextRange(),
                    SelectionRange = new TextRange()
                };
            }

            symbolNode.CalculateUid();

            return symbolNode;
        }

        protected SyntaxTreeViewerTreeNode? ProcessSyntaxTreeNode(HashSet<SyntaxNode> processedNodes, SyntaxTree syntaxTree, SyntaxNode node, string? name)
        {
            processedNodes.Add(node);

            var nodeInfoText = node.Kind.ToString() + " " + node.FullSpan.ToString();
            var viewTreeNode = new SyntaxTreeViewerTreeNode();
            if (String.IsNullOrWhiteSpace(name))
            {
                viewTreeNode.Name = node.Kind.ToString();
                viewTreeNode.FullName = nodeInfoText;
            }
            else
            {
                viewTreeNode.Name = name;
                viewTreeNode.FullName = name + " - " + nodeInfoText;
            }
            viewTreeNode.SyntaxNode = node;
            viewTreeNode.Type = node.GetType().Name;

            if (node.ContainsDiagnostics)
                viewTreeNode.ContainsDiagnostics = true;
           
            viewTreeNode.Range = syntaxTree.GetLineRange(node.FullSpan);
            viewTreeNode.SelectionRange = syntaxTree.GetLineRange(node.Span);

            var propertiesList = node.GetType().GetProperties();
            foreach (var property in propertiesList)
            {
                var value = property.GetValue(node);
                if (value != null)
                {
                    switch (value)
                    {
                        case SyntaxNode childNode:
                            if (!processedNodes.Contains(childNode))
                            {
                                var childSymbolNode = ProcessSyntaxTreeNode(processedNodes, syntaxTree, childNode, property.Name);
                                if (childSymbolNode != null)
                                    viewTreeNode.AddChildSymbol(childSymbolNode);
                            }
                            break;
                        case SyntaxToken childToken:
                            viewTreeNode.AddChildSymbol(ProcessSyntaxTreeToken(syntaxTree, childToken, property.Name));
                            break;
                        case IEnumerable<SyntaxNode> childNodesCollection:
                            var childCollectionSymbolNode = ProcessSyntaxTreeNodesCollection(processedNodes, syntaxTree, childNodesCollection, property.Name);
                            if (childCollectionSymbolNode != null)
                                viewTreeNode.AddChildSymbol(childCollectionSymbolNode);
                            break;
                    }
                }
            }

            if ((viewTreeNode.Range == null) || (viewTreeNode.Range.IsEmpty))
                return null;

            return viewTreeNode;
        }

        protected SyntaxTreeViewerTreeNode ProcessSyntaxTreeToken(SyntaxTree syntaxTree, SyntaxToken node, string name)
        {
            var nodeInfoText = node.Kind.ToString() + " " + node.FullSpan.ToString();

            var symbolInfo = new SyntaxTreeViewerTreeNode();

            if (String.IsNullOrWhiteSpace(name))
            {
                symbolInfo.Name = node.Kind.ToString();
                symbolInfo.FullName = nodeInfoText;
            }
            else
            {
                symbolInfo.Name = name;
                symbolInfo.FullName = name + " - " + nodeInfoText;
            }
            symbolInfo.SyntaxNode = null; //node;
            symbolInfo.Type = node.GetType().Name;

            if (node.ContainsDiagnostics)
                symbolInfo.ContainsDiagnostics = true;

            symbolInfo.Range = syntaxTree.GetLineRange(node.FullSpan);
            symbolInfo.SelectionRange = syntaxTree.GetLineRange(node.Span);

            return symbolInfo;
        }

        protected SyntaxTreeViewerTreeNode? ProcessSyntaxTreeNodesCollection(HashSet<SyntaxNode> processedNodes, SyntaxTree syntaxTree, IEnumerable<SyntaxNode> nodesCollection, string name)
        {
            var symbolInfo = new SyntaxTreeViewerTreeNode();
            symbolInfo.Name = name;
            symbolInfo.FullName = name;
            symbolInfo.SyntaxNode = null; // node;
            symbolInfo.Type = nodesCollection.GetType().Name;

            symbolInfo.Range = new TextRange();
            symbolInfo.SelectionRange = new TextRange();

            foreach (SyntaxNode node in nodesCollection)
            {
                if (!processedNodes.Contains(node))
                {
                    var childSymbolInfo = ProcessSyntaxTreeNode(processedNodes, syntaxTree, node, null);
                    if (childSymbolInfo != null)
                    {
                        symbolInfo.AddChildSymbol(childSymbolInfo);

                        if (childSymbolInfo.Range != null)
                            symbolInfo.Range.Expand(childSymbolInfo.Range);

                        if (childSymbolInfo.SelectionRange != null)
                            symbolInfo.SelectionRange.Expand(childSymbolInfo.SelectionRange);
                    }
                }
            }

            if ((symbolInfo.ChildSymbols != null) && (symbolInfo.ChildSymbols.Count > 0))
                return symbolInfo;
            return null;
        }

    }
}
