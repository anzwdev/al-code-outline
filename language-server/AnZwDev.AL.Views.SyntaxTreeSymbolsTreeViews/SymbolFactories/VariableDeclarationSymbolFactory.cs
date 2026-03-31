using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.Formatters;
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
    internal static class VariableDeclarationSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(VariableDeclarationSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var parentVarSection = parentNode?.FindParent(ALSyntaxNodeKind.GlobalVarSection);
            var accessModifier = (parentVarSection != null) ? parentVarSection.Access : ALSyntaxNodeAccessModifier.Public;

            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.VariableDeclaration,
                TreeViewNodeNameSetters.IdentifierName);
            if (node.Type != null)
                symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + DataTypeFormatter.GetCode(node.Type);
            symbol.Access = accessModifier;

            return symbol;
        }
    }
}
