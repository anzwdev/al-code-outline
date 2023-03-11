using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class TableFieldCaptionInfo
    {
        public string FieldName { get; set; }
        public LabelInformation Caption { get; set; }
        public string Description { get; set; }

        public TableFieldCaptionInfo() : this(null, null, null)
        {
        }

        public TableFieldCaptionInfo(LabelInformation caption) : this(caption, null, null)
        {
        }

        public TableFieldCaptionInfo(LabelInformation caption, string description, string fieldName)
        {
            this.FieldName = fieldName;
            this.Caption = caption;
            this.Description = description;
        }

    }
}
