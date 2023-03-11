using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Extensions;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableFieldInformaton : SymbolWithIdInformation
    {

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("fieldClass")]
        public ALAppTableFieldClass FieldClass { get; set; }

        [JsonProperty("state")]
        public ALAppTableFieldState State { get; set; }

        [JsonProperty("captionLabel")]
        public LabelInformation CaptionLabel { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("toolTips", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ToolTips { get; set; }

        public TableFieldInformaton()
        {
            this.InitializeLabels();
        }

        public TableFieldInformaton(int id, string name, string dataType)
        {
            this.Id = id;
            this.Name = name;
            this.Caption = name;
            this.DataType = dataType;
            this.Description = null;
            this.FieldClass = ALAppTableFieldClass.Normal;
            this.State = ALAppTableFieldState.Active;
            this.InitializeLabels();
        }

        public TableFieldInformaton(ALProject project, ALAppTableField symbolReference)
        {
            this.InitializeLabels();
            this.FieldClass = ALAppTableFieldClass.Normal;
            this.State = ALAppTableFieldState.Active;
            this.Id = symbolReference.Id;
            this.Name = symbolReference.Name;
            if (symbolReference.Properties != null)
            {
                this.Caption = symbolReference.Properties.GetValue("Caption");
                this.Description = symbolReference.Properties.GetValue("Description");
                this.CaptionLabel.Update(symbolReference.Properties);
                this.FieldClass = symbolReference.GetFieldClass();
                this.State = symbolReference.GetFieldState();
            }

            if (String.IsNullOrWhiteSpace(this.Caption))
            {
                string caption = this.Name;
                if (project != null)
                    caption = caption.RemovePrefixSuffix(
                        project.MandatoryPrefixes, project.MandatorySuffixes, project.MandatoryAffixes, project.AdditionalMandatoryAffixesPatterns);
                this.Caption = caption;
                this.CaptionLabel.SetValue(this.Caption);
            }

            this.DataType = symbolReference.TypeDefinition.Name;
        }

        private void InitializeLabels()
        {
            this.CaptionLabel = new LabelInformation("Caption");
        }

        public void UpdateProperties(ALAppPropertiesCollection propertiesCollection)
        {
            if (propertiesCollection != null)
            {
                //function is called from properties update in table extension,
                //so if Properties.GetValue returns null, it means that
                //property has not been changed and should not be updated here
                string caption = propertiesCollection.GetValue("Caption");
                if (!String.IsNullOrWhiteSpace(caption))
                {
                    this.Caption = caption;
                    this.CaptionLabel.Update(propertiesCollection);
                }
                string description = propertiesCollection.GetValue("Description");
                if (!String.IsNullOrWhiteSpace(description))
                    this.Description = description;
            }
        }

    }
}
