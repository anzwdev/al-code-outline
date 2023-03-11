using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.ChangeTracking
{
    public class DocumentChangeNotificationRequest
    {
        public string path { get; set; }
        public string content { get; set; }
    }
}
