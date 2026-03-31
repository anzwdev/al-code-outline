using AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Handlers;
using AnZwDev.AL.Workspaces.Documents.SymbolsViewers;
using AnZwDev.LanguageServer;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer
{
    public class SymbolsViewerModule : LanguageServerModule
    {
        public SymbolsViewerModule(LanguageServerHost host) : base(host)
        {
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            Services.AddSingleton<SymbolsViewerDocumentStore>(new SymbolsViewerDocumentStore());
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new OpenAppFileDocumentRequestHandler(Services));
            RegisterRequestHandler(new OpenProjectDocumentRequestHandler(Services));
            RegisterRequestHandler(new GetObjectHeadersTreeRequestHandler(Services));
            RegisterRequestHandler(new GetObjectRequestHandler(Services));
            RegisterRequestHandler(new GetSymbolLocationRequestHandler(Services));
            RegisterRequestHandler(new CloseDocumentRequestHandler(Services));
        }

    }
}
