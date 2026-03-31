using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers
{
    public class DocumentContentChangeRequestHandler : RequestHandler
    {

        public DocumentContentChangeRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("ws/documentContentChange", UseSingleObjectParameterDeserialization = true)]
        public DocumentContentChangeResponse DocumentContentChange(DocumentContentChangeRequest parameters)
        {
            if (!String.IsNullOrWhiteSpace(parameters.path))
                Services.GetService<Workspace>()?
                    .Projects.FindByPath(parameters.path)?
                    .Files.Find(parameters.path)?
                    .ChangeEditorContent(parameters.content ?? String.Empty);

            DocumentContentChangeResponse response = new DocumentContentChangeResponse
            {
                //!!! TO-DO (Refactoring) !!!
                //root = this.Server.Workspace.OnDocumentChange(parameters.path, parameters.content, parameters.returnSymbols)
            };
            return response;
        }

    }
}
