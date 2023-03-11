using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class ConfigurationChangeNotificationHandler : BaseALNotificationHandler<ConfigurationChangeNotificationRequest>
    {

        public ConfigurationChangeNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "ws/configurationChange")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(ConfigurationChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.UpdateProjectsConfiguration(parameters.updatedProjects);
        }
#pragma warning restore 1998

    }
}
