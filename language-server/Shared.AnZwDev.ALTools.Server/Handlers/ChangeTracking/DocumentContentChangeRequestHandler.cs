using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentContentChangeRequestHandler : RequestHandler
    {

        public DocumentContentChangeRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/documentContentChange", UseSingleObjectParameterDeserialization = true)]
        public DocumentContentChangeResponse DocumentContentChange(DocumentContentChangeRequest parameters)
        {
            DocumentContentChangeResponse response = new DocumentContentChangeResponse
            {
                root = this.Server.Workspace.OnDocumentChange(parameters.path, parameters.content, parameters.returnSymbols)
            };
            return response;
        }

    }
}
