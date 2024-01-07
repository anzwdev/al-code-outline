using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentSaveNotificationHandler : RequestHandler
    {

        public DocumentSaveNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/documentSave", UseSingleObjectParameterDeserialization = true)]
        public void DocumentSave(DocumentChangeNotificationRequest parameters)
        {
            this.Server.Workspace.OnDocumentSave(parameters.path);
        }

    }
}
