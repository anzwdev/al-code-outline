using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentCloseNotificationHandler : RequestHandler
    {

        public DocumentCloseNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/documentClose", UseSingleObjectParameterDeserialization = true)]
        public void DocumentClose(DocumentChangeNotificationRequest parameters)
        {
            this.Server.Workspace.OnDocumentClose(parameters.path);
        }

    }
}
