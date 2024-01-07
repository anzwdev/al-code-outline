using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileRenameNotificationHandler : RequestHandler
    {

        public FileRenameNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/fileRename", UseSingleObjectParameterDeserialization = true)]
        public void FileRename(FilesRenameNotificationRequest parameters)
        {
            if (parameters.files != null)
            {
                for (int i = 0; i < parameters.files.Length; i++)
                {
                    this.Server.Workspace.OnFileRename(parameters.files[i].oldPath, parameters.files[i].newPath);
                }
            }
        }

    }
}