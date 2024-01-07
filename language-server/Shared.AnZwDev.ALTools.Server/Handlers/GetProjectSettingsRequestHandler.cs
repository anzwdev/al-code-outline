using System;
using System.Threading.Tasks;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetProjectSettingsRequestHandler : RequestHandler
    {

        public GetProjectSettingsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getprojectsettings", UseSingleObjectParameterDeserialization = true)]
        public GetProjectSettingsResponse GetProjectSettings(GetProjectSettingsRequest parameters)
        {
            GetProjectSettingsResponse response = new GetProjectSettingsResponse();
            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    response.mandatoryPrefixes = project.MandatoryPrefixes;
                    response.mandatorySuffixes = project.MandatorySuffixes;
                    response.mandatoryAffixes = project.MandatoryAffixes;
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
