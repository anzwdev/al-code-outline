using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.CodeTransformations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands.Parameters
{
    internal class TriggersOrderParameter
    {

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("order")]
        public string[] Triggers { get; set; }

        public TriggersOrderParameter()
        { 
        }

        public TriggersOrder ToTriggersOrder()
        {
            //try to convert 
            if ((!String.IsNullOrEmpty(Parent)) && 
                (this.Triggers != null) && 
                (this.Triggers.Length > 0))
            {
                var kind = ConvertParentToKind();
                if (kind != ConvertedSyntaxKind.None)
                    return new TriggersOrder(kind, this.Triggers);
            }
            return null;
        }

        private ConvertedSyntaxKind ConvertParentToKind()
        {
            if (Enum.TryParse<ConvertedSyntaxKind>(Parent, true, out var kind))
                return kind;

            switch (Parent.ToLower())
            {
                case "codeunit": return ConvertedSyntaxKind.CodeunitObject;
                case "table": return ConvertedSyntaxKind.TableObject;
                case "tableextension": return ConvertedSyntaxKind.TableExtensionObject;
                case "field": return ConvertedSyntaxKind.Field;
                case "fieldextension": return ConvertedSyntaxKind.FieldModification;
                case "page": return ConvertedSyntaxKind.PageObject;
                case "requestpage": return ConvertedSyntaxKind.RequestPage;
                case "requestpageextension": return ConvertedSyntaxKind.RequestPageExtension;
                case "pageextension": return ConvertedSyntaxKind.PageExtensionObject;
                case "pagefield": return ConvertedSyntaxKind.PageField;
                case "pagefieldextension": return ConvertedSyntaxKind.ControlModifyChange;
                case "action": return ConvertedSyntaxKind.PageAction;
                case "actionextension": return ConvertedSyntaxKind.ActionModifyChange;
                case "report": return ConvertedSyntaxKind.ReportObject;
                case "reportextension": return ConvertedSyntaxKind.ReportExtension;
                case "reportdataitem": return ConvertedSyntaxKind.ReportDataItem;
                case "reportextensiondatasetmodify": return ConvertedSyntaxKind.ReportExtensionDataSetModify;
                case "xmlport": return ConvertedSyntaxKind.XmlPortObject;
                case "xmlporttableelement": return ConvertedSyntaxKind.XmlPortTableElement;
                case "xmlportfieldelement": return ConvertedSyntaxKind.XmlPortFieldElement;
                case "xmlporttextelement": return ConvertedSyntaxKind.XmlPortTextElement;
                case "xmlportfieldattribute": return ConvertedSyntaxKind.XmlPortFieldAttribute;
                case "xmlporttextattribute": return ConvertedSyntaxKind.XmlPortTextAttribute;
                case "query": return ConvertedSyntaxKind.QueryObject;
            }
            return ConvertedSyntaxKind.None;
        }


    }
}
