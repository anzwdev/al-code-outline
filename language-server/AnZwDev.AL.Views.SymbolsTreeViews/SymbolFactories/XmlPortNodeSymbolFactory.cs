using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AnZwDev.AL.Symbols.Formatters;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class XmlPortNodeSymbolFactory : XmlPortNodeSymbolFactory<XmlPortNodeSymbol>
    {
    }

    internal class XmlPortNodeSymbolFactory<T> : NamedSymbolWithPropertiesFactory<T> where T : XmlPortNodeSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return symbol.Kind.ToALSyntaxNodeKind();
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            var sourceExpression = GetSourceExpression(symbol);
            if (!String.IsNullOrEmpty(sourceExpression))
            {
                node.Name = node.Kind.ToDescriptionString() + " " + sourceExpression;
                node.Source = sourceExpression;
            }

            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Schema, SymbolFactoryInstances.XmlPortNodeSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

        private string? GetSourceExpression(T symbol)
        {
            switch (symbol)
            {
                case XmlPortTableElementSymbol tableElement:
                    if (tableElement.SourceTable != null)
                        return DisplayStringFormatter.FormatFullyQualifiedName(tableElement.SourceTable.Value.FullyQualifiedName);
                    break;
                case XmlPortFieldNodeSymbol fieldNode:
                    return fieldNode.Expression;
            }
            return null;
        }

    }
}
