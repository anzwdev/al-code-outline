using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces
{
    public class ProjectSettings
    {

        public string PackagesCachePath { get; set; } = "";
        public string? RootNamespace { get; set; }
        public List<string>? CodeAnalyzers { get; set; }
        public List<string>? MandatoryPrefixes { get; set; }
        public List<string>? MandatorySuffixes { get; set; }
        public List<string>? MandatoryAffixes { get; set; }
        public List<string>? AdditionalMandatoryAffixesPatterns { get; set; }

    }
}
