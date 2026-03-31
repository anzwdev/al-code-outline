using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class FileSystemFileCreateNotificationHandler : RequestHandler
    {

        public FileSystemFileCreateNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/fsFileCreate", UseSingleObjectParameterDeserialization = true)]
        public void FSFileCreate(FileSystemChangeNotificationRequest parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters.path))
                Services.GetService<Workspace>()?
                    .Projects.FindByPath(parameters.path)?
                    .Files.Add(parameters.path);
        }

    }
}
