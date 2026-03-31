using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class FileSystemFileChangeNotificationHandler : RequestHandler
    {

        public FileSystemFileChangeNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/fsFileChange", UseSingleObjectParameterDeserialization = true)]
        public void FSFileChange(FileSystemChangeNotificationRequest parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters.path))
                Services.GetService<Workspace>()?
                    .Projects.FindByPath(parameters.path)?
                    .Files.Find(parameters.path)?
                    .FileContentChanged();
        }

    }
}
