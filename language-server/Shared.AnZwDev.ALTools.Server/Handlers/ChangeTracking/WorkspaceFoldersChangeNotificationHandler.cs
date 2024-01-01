using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class WorkspaceFoldersChangeNotificationHandler: RequestHandler
    {

        public WorkspaceFoldersChangeNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("ws/workspaceFoldersChange", UseSingleObjectParameterDeserialization = true)]
        public void WorkspaceFoldersChange(WorkspaceFoldersChangeNotificationRequest parameters)
        {
            this.Server.Workspace.UpdateProjects(parameters.added, parameters.removed);
        }

    }
}
