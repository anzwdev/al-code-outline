using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectReference
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public VersionNumber Version { get; set; }

        public ALProjectReference()
        {
        }

        public ALProjectReference(string newId, string newName, string newPublisher, string newVersion)
        {
            this.Id = newId;
            this.Name = newName;
            this.Publisher = newPublisher;
            this.Version = new VersionNumber(newVersion);
        }


    }
}
