using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class TableExtensionSymbolFactory : TableExtensionSymbolFactory<TableExtensionSymbol>
    {
    }

    internal class TableExtensionSymbolFactory<T> : ObjectExtensionWithCodeSymbolFactory<T> where T : TableExtensionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.TableExtensionObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Fields, ALSyntaxNodeKind.FieldList, "fields", SymbolFactoryInstances.TableFieldSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Keys, ALSyntaxNodeKind.KeyList, "keys", SymbolFactoryInstances.TableKeySymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.FieldGroups, ALSyntaxNodeKind.FieldGroupExtensionList, "fieldgroups", SymbolFactoryInstances.TableFieldGroupExtensionSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }

}
