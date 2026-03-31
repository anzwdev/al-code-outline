using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews
{
    public class SymbolsTreeViewBuilder
    {

        public SymbolsTreeNode? CreateView(ApplicationSymbol symbol)
        {
            var node = SymbolFactoryInstances.ApplicationSymbolFactory.Create(symbol);
            node.UpdateUid();
            return node;
        }

        public SymbolsTreeNode? CreateView(ProjectDefinitionSymbol symbol)
        {
            var node = SymbolFactoryInstances.ProjectDefinitionSymbolFactory.Create(symbol);
            node.UpdateUid();
            return node;
        }

    }
}
