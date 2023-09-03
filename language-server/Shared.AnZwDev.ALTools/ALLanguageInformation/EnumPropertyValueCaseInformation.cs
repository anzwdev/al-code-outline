using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public static class EnumPropertyValueCaseInformation
    {

        private static CaseInformationCollection _values = null;
        public static CaseInformationCollection Values
        {
            get
            {
                if (_values == null)
                    InitValues();
                return _values;
            }
        }

        private static void InitValues()
        {
            _values = new CaseInformationCollection();
            _values.AddRange("AccountData", "Action", "ActionRef", "Additional", "Administration", "Always", "Ambiguous", "API", "Application", 
                "ApplicationScope", "Area", "Attention", "AttentionAccent", "Attribute", "AutomaticFeed", "Average", "BigInteger", "BigText", 
                "Bitmap", "BlankNeg", "BlankNegAndZero", "BlankPos", "BlankZero", "BlankZeroAndPos", "Both", "Browse", "BuiltInMethod", 
                "BuiltInOperator", "Card", "CardPart", "Cassette", "Category10", "Category11", "Category12", "Category13", "Category14", 
                "Category15", "Category16", "Category17", "Category18", "Category19", "Category20", "Category4", "Category5", "Category6", 
                "Category7", "Category8", "Category9", "CDS", "ChartPart", "Cloud", "Codeunit", "CollapseAll", "Columns", "ConcurrentUserServicePlan", 
                "ConfirmationDialog", "ContextMenu", "ControlAddInEvent", "Count", "Create", "CRM", "CrossJoin", "CueGroup", "Custom1", "Custom10", 
                "Custom11", "Custom12", "Custom13", "Custom14", "Custom15", "Custom16", "Custom2", "Custom3", "Custom4", "Custom5", "Custom6", 
                "Custom7", "Custom8", "Custom9", "CustomAction", "CustomerContent", "Day", "DeclareMethod", "Default", "Delegated", "Disabled", 
                "Document", "Documents", "DontBlank", "Edit", "Element", "EMail", "EndUserIdentifiableInformation", "EndUserPseudonymousIdentifiers", 
                "Envelope", "EventTrigger", "Excel", "Exchange", "ExpandAll", "Export", "Extension", "ExternalSQL", "Favorable", "Field", "Fixed", 
                "FixedText", "FlatRateServicePlan", "Flow", "FlowField", "FlowFilter", "FlowTemplate", "FlowTemplateGallery", "FormSource", "Full", 
                "FullOuterJoin", "Function", "Grid", "Group", "HeadlinePart", "History", "Implicit", "Import", "InheritFromTestCodeunit", "InnerJoin", 
                "Insert", "Install", "Integer", "Internal", "ISO88592", "Json", "Label", "LargeCapacity", "LargeFormat", "LeftOuterJoin", "LeftSwipe", 
                "Legacy", "List", "ListPart", "ListPlus", "Lists", "Local", "Lower", "Manual", "ManualFeed", "Many", "Masked", "Max", "Memo", "Method", 
                "MicrosoftGraph", "Middle", "Min", "Modify", "Month", "MSDOS", "NavigatePage", "Never", "New", "No", "None", "NonRestrictive", 
                "Normal", "Once", "OnPrem", "Optional", "OrganizationIdentifiableInformation", "Page", "Part", "Pending", "Person", "Personalization", 
                "PerUserOfferPlan", "PerUserServicePlan", "PhoneNo", "PrintLayout", "Process", "Promoted", "Property", "Protected", "Public", "Ratio", 
                "RDLC", "Read", "ReadExclusive", "ReadOnly", "ReadShared", "ReadUncommitted", "ReadWrite", "Removed", "Repeater", "Report", 
                "ReportPreview", "ReportProcessingOnly", "ReportsAndAnalysis", "Required", "Restrictive", "RightOuterJoin", "RightSwipe", "Role", 
                "RoleCenter", "Row", "Rows", "Separator", "SmallFormat", "Snapshot", "SplitButton", "Standard", "StandardAccent", "StandardDialog", 
                "StaticAutomatic", "Strong", "StrongAccent", "Subordinate", "SubPart", "Sum", "SystemMetadata", "SystemPart", "Table", "Tasks", 
                "Temporary", "Test", "TestRunner", "Text", "ToBeClassified", "TractorFeed", "Trigger", "Unbounded", "Undefined", "Unfavorable", 
                "Unlicensed", "Unspecified", "Update", "UpdateNoLocks", "Upgrade", "Upper", "URL", "UserControl", "UserDefined", "UTF16", "UTF8", 
                "V10", "V11", "Varchar", "VariableText", "Variant", "View", "WhenSpecified", "Wide", "WINDOWS", "Word", "Worksheet", "Xml", "XmlPort", 
                "Year", "Yes", "Zero", "ZeroOrOne");
        }

    }
}
