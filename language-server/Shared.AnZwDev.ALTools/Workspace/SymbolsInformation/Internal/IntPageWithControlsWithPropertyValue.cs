using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation.Internal
{
    internal class IntPageWithControlsWithPropertyValue
    {

        public string Name { get; set; }
        public string SourceTable { get; set; }
        public string PropertyName { get; }
        public Dictionary<string, IntPageControlWithPropertyValue> Controls { get; }

        public IntPageWithControlsWithPropertyValue(ALAppPage alAppPage, string propertyName)
        {
            //get page properties
            this.Controls = new Dictionary<string, IntPageControlWithPropertyValue>();
            this.Name = alAppPage.Name;
            this.PropertyName = propertyName;
            if (alAppPage.Properties != null)
                this.SourceTable = alAppPage.Properties.GetValue("SourceTable");

            //add controls
            this.AddControls(alAppPage.Controls);
        }

        protected void AddControls(ALAppElementsCollection<ALAppPageControl> alAppControlsList)
        {
            if (alAppControlsList != null)
                for (int i = 0; i < alAppControlsList.Count; i++)
                    this.AddControl(alAppControlsList[i]);
        }

        protected void AddControl(ALAppPageControl alAppControl)
        {
            //add control details
            string expression = alAppControl.GetSourceExpression();
            if (!String.IsNullOrWhiteSpace(expression))
            {
                string key = alAppControl.Name?.ToLower();
                if (key != null)
                {
                    if (this.Controls.ContainsKey(key))
                    {
                        ALAppProperty property = alAppControl.Properties?.GetProperty(this.PropertyName);
                        if ((property != null) && (property.Value != null))
                            this.Controls[key].PropertyValue = property.Value;
                    }
                    else
                    {
                        this.Controls.Add(key, new IntPageControlWithPropertyValue()
                        {
                            ControlName = alAppControl.Name,
                            ControlSource = expression,
                            PropertyValue = alAppControl.Properties?.GetProperty(this.PropertyName)?.Value
                        });
                    }
                }
            }

            //add child controls
            this.AddControls(alAppControl.Controls);
        }

        public void ApplyPageExtension(ALAppPageExtension alAppPageExtension)
        {
            //collect control changes
            if (alAppPageExtension.ControlChanges != null)
                for (int i=0; i < alAppPageExtension.ControlChanges.Count; i++)
                {
                    this.AddControls(alAppPageExtension.ControlChanges[i].Controls);
                }
        }

    }
}
