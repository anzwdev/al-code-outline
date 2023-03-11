using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.ChangeTracking
{
    public class WorkspaceFoldersChangeNotificationRequest
    {

        public ALProjectSource[] added { get; set; }
        public string[] removed { get; set; }

    }
}
