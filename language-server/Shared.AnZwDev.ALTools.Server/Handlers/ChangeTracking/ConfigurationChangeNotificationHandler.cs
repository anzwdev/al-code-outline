using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class ConfigurationChangeNotificationHandler : RequestHandler
    {

        public ConfigurationChangeNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/configurationChange", UseSingleObjectParameterDeserialization = true)]
        public void ConfigurationChange(ConfigurationChangeNotificationRequest parameters)
        {
            this.Server.Workspace.UpdateProjectsConfiguration(parameters.updatedProjects);
        }

    }
}
