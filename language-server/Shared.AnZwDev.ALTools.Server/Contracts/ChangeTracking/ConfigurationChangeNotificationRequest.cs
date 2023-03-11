using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.ChangeTracking
{
    public class ConfigurationChangeNotificationRequest
    {
        public ALProjectSource[] updatedProjects { get; set; }
    }
}
