using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class ObjectSymbolFactory<T> : SymbolFactory<T> where T : ObjectSymbol
    {

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            node.Id = symbol.Identifier.Id;
            node.Name = symbol.Identifier.FullyQualifiedName.Name;
            node.FullName = kind.ToDescriptionString() + " " + ALLiteralFormatter.GetName(symbol.Identifier.FullyQualifiedName.Name);
            node.Access = symbol.AccessLevel.ToALSyntaxNodeAccessModifier();
            node.NamespaceName = symbol.Identifier.FullyQualifiedName.Namespace;
            node.Usings = symbol.Usings;

            return node;
        }

    }
}
