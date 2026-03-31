using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews
{
    public partial class SyntaxTreeSymbolsTreeViewBuilder
    {

        public SyntaxTreeSymbolsTreeViewNode? CreateView(string? content)
        {
            if (String.IsNullOrWhiteSpace(content))
                return null;

            var syntaxTree = SyntaxTree.ParseObjectText(content);
            if (syntaxTree == null)
                return null;

            return CreateView(syntaxTree);
        }

        public SyntaxTreeSymbolsTreeViewNode? CreateView(SyntaxTree syntaxTree)
        {
            var rootNode = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (rootNode == null)
                return null;

            var firstRegionDirective = RegionDirectivesCollector.Collect(syntaxTree, rootNode);
            var rootSymbol = new SyntaxTreeSymbolsTreeViewNode();

            CreateTree(rootNode, rootSymbol, firstRegionDirective);

            //return compilation unit as root symbol if it is the only child of the root symbol
            //otherwise return root symbol with compilation unit and regions as children
            if ((rootSymbol.ChildSymbols != null) && (rootSymbol.ChildSymbols.Count == 1))
                rootSymbol = rootSymbol.ChildSymbols[0];

            return rootSymbol;
        }

        private (SyntaxTreeSymbolsTreeViewNode?, RegionDirective?) CreateTree(SyntaxNode syntaxNode, SyntaxTreeSymbolsTreeViewNode parentNode, RegionDirective? currentRegion)
        {
            var treeNode = CreateSymbol(syntaxNode, parentNode);
            if (treeNode == null)
                return (null, currentRegion);

            (parentNode, currentRegion) = AddChildSymbol(parentNode, currentRegion, treeNode);
            (var childParentNode, var childCurrentRegion) = ProcessChildSyntaxNodesCollection(syntaxNode, treeNode, currentRegion);
            currentRegion = AddRemainingSymbolRegions(treeNode, childParentNode, childCurrentRegion);

            return (parentNode, currentRegion);
        }

        private (SyntaxTreeSymbolsTreeViewNode, RegionDirective?) ProcessChildSyntaxNodesCollection(SyntaxNode syntaxNode, SyntaxTreeSymbolsTreeViewNode parentNode, RegionDirective? currentRegion)
        {
            var childNodesCollection = syntaxNode.ChildNodes();
            foreach (var childNode in childNodesCollection)
            {
                (var newParentNode, var newCurrentRegion) = CreateTree(childNode, parentNode, currentRegion);
                if (newParentNode != null)
                {
                    parentNode = newParentNode;
                    currentRegion = newCurrentRegion;
                }
            }
            return (parentNode, currentRegion);
        }

        private (SyntaxTreeSymbolsTreeViewNode, RegionDirective?) AddChildSymbol(SyntaxTreeSymbolsTreeViewNode parentSymbolOrRegion, RegionDirective? currentRegion, SyntaxTreeSymbolsTreeViewNode childSymbol)
        {
            //go up or add regions closed before symbol
            (parentSymbolOrRegion, currentRegion) = CloseOrAddRegionsBeforePosition(parentSymbolOrRegion, currentRegion, childSymbol);

            //add regions opened before the symbol
            parentSymbolOrRegion.AddChildSymbol(childSymbol);

            return (parentSymbolOrRegion, currentRegion);
        }

        private (SyntaxTreeSymbolsTreeViewNode, RegionDirective?) CloseOrAddRegionsBeforePosition(SyntaxTreeSymbolsTreeViewNode parentSymbolOrRegion, RegionDirective? currentRegionDirective, SyntaxTreeSymbolsTreeViewNode childSymbol)
        {
            while ((currentRegionDirective?.Next != null) && (childSymbol.SelectionRange != null)  && (currentRegionDirective.Next.SelectionRange.Start.CompareTo(childSymbol.SelectionRange.Start) <= 0))
            {
                currentRegionDirective = currentRegionDirective!.Next!;
                parentSymbolOrRegion = ProcessRegionDirective(parentSymbolOrRegion, currentRegionDirective);
            }

            return (parentSymbolOrRegion, currentRegionDirective);
        }

        private RegionDirective? AddRemainingSymbolRegions(SyntaxTreeSymbolsTreeViewNode parentSymbol, SyntaxTreeSymbolsTreeViewNode parentSymbolOrRegion, RegionDirective? currentRegionDirective)
        {
            while ((currentRegionDirective?.Next != null) && (parentSymbol.Range != null) && (currentRegionDirective.Next.SelectionRange.Start.CompareTo(parentSymbol.Range.End) <= 0))
            {
                currentRegionDirective = currentRegionDirective!.Next!;
                parentSymbolOrRegion = ProcessRegionDirective(parentSymbolOrRegion, currentRegionDirective);
            }
            return currentRegionDirective;
        }

        private SyntaxTreeSymbolsTreeViewNode ProcessRegionDirective(SyntaxTreeSymbolsTreeViewNode parentSymbolOrRegion, RegionDirective currentRegionDirective)
        {
            if (currentRegionDirective.IsStartRegion)
            {
                var newRegion = new SyntaxTreeSymbolsTreeViewNode()
                {
                    Kind = ALSyntaxNodeKind.Region,
                    Name = "#region",
                    FullName = currentRegionDirective.Name,
                    Range = currentRegionDirective.Range,
                    SelectionRange = currentRegionDirective.SelectionRange
                };
                parentSymbolOrRegion.AddChildSymbol(newRegion);
                parentSymbolOrRegion = newRegion;
            }
            else
            {
                if (parentSymbolOrRegion.Kind == ALSyntaxNodeKind.Region)
                {
                    if (parentSymbolOrRegion.Range != null)
                        parentSymbolOrRegion.Range.End = currentRegionDirective.Range.End;
                    
                    if (parentSymbolOrRegion.ParentSymbol != null)
                        parentSymbolOrRegion = parentSymbolOrRegion.ParentSymbol;
                }
            }

            return parentSymbolOrRegion;
        }

    }
}
