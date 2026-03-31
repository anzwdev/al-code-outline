using AnZwDev.AL.Views.SymbolsTreeViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.Documents.SymbolsViewers
{
    public class SymbolsViewerProjectSymbolsLoader : SymbolsViewerSymbolsLoader
    {

        public Project Project { get; }
        public bool IncludeDependencies { get; }

        public SymbolsViewerProjectSymbolsLoader(Project project, bool includeDependencies)
        {
            IncludeDependencies = includeDependencies;
            Project = project;
        }

        public override SymbolsTreeNode? Load()
        {
            if (IncludeDependencies)
            {
                var projectSymbol = Project.SymbolsProvider.CreateProjectDefinitionSymbol();
                if (projectSymbol != null)
                    return _treeViewBuilder.CreateView(projectSymbol);
            }
            else
            {
                var applicationSymbol = Project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols();
                if (applicationSymbol != null)
                    return _treeViewBuilder.CreateView(applicationSymbol);
            }

            return null;
        }

    }
}
