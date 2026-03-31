using AnZwDev.System.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.Documents.SymbolsViewers
{
    public class SymbolsViewerDocumentStore : WorkspaceDocumentStore<SymbolsViewerDocument>
    {

        public SymbolsViewerDocument Open(Workspace workspace, string path)
        {
            return Open(workspace, new SymbolsViewerAppFileSymbolsLoader(workspace, path));
        }

        public SymbolsViewerDocument Open(Project project, bool includeDependencies)
        {
            return Open(project.Workspace, new SymbolsViewerProjectSymbolsLoader(project, includeDependencies));
        }

        public SymbolsViewerDocument Open(Workspace workspace, SymbolsViewerSymbolsLoader symbolsLoader)
        {
            var document = new SymbolsViewerDocument(workspace, symbolsLoader);
            document.Load();
            Add(document);
            return document;
        }

    }
}
