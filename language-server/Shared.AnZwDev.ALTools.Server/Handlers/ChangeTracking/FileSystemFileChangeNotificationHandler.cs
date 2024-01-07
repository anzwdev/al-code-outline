using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileSystemFileChangeNotificationHandler : RequestHandler
    {

        public FileSystemFileChangeNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/fsFileChange", UseSingleObjectParameterDeserialization = true)]
        public void FSFileChange(FileSystemChangeNotificationRequest parameters)
        {
            this.Server.Workspace.OnFileSystemFileChange(parameters.path);

        }

    }
}
