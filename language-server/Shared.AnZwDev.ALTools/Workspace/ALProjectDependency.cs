using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectDependency
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public VersionNumber Version { get; set; }
        public bool Propagated { get; set; }
        public bool InternalsVisible { get; set; }

        public ALProject SourceProject { get; set; }
        public AppPackageInformation SourceAppFile { get; set; }
        public ALAppSymbolReference Symbols { get; set; }

        public ALProjectDependency()
        {
            this.Propagated = false;
            this.InternalsVisible = false;
        }

        public ALProjectDependency(string id, string name, string publisher, string version)
        {
            this.Id = id;
            this.Name = name;
            this.Publisher = publisher;
            this.Version = new VersionNumber(version);
            this.Propagated = false;
            this.InternalsVisible = false;
        }

        public bool IdReferencesReplaced()
        {
            return ((this.SourceProject != null) || (this.Symbols == null) || (this.Symbols.IdReferencesReplaced()));
        }

    }
}
