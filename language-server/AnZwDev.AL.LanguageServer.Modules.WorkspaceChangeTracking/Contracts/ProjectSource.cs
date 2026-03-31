using AnZwDev.AL.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts
{
    public class ProjectSource
    {
        public string? folderPath { get; set; }
        public string? rootNamespace { get; set; }
        public string? packageCachePath { get; set; }
        public List<string>? codeAnalyzers { get; set; }
        public List<string>? additionalMandatoryAffixesPatterns { get; set; }

        public ProjectDescriptor ToProjectDescriptor()
        {
            return new ProjectDescriptor()
            {
                ProjectPath = folderPath,
                Settings = new ProjectSettings()
                {
                    RootNamespace = rootNamespace,
                    PackagesCachePath = packageCachePath ?? String.Empty,
                    CodeAnalyzers = codeAnalyzers,
                    AdditionalMandatoryAffixesPatterns = additionalMandatoryAffixesPatterns
                }
            };
        }

    }

}
