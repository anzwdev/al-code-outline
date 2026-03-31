using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class WorkspaceFoldersChangeNotificationHandler: RequestHandler
    {

        public WorkspaceFoldersChangeNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/workspaceFoldersChange", UseSingleObjectParameterDeserialization = true)]
        public void WorkspaceFoldersChange(WorkspaceFoldersChangeNotificationRequest parameters)
        {
            var workspace = Services.GetService<Workspace>();
            if (workspace != null)
            {
                //convert parameters.added array to project descriptors
                List<ProjectDescriptor>? addProjects = parameters.added?.Select(p => p.ToProjectDescriptor()).ToList();
                List<ProjectDescriptor>? removeProjects = parameters.removed?.Select(p => new ProjectDescriptor() { ProjectPath = p }).ToList();
                workspace.Projects.Update(addProjects, removeProjects);
            }
        }

    }
}
