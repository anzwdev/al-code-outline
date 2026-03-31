using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class ConfigurationChangeNotificationHandler : RequestHandler
    {

        public ConfigurationChangeNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/configurationChange", UseSingleObjectParameterDeserialization = true)]
        public void ConfigurationChange(ConfigurationChangeNotificationRequest parameters)
        {
            if (parameters.updatedProjects != null)
            { 
                var workspace = Services.GetService<Workspace>();
                if (workspace != null)
                    for (int i = 0; i < parameters.updatedProjects.Length; i++)
                        if (!String.IsNullOrEmpty(parameters.updatedProjects[i].folderPath))
                            workspace.Projects
                                .FindByPath(parameters.updatedProjects[i].folderPath!)?
                                .Update(parameters.updatedProjects[i].ToProjectDescriptor());
            }    
        }

    }
}
