using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileSystemFileDeleteNotificationHandler : RequestHandler
    {

        public FileSystemFileDeleteNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/fsFileDelete", UseSingleObjectParameterDeserialization = true)]
        public void FSFileDelete(FileSystemChangeNotificationRequest parameters)
        {
            this.Server.Workspace.OnFileSystemFileDelete(parameters.path);
        }

    }
}
