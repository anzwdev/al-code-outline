using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentOpenNotificationHandler : RequestHandler
    {

        public DocumentOpenNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/documentOpen", UseSingleObjectParameterDeserialization = true)]
        public void DocumentOpen(DocumentChangeNotificationRequest parameters)
        {
            this.Server.Workspace.OnDocumentOpen(parameters.path);
        }

    }
}
