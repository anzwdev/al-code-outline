using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Workspace;
using System.IO;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Extensions;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALRawSyntaxTreeSymbolsReader : ALBaseSyntaxTreeSymbolsReader
    {

        public ALRawSyntaxTreeSymbolsReader()
        {
        }

        public override ALSyntaxTreeSymbol ProcessSourceCode(string source, ALProject project)
        {
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(source, project);

            if (syntaxTree != null)
            {
                SyntaxNode node = syntaxTree.GetRoot();
                if (node != null)
                    return ProcessSyntaxTreeNode(new HashSet<SyntaxNode>(), syntaxTree, node, null);
            }
            return null;
        }

        protected ALSyntaxTreeSymbol ProcessSyntaxTreeNode(HashSet<SyntaxNode> processedNodes, SyntaxTree syntaxTree, SyntaxNode node, string name)
        {
            processedNodes.Add(node);

            var nodeInfoText = node.Kind.ToString() + " " + node.FullSpan.ToString();
            ALSyntaxTreeSymbol symbolInfo = new ALSyntaxTreeSymbol();
            symbolInfo.kind = ALSymbolKind.SyntaxTreeNode;
            if (String.IsNullOrWhiteSpace(name))
            {
                symbolInfo.name = node.Kind.ToString();
                symbolInfo.fullName = nodeInfoText;
            } 
            else
            {
                symbolInfo.name = name;
                symbolInfo.fullName = name + " - " + nodeInfoText;
            }
            symbolInfo.syntaxTreeNode = node;
            symbolInfo.type = node.GetType().Name;

            if (node.ContainsDiagnostics)
                symbolInfo.containsDiagnostics = true;

            var lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            symbolInfo.range = new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            symbolInfo.selectionRange = new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

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
                                symbolInfo.AddChildSymbol(ProcessSyntaxTreeNode(processedNodes, syntaxTree, childNode, property.Name));
                            break;
                        case SyntaxToken childToken:
                            symbolInfo.AddChildSymbol(ProcessSyntaxTreeToken(syntaxTree, childToken, property.Name));
                            break;
                        case IEnumerable<SyntaxNode> childNodesCollection:
                            symbolInfo.AddChildSymbol(ProcessSyntaxTreeNodesCollection(processedNodes, syntaxTree, childNodesCollection, property.Name));
                            break;
                    }
                }
            }

            if ((symbolInfo.range == null) || (symbolInfo.range.isEmpty))
                return null;

            return symbolInfo;
        }

        protected ALSyntaxTreeSymbol ProcessSyntaxTreeToken(SyntaxTree syntaxTree, SyntaxToken node, string name)
        {
            var nodeInfoText = node.Kind.ToString() + " " + node.FullSpan.ToString();

            ALSyntaxTreeSymbol symbolInfo = new ALSyntaxTreeSymbol();
            symbolInfo.kind = ALSymbolKind.SyntaxTreeToken;
            if (String.IsNullOrWhiteSpace(name))
            {
                symbolInfo.name = node.Kind.ToString();
                symbolInfo.fullName = nodeInfoText;
            }
            else
            {
                symbolInfo.name = name;
                symbolInfo.fullName = name + " - " + nodeInfoText;
            }
            symbolInfo.syntaxTreeNode = null; //node;
            symbolInfo.type = node.GetType().Name;

            if (node.ContainsDiagnostics)
                symbolInfo.containsDiagnostics = true;

            var lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            symbolInfo.range = new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            symbolInfo.selectionRange = new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            return symbolInfo;
        }

        protected ALSyntaxTreeSymbol ProcessSyntaxTreeNodesCollection(HashSet<SyntaxNode> processedNodes, SyntaxTree syntaxTree, IEnumerable<SyntaxNode> nodesCollection, string name)
        {
            ALSyntaxTreeSymbol symbolInfo = new ALSyntaxTreeSymbol();
            symbolInfo.kind = ALSymbolKind.SyntaxTreeToken;
            symbolInfo.name = name;
            symbolInfo.fullName = name;
            symbolInfo.syntaxTreeNode = null; // node;
            symbolInfo.type = nodesCollection.GetType().Name;

            symbolInfo.range = new TextRange();
            symbolInfo.selectionRange = new TextRange();

            foreach (SyntaxNode node in nodesCollection)
            {
                if (!processedNodes.Contains(node))
                {
                    var childSymbolInfo = ProcessSyntaxTreeNode(processedNodes, syntaxTree, node, null);
                    if (childSymbolInfo != null)
                    {
                        symbolInfo.AddChildSymbol(childSymbolInfo);
                        symbolInfo.range.Add(childSymbolInfo.range);
                        symbolInfo.selectionRange.Add(childSymbolInfo.selectionRange);
                    }
                }
            }

            if ((symbolInfo.childSymbols != null) && (symbolInfo.childSymbols.Count > 0))
                return symbolInfo;
            return null;
        }

    }
}
