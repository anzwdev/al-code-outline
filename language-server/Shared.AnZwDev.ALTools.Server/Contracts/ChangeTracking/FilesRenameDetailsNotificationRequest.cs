using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.ChangeTracking
{
    public class FilesRenameDetailsNotificationRequest
    {

        public string oldPath { get; set; }
        public string newPath { get; set; }

    }
}
