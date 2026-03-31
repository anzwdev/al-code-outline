using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using AnZwDev.System.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class DotNetTypeDeclarationSymbolFactory : DotNetTypeDeclarationSymbolFactory<DotNetTypeDeclarationSymbol>
    {
    }

    internal class DotNetTypeDeclarationSymbolFactory<T> : SymbolFactory<T> where T : DotNetTypeDeclarationSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.DotNetTypeDeclaration;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (!String.IsNullOrWhiteSpace(symbol.AliasName))
                node.FullName = ALLiteralFormatter.GetName(symbol.AliasName) + ": " + ALLiteralFormatter.GetName(symbol.TypeName.NotNull());

            return node;
        }
    }

}
