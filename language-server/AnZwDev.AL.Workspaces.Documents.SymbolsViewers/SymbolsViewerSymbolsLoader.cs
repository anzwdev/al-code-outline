using AnZwDev.AL.Symbols.Providers.AppPackages;
using AnZwDev.AL.Views.SymbolsTreeViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.Documents.SymbolsViewers
{
    public abstract class SymbolsViewerSymbolsLoader
    {

        protected static SymbolsTreeViewBuilder _treeViewBuilder = new SymbolsTreeViewBuilder();

        public SymbolsViewerSymbolsLoader()
        {
        }

        public abstract SymbolsTreeNode? Load();

    }
}
