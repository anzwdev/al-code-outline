using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace.Serialization
{
    public partial class ALProjectMetadataSerializer
    {

        public ALProjectMetadataSerializer()
        {
        }

        public static void LoadFromFile(ALProject project, string path)
        {
            LoadFromJson(project, FileUtils.SafeReadAllText(path));
        }

        public static void LoadFromJson(ALProject project, string jsonContent)
        {
            project.Dependencies.Clear();

            //deserialize to internal structures
            ProjectMetadata metadata = JsonConvert.DeserializeObject<ProjectMetadata>(jsonContent);

            //update project
            project.Properties = metadata.ToALProjectProperties();
            metadata.PopulateDependencies(project.Dependencies);
        }

    }
}
