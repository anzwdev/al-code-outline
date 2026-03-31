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
    internal static class VariableDeclarationNameSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(VariableDeclarationNameSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var parentVarSection = parentNode?.FindParent(ALSyntaxNodeKind.GlobalVarSection);
            var accessModifier = (parentVarSection != null) ? parentVarSection.Access : ALSyntaxNodeAccessModifier.Public;

            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, node.Name, 
                ALSyntaxNodeKind.VariableDeclarationName,
                TreeViewNodeNameSetters.IdentifierName);
            symbol.Access = accessModifier;

            var variableListDeclarationSyntax = node.GetParentOfType<VariableListDeclarationSyntax>();
            if (variableListDeclarationSyntax?.Type != null)
            {
                string typeName = DataTypeFormatter.GetCode(variableListDeclarationSyntax.Type);
                string elementDataType = typeName;
                if (variableListDeclarationSyntax.Type.DataType != null)
                    elementDataType = DataTypeFormatter.GetCode(variableListDeclarationSyntax.Type.DataType);

                symbol.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + typeName;
                symbol.Subtype = typeName;
                symbol.ElementSubtype = elementDataType;
            }

            return symbol;
        }
    }
}
