using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.Server.Contracts.LanguageInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.LanguageInformation
{
    public class GetImagesRequestHandler : RequestHandler
    {

        public GetImagesRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getimages", UseSingleObjectParameterDeserialization = true)]
        public GetImagesResponse GetImages(GetImagesRequest parameters)
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

    }
}
