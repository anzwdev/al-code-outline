using AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Handlers;
using AnZwDev.LanguageServer;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider
{
    public class SymbolsSourceProviderModule : LanguageServerModule
    {
        public SymbolsSourceProviderModule(LanguageServerHost host) : base(host)
        {
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new GetAppFileSymbolSourceRequestHandler(Services));
        }

    }
}
