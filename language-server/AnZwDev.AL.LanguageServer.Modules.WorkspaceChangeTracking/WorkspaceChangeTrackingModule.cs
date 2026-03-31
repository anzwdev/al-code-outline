using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Handlers;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking
{
    public class WorkspaceChangeTrackingModule : LanguageServerModule
    {
        
        public WorkspaceChangeTrackingModule(LanguageServerHost languageServerHost) : 
            base(languageServerHost)
        {
        }

        protected override void RegisterHandlers()
        {
            base.RegisterHandlers();

            RegisterRequestHandler(new WorkspaceFoldersChangeNotificationHandler(Services));
            RegisterRequestHandler(new DocumentOpenNotificationHandler(Services));
            RegisterRequestHandler(new DocumentContentChangeRequestHandler(Services));
            RegisterRequestHandler(new DocumentCloseNotificationHandler(Services));

            RegisterRequestHandler(new FileSystemFileCreateNotificationHandler(Services));
            RegisterRequestHandler(new FileSystemFileDeleteNotificationHandler(Services));
            RegisterRequestHandler(new FileSystemFileChangeNotificationHandler(Services));

            RegisterRequestHandler(new ConfigurationChangeNotificationHandler(Services));
        }

    }
}
