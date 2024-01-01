using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileCreateNotificationHandler : RequestHandler
    {

        public FileCreateNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/fileCreate", UseSingleObjectParameterDeserialization = true)]
        public void FileCreate(FilesNotificationRequest parameters)
        {
            this.Server.Workspace.OnFilesCreate(parameters.files);

        }

    }
}