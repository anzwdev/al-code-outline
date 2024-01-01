using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileSystemFileCreateNotificationHandler : RequestHandler
    {

        public FileSystemFileCreateNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/fsFileCreate", UseSingleObjectParameterDeserialization = true)]
        public void FSFileCreate(FileSystemChangeNotificationRequest parameters)
        {
            this.Server.Workspace.OnFileSystemFileCreate(parameters.path);

        }

    }
}
