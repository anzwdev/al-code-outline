using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class TableSymbolFactory : TableSymbolFactory<TableSymbol>
    {
    }

    internal class TableSymbolFactory<T> : ObjectWithCodeSymbolFactory<T> where T : TableSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.TableObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Fields, ALSyntaxNodeKind.FieldList, "fields", SymbolFactoryInstances.TableFieldSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Keys, ALSyntaxNodeKind.KeyList, "keys", SymbolFactoryInstances.TableKeySymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.FieldGroups, ALSyntaxNodeKind.FieldGroupList, "fieldgroups", SymbolFactoryInstances.TableFieldGroupSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }

}
