using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Handlers;
using AnZwDev.LanguageServer;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer
{
    public class SyntaxTreeViewerModule : LanguageServerModule
    {

        public SyntaxTreeViewerModule(LanguageServerHost languageServerHost) :
            base(languageServerHost)
        {
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new GetSyntaxTreeViewerTreeViewRequestHandler(Services));
            RegisterRequestHandler(new GetSyntaxTreeViewerTreeNodePropertiesRequestHandler(Services));
        }

    }
}
