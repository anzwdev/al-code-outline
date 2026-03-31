using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class FileSystemFileDeleteNotificationHandler : RequestHandler
    {

        public FileSystemFileDeleteNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/fsFileDelete", UseSingleObjectParameterDeserialization = true)]
        public void FSFileDelete(FileSystemChangeNotificationRequest parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters.path))
                Services.GetService<Workspace>()?
                    .Projects.FindByPath(parameters.path)?
                    .Files.Remove(parameters.path);
        }

    }
}
