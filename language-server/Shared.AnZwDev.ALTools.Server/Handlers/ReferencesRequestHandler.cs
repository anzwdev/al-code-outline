using AnZwDev.ALTools.Navigation;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class ReferencesRequestHandler : BaseALRequestHandler<ReferencesRequest, ReferencesResponse>
    {

        private ReferencesProvidersCollection _referencesProviders;

        public ReferencesRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/references")
        {
            _referencesProviders = new ReferencesProvidersCollection(server);
        }

#pragma warning disable 1998
        protected override async Task<ReferencesResponse> HandleMessage(ReferencesRequest parameters, RequestContext<ReferencesResponse> context)
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
#pragma warning restore 1998

    }
}
