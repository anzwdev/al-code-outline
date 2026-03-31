using AnZwDev.AL.Symbols.Providers.AppPackages;
using AnZwDev.AL.Views.SymbolsTreeViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.Documents.SymbolsViewers
{
    public class SymbolsViewerAppFileSymbolsLoader : SymbolsViewerSymbolsLoader
    {

        public Workspace Workspace { get; }
        public string FilePath { get; }

        public SymbolsViewerAppFileSymbolsLoader(Workspace workspace, string filePath)
        {
            FilePath = filePath;
            Workspace = workspace;
        }

        public override SymbolsTreeNode? Load()
        {
            var symbolsProvider = new AppPackageSymbolsProvider(FilePath, Workspace.SymbolsCache);
            symbolsProvider.Load(false);

            var symbols = symbolsProvider?.GetSymbols();

            if (symbols == null)
                return null;
            return _treeViewBuilder.CreateView(symbols);
        }

    }
}
