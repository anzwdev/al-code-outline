using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class WarningDirectiveInfo
    {

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        
        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public WarningDirectiveInfoKind Kind { get; set; }
        
        [JsonProperty("fullPath", NullValueHandling = NullValueHandling.Ignore)]
        public string FullPath { get; set; }
        
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        
        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public Range Range { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("childItems", NullValueHandling = NullValueHandling.Ignore)]
        public List<WarningDirectiveInfo> ChildItems { get; set; }

        public static WarningDirectiveInfo Create(ALProject project, List<WarningDirectiveInfo> childItems = null)
        {
            return new WarningDirectiveInfo()
            {
                Title = Path.GetFileName(project.RootPath),
                FullPath = project.RootPath,
                Kind = WarningDirectiveInfoKind.Project,
                ChildItems = childItems
            };
        }

        public static WarningDirectiveInfo Create(ALProjectFile file)
        {
            return new WarningDirectiveInfo()
            {
                Title = Path.GetFileName(file.FullPath),
                FullPath = file.FullPath,
                Kind = WarningDirectiveInfoKind.File
            };
        }

        public static WarningDirectiveInfo Create(string ruleId, bool disabled, Range range)
        {
            var title = disabled ? "Disabled" : "Enabled";
            title = title + " - "+ ruleId + ", Ln " + (range.start.line + 1).ToString();

            return new WarningDirectiveInfo()
            {
                Title = title,
                Disabled = disabled,
                Range = range,
                Kind = WarningDirectiveInfoKind.DirectiveLocation
            };
        }

        public static WarningDirectiveInfo CreateRule(string ruleId, string ruleDescription)
        {
            return new WarningDirectiveInfo()
            {
                Title = String.IsNullOrWhiteSpace(ruleDescription) ? ruleId : ruleId + " - " + ruleDescription,
                Description = ruleDescription,
                Kind = WarningDirectiveInfoKind.Rule
            };
        }

        public void AddChildItem(WarningDirectiveInfo info)
        {
            if (ChildItems == null)
                ChildItems = new List<WarningDirectiveInfo>();
            ChildItems.Add(info);
        }

    }
}
