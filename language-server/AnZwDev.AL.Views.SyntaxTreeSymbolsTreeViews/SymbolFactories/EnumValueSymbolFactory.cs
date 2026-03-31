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
    internal static class EnumValueSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(EnumValueSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                
                node, node.Name, 
                ALSyntaxNodeKind.EnumValue, 
                TreeViewNodeNameSetters.IdentifierName,
                node.OpenBraceToken, node.CloseBraceToken);

            string idText = node.Id.ToString();
            if (!String.IsNullOrWhiteSpace(idText))
            {
                int id;
                if (Int32.TryParse(idText, out id))
                    symbol.Id = id;
            }

            return symbol;
        }
    }
}
