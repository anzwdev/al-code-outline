using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class DocumentCloseNotificationHandler : RequestHandler
    {

        public DocumentCloseNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/documentClose", UseSingleObjectParameterDeserialization = true)]
        public void DocumentClose(DocumentChangeNotificationRequest parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters.path))
            {
                Services.GetService<Workspace>()?
                    .Projects.FindByPath(parameters.path)?
                    .Files.Find(parameters.path)?
                    .CloseEditor();
            }
        }

    }
}
