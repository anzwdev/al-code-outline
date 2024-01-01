using AnZwDev.ALTools.Hover;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    internal class HoverRequestHandler : RequestHandler
    {

        private HoverProvidersCollection _hoverProviders;

        public HoverRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
            _hoverProviders = new HoverProvidersCollection(languageServerHost.ALDevToolsServer);
        }

        [JsonRpcMethod("al/hover", UseSingleObjectParameterDeserialization = true)]
        public HoverResponse Hover(HoverRequest parameters)
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

    }
}
