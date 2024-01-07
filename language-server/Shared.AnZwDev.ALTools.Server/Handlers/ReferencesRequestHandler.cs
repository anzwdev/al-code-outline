using AnZwDev.ALTools.Navigation;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class ReferencesRequestHandler : RequestHandler
    {

        private ReferencesProvidersCollection _referencesProviders;

        public ReferencesRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
            _referencesProviders = new ReferencesProvidersCollection(languageServerHost.ALDevToolsServer);
        }

        [JsonRpcMethod("al/references", UseSingleObjectParameterDeserialization = true)]
        public ReferencesResponse GetReferences(ReferencesRequest parameters)
        {
            ReferencesResponse response = new ReferencesResponse();
            try
            {
                response.references = _referencesProviders.FindReferences(parameters.isActiveDocument, parameters.source, parameters.position);
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            return response;
        }

    }
}
