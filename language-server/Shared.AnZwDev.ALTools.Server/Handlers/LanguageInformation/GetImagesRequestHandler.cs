using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.Server.Contracts.LanguageInformation;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.LanguageInformation
{
    public class GetImagesRequestHandler : BaseALRequestHandler<GetImagesRequest, GetImagesResponse>
    {

        public GetImagesRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getimages")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetImagesResponse> HandleMessage(GetImagesRequest parameters, RequestContext<GetImagesResponse> context)
        {
            GetImagesResponse response = new GetImagesResponse();
            try
            {
                ImageInformationProvider imageInformationProvider = new ImageInformationProvider();
                switch (parameters.type)
                {
                    case "actionImages":
                        response.images = imageInformationProvider.GetActionImages();
                        break;
                    case "actionCueGroupImages":
                        response.images = imageInformationProvider.GetActionCueGroupImages();
                        break;
                    case "fieldCueGroupImages":
                        response.images = imageInformationProvider.GetFieldCueGroupImages();
                        break;
                    case "roleCenterActionImages":
                        response.images = imageInformationProvider.GetRoleCenterActionImages();
                        break;
                }
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
