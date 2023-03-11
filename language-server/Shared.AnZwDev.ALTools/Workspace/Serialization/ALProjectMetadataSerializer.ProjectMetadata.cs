using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace.Serialization
{
    public partial class ALProjectMetadataSerializer
    {

        private class ProjectMetadata
        {

            #region Public properties

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("publisher")]
            public string Publisher { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("platform")]
            public string Platform { get; set; }

            [JsonProperty("application")]
            public string Application { get; set; }

            [JsonProperty("test")]
            public string Test { get; set; }

            [JsonProperty("dependencies")]
            public ProjectDependencyMetadata[] Dependencies { get; set; }

            [JsonProperty("idRange")]
            public ProjectIdRangeMetadata Range { get; set; }

            [JsonProperty("idRanges")]
            public ProjectIdRangeMetadata[] Ranges { get; set; }

            [JsonProperty("Runtime")]
            public string Runtime { get; set; }

            [JsonProperty("internalsVisibleTo")]
            public ProjectDependencyMetadata[] InternalsVisibleTo { get; set; }

            #endregion

            #region Initialization

            public ProjectMetadata()
            {
            }

            #endregion

            #region Dependencies

            public void PopulateDependencies(ALProjectDependenciesCollection projectDependencies)
            {
                //copy dependencies properties
                if (!String.IsNullOrWhiteSpace(this.Platform))
                    projectDependencies.Platform = new ALProjectDependency(null, "System", "Microsoft", this.Platform);
                if (!String.IsNullOrWhiteSpace(this.Application))
                    projectDependencies.Application = new ALProjectDependency(null, "Application", "Microsoft", this.Application);
                if (!String.IsNullOrWhiteSpace(this.Test))
                    projectDependencies.Test = new ALProjectDependency(null, "Test", "Microsoft", this.Test);
                
                //copy other dependencies
                if (this.Dependencies != null)
                {
                    for (int i = 0; i < this.Dependencies.Length; i++)
                    {
                        ProjectDependencyMetadata dependency = this.Dependencies[i];
                        projectDependencies.Add(new ALProjectDependency(dependency.GetId(), dependency.Name, dependency.Publisher, dependency.Version));
                    }
                }
            }

            #endregion

            #region Conversion to ALProjectMetadata

            public ALProjectProperties ToALProjectProperties()
            {
                ALProjectProperties properties = new ALProjectProperties();
                this.CopyProperties(properties);
                this.CopyIdRanges(properties);
                this.CopyInternalsVisibleTo(properties);
                return properties;
            }

            private void CopyProperties(ALProjectProperties targetProperties)
            {
                targetProperties.Id = this.Id;
                targetProperties.Name = this.Name;
                targetProperties.Publisher = this.Publisher;
                targetProperties.Version = new Core.VersionNumber(this.Version);
                targetProperties.Runtime = new Core.VersionNumber(this.Runtime);
            }

            private void CopyIdRanges(ALProjectProperties targetProperties)
            {
                if (this.Range != null)
                    targetProperties.AddIdRange(this.Range.From, this.Range.To);
                if (this.Ranges != null)
                {
                    for (int i=0; i<this.Ranges.Length; i++)
                    {
                        ProjectIdRangeMetadata idRange = this.Ranges[i];
                        targetProperties.AddIdRange(idRange.From, idRange.To);
                    }
                }
            }

            private void CopyInternalsVisibleTo(ALProjectProperties targetProperties)
            {
                if ((this.InternalsVisibleTo != null) && (this.InternalsVisibleTo.Length > 0))
                {
                    targetProperties.InternalsVisibleTo = new List<ALProjectReference>();
                    for (int i = 0; i < this.InternalsVisibleTo.Length; i++)
                    {
                        ProjectDependencyMetadata dependency = this.InternalsVisibleTo[i];
                        targetProperties.InternalsVisibleTo.Add(new ALProjectReference(dependency.GetId(), dependency.Name, dependency.Publisher, dependency.Version));
                    }
                }
                else
                    targetProperties.InternalsVisibleTo = null;
            }

            #endregion

        }

    }
}
