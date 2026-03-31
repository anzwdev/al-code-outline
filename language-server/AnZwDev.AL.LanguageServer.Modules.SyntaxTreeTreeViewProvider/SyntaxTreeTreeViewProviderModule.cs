using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeTreeViewProvider.Handlers;
using AnZwDev.LanguageServer;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeTreeViewProvider
{
    public class SyntaxTreeTreeViewProviderModule : LanguageServerModule
    {

        public SyntaxTreeTreeViewProviderModule(LanguageServerHost languageServerHost) :
            base(languageServerHost)
        {
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new GetSyntaxTreeTreeViewRequestHandler(Services));
        }

    }
}
