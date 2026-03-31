using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class TableFieldSymbolFactory : TableFieldSymbolFactory<TableFieldSymbol>
    {
    }

    internal class TableFieldSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : TableFieldSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.Field;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (symbol.TypeDefinition != null)
                node.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + DisplayStringFormatter.FormatTypeDefinitionSymbol(symbol.TypeDefinition);
            SetSymbolSubtype(node, symbol);

            return node;
        }

        private void SetSymbolSubtype(SymbolsTreeNode node, TableFieldSymbol symbol)
        {
            //detect subtype
            if (symbol.Properties != null)
            {
                if (!symbol.Properties.Enabled)
                {
                    node.Subtype = "Disabled";
                    node.FullName = node.FullName + " (Disabled)";
                    return;
                }

                var obsoleteState = symbol.Properties.ObsoleteState;


                if (obsoleteState != ObsoleteState.No)
                {
                    var obsoleteStateString = obsoleteState.ToString();
                    var obsoleteStateDetails = symbol.Properties.ObsoleteReason;
                    
                    if (!string.IsNullOrWhiteSpace(obsoleteStateDetails))
                        obsoleteStateDetails = obsoleteStateDetails + ": " + obsoleteStateDetails.Trim();
                    obsoleteStateDetails = obsoleteStateString + obsoleteStateDetails;

                    node.Subtype = obsoleteStateString;
                    node.FullName = node.FullName + " (" + obsoleteStateDetails + ")";
                }
            }
        }

    }
}
