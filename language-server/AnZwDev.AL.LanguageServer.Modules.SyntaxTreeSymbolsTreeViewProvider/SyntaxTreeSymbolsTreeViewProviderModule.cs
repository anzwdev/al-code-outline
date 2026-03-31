using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeSymbolsTreeViewProvider.Handlers;
using AnZwDev.LanguageServer;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeSymbolsTreeViewProvider
{
    public class SyntaxTreeSymbolsTreeViewProviderModule : LanguageServerModule
    {

        public SyntaxTreeSymbolsTreeViewProviderModule(LanguageServerHost languageServerHost) :
            base(languageServerHost)
        {
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new GetSyntaxTreeSymbolsTreeViewRequestHandler(Services));
        }

    }
}
