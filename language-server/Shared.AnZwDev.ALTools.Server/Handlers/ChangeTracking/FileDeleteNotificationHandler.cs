using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileDeleteNotificationHandler : RequestHandler
    {

        public FileDeleteNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/fileDelete", UseSingleObjectParameterDeserialization = true)]
        public void FileDelete(FilesNotificationRequest parameters)
        {
            this.Server.Workspace.OnFilesDelete(parameters.files);
        }

    }
}