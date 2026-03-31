using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class DocumentOpenNotificationHandler : RequestHandler
    {

        public DocumentOpenNotificationHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/documentOpen", UseSingleObjectParameterDeserialization = true)]
        public void DocumentOpen(DocumentChangeNotificationRequest parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters.path))
                Services.GetService<Workspace>()?
                    .Projects.FindByPath(parameters.path)?
                    .Files.Find(parameters.path)?
                    .OpenEditor();
        }

    }
}
