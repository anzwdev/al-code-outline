using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers;
using AnZwDev.LanguageServer;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider
{
    public class ProjectInformationProviderModule : LanguageServerModule
    {
        public ProjectInformationProviderModule(LanguageServerHost host) : base(host)
        {
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new GetProjectProfileRequestHandler(Services));
            RegisterRequestHandler(new GetNamespaceAndUsingsRequestHandler(Services));
            RegisterRequestHandler(new GetNextObjectIdRequestHandler(Services));

            RegisterRequestHandler(new GetObjectsListRequestHandler(Services));
            RegisterRequestHandler(new GetObjectMethodsRequestHandler(Services));
            RegisterRequestHandler(new GetTableFieldsRequestHandler(Services));
        }

    }
}
