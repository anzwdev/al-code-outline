using AnZwDev.ALTools.Hover;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    internal class HoverRequestHandler : BaseALRequestHandler<HoverRequest, HoverResponse>
    {

        private HoverProvidersCollection _hoverProviders;

        public HoverRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/hover")
        {
            _hoverProviders = new HoverProvidersCollection(server);
        }

#pragma warning disable 1998
        protected override async Task<HoverResponse> HandleMessage(HoverRequest parameters, RequestContext<HoverResponse> context)
        {
            HoverResponse response = new HoverResponse();
            try
            {
                response.hover = _hoverProviders.ProvideHover(parameters.isActiveDocument, parameters.source, parameters.position);
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
