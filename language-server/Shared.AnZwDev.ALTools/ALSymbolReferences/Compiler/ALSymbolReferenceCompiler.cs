using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Extensions;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.WorkspaceCommands;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.ALTools.ALSymbolReferences.Compiler
{
    public class ALSymbolReferenceCompiler
    {

        public ALSymbolReferenceCompiler()
        {
        }

        public (List<ALAppObject>, List<ALAppDirective>) CreateObjectsAndDirectivesList(string sourcePath, string source)
        {
            return CreateObjectsAndDirectivesList(sourcePath, SyntaxTree.ParseObjectText(source));
        }

        public (List<ALAppObject>, List<ALAppDirective>) CreateObjectsAndDirectivesList(string sourcePath, SyntaxTree syntaxTree)
        {
            return (CreateObjectsList(sourcePath, syntaxTree),
                CreateDirectivesList(syntaxTree));
        }

        public List<ALAppObject> CreateObjectsList(string sourcePath, string source)
        {
            return CreateObjectsList(sourcePath, SyntaxTree.ParseObjectText(source));
        }

        public List<ALAppObject> CreateObjectsList(string sourcePath, SyntaxTree syntaxTree)
        {
            SyntaxNode node = syntaxTree.GetRoot();
            if (node != null)
            {
                List<ALAppObject> alObjectsList = new List<ALAppObject>();
                this.ProcessSyntaxNode(node, alObjectsList);
                foreach (ALAppObject alAppObject in alObjectsList)
                {
                    alAppObject.ReferenceSourceFileName = sourcePath;
                    alAppObject.INT_Parsed = true;
                }
                return alObjectsList;
            }

            return null;
        }

        #region Process directives

        private List<ALAppDirective> CreateDirectivesList(SyntaxTree syntaxTree)
        {
#if BC
            var node = syntaxTree.GetRoot();
            var directiveSyntax = node?.GetFirstDirective();
            if (directiveSyntax != null)
            {
                List<ALAppDirective> alDirectivesList = new List<ALAppDirective>();
                while (directiveSyntax != null)
                {
                    var alDirective = CreateALAppDirective(syntaxTree, directiveSyntax);
                    if (alDirective != null)
                        alDirectivesList.Add(alDirective);
                    directiveSyntax = directiveSyntax.GetNextDirective();
                }
                return alDirectivesList;
            }
#endif
            return null;
        }

#if BC

        private ALAppDirective CreateALAppDirective(SyntaxTree syntaxTree, DirectiveTriviaSyntax directiveSyntax)
        {
            switch (directiveSyntax)
            {
                case PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTriviaSyntax:
                    return new ALAppPragmaWarningDirective(
                        new Range(syntaxTree.GetLineSpan(directiveSyntax.FullSpan)),
                        pragmaWarningDirectiveTriviaSyntax.DisableOrRestoreKeyword.Kind.ConvertToLocalType() == ConvertedSyntaxKind.DisableKeyword,
                        GetRulesIds(pragmaWarningDirectiveTriviaSyntax.ErrorCodes));
                case PragmaImplicitWithDirectiveTriviaSyntax pragmaImplicitWithDirectiveTriviaSyntax:
                    break;
            }
            return null;
        }

        private List<string> GetRulesIds(SeparatedSyntaxList<CodeExpressionSyntax> errorCodes)
        {
            if ((errorCodes == null) || (errorCodes.Count == 0))
                return null;
            List<string> rules = new List<string>();
            for (int i=0; i<errorCodes.Count; i++)
            {
                string code = errorCodes[i].ToString();
                if (!String.IsNullOrWhiteSpace(code))
                    rules.Add(code);
            }
            return rules;
        }

#endif

#endregion


#region Process syntax nodes

protected void ProcessSyntaxNodesList(IEnumerable<SyntaxNode> nodesList, List<ALAppObject> alObjectsList)
        {
            if (nodesList != null)
            {
                foreach (SyntaxNode node in nodesList)
                {
                    this.ProcessSyntaxNode(node, alObjectsList);
                }
            }
        }

        protected void ProcessSyntaxNode(SyntaxNode node, List<ALAppObject> alObjectsList)
        {
            ConvertedSyntaxKind convertedKind = node.Kind.ConvertToLocalType();
            switch (convertedKind)
            {
                case ConvertedSyntaxKind.TableObject:
                    alObjectsList.Add(this.CreateTable((TableSyntax)node));
                    break;
                case ConvertedSyntaxKind.PageObject:
                    alObjectsList.Add(this.CreatePage((PageSyntax)node));
                    break;
                case ConvertedSyntaxKind.ReportObject:
                    alObjectsList.Add(this.CreateReport((ReportSyntax)node));
                    break;
                case ConvertedSyntaxKind.XmlPortObject:
                    alObjectsList.Add(this.CreateXmlPort((XmlPortSyntax)node));
                    break;
                case ConvertedSyntaxKind.QueryObject:
                    alObjectsList.Add(this.CreateQuery((QuerySyntax)node));
                    break;
                case ConvertedSyntaxKind.CodeunitObject:
                    alObjectsList.Add(this.CreateCodeunit((CodeunitSyntax)node));
                    break;
                case ConvertedSyntaxKind.ControlAddInObject:
                    alObjectsList.Add(this.CreateControlAddIn((ControlAddInSyntax)node));
                    break;
                case ConvertedSyntaxKind.PageExtensionObject:
                    alObjectsList.Add(this.CreatePageExtension((PageExtensionSyntax)node));
                    break;
                case ConvertedSyntaxKind.TableExtensionObject:
                    alObjectsList.Add(this.CreateTableExtension((TableExtensionSyntax)node));
                    break;
                case ConvertedSyntaxKind.ProfileObject:
                    alObjectsList.Add(this.CreateProfile((ProfileSyntax)node));
                    break;
                case ConvertedSyntaxKind.PageCustomizationObject:
                    alObjectsList.Add(this.CreatePageCustomization((PageCustomizationSyntax)node));
                    break;
#if BC
                case ConvertedSyntaxKind.DotNetPackage:
                    alObjectsList.Add(this.CreateDotNetPackage((DotNetPackageSyntax)node));
                    break;
                case ConvertedSyntaxKind.EnumType:
                    alObjectsList.Add(this.CreateEnumType((EnumTypeSyntax)node));
                    break;
                case ConvertedSyntaxKind.EnumExtensionType:
                    alObjectsList.Add(this.CreateEnumExtensionType((EnumExtensionTypeSyntax)node));
                    break;
                case ConvertedSyntaxKind.Interface:
                    alObjectsList.Add(this.CreateInterface((InterfaceSyntax)node));
                    break;

                case ConvertedSyntaxKind.ReportExtensionObject:
                    alObjectsList.Add(this.CreateReportExtension((ReportExtensionSyntax)node));
                    break;
                case ConvertedSyntaxKind.PermissionSet:
                    alObjectsList.Add(this.CreatePermissionSet((PermissionSetSyntax)node));
                    break;
                case ConvertedSyntaxKind.PermissionSetExtension:
                    alObjectsList.Add(this.CreatePermissionSetExtension((PermissionSetExtensionSyntax)node));
                    break;

#endif
                default:
                    this.ProcessSyntaxNodesList(node.ChildNodes(), alObjectsList);
                    break;
            }
        }

#endregion

        #region Table methods

        protected ALAppTable CreateTable(TableSyntax node)
        {
            ALAppTable alObject = new ALAppTable();

            this.ProcessApplicationObject(alObject, node);

            if (node.Fields != null)
                alObject.Fields = this.CreateTableFieldsList(node.Fields.Fields);
            if (node.Keys != null)
                alObject.Keys = this.CreateTableKeysList(node.Keys.Keys);
            if (node.FieldGroups != null)
                alObject.FieldGroups = this.CreateTableFieldGroupsList(node.FieldGroups.FieldGroups);

            return alObject;
        }

        #region Table fields

        protected ALAppElementsCollection<ALAppTableField> CreateTableFieldsList(SyntaxList<FieldSyntax> nodeList)
        {
            if ((nodeList != null) && (nodeList.Count > 0))
            {
                ALAppElementsCollection<ALAppTableField> list = new ALAppElementsCollection<ALAppTableField>();
                foreach (FieldSyntax node in nodeList)
                {
                    list.Add(this.CreateTableField(node));
                }
                return list;
            }
            return null;
        }

        protected ALAppTableField CreateTableField(FieldSyntax node)
        {
            ALAppTableField alElement = new ALAppTableField();
            if (node.No != null)
                alElement.Id = this.GetIntValue(node.No.ValueText);
            alElement.Name = node.GetNameStringValue();
            alElement.TypeDefinition = this.CreateTypeDefinition(node.Type);
            if (node.PropertyList != null)
                alElement.Properties = this.CreatePropertiesList(node.PropertyList.Properties);
            return alElement;
        }

        #endregion

        #region Table keys

        protected ALAppElementsCollection<ALAppTableKey> CreateTableKeysList(SyntaxList<KeySyntax> nodeList)
        {
            if ((nodeList != null) && (nodeList.Count > 0))
            {
                ALAppElementsCollection<ALAppTableKey> list = new ALAppElementsCollection<ALAppTableKey>();
                foreach (KeySyntax node in nodeList)
                {
                    list.Add(this.CreateTableKey(node));
                }
                return list;
            }
            return null;
        }

        protected ALAppTableKey CreateTableKey(KeySyntax node)
        {
            ALAppTableKey alElement = new ALAppTableKey();
            alElement.Name = node.GetNameStringValue();

            if ((node.Fields != null) && (node.Fields.Count > 0))
            {
                alElement.FieldNames = new string[node.Fields.Count];
                for (int i = 0; i < alElement.FieldNames.Length; i++)
                {
                    alElement.FieldNames[i] = node.Fields[i].ToString();
                }
            }

            return alElement;
        }

#endregion

        #region Table field groups

        protected ALAppElementsCollection<ALAppFieldGroup> CreateTableFieldGroupsList(SyntaxList<FieldGroupSyntax> nodeList)
        {
            if ((nodeList != null) && (nodeList.Count > 0))
            {
                ALAppElementsCollection<ALAppFieldGroup> list = new ALAppElementsCollection<ALAppFieldGroup>();
                foreach (FieldGroupSyntax node in nodeList)
                {
                    list.Add(this.CreateTableFieldGroup(node));
                }
                return list;
            }
            return null;
        }

        protected ALAppFieldGroup CreateTableFieldGroup(FieldGroupSyntax node)
        {
            ALAppFieldGroup alElement = new ALAppFieldGroup();
            alElement.Name = node.GetNameStringValue();

            if ((node.Fields != null) && (node.Fields.Count > 0))
            {
                alElement.FieldNames = new string[node.Fields.Count];
                for (int i = 0; i < alElement.FieldNames.Length; i++)
                {
                    alElement.FieldNames[i] = node.Fields[i].ToString();
                }
            }

            return alElement;
        }

#endregion

        #endregion

        #region Page methods

        protected ALAppPage CreatePage(PageSyntax node)
        {
            ALAppPage alObject = new ALAppPage();

            this.ProcessApplicationObject(alObject, node);

            if (node.Layout != null)
                alObject.Controls = this.CreatePageControlsList(node.Layout.Areas);
            if (node.Actions != null)
                alObject.Actions = this.CreatePageActionsList(node.Actions.Areas);

            return alObject;
        }

        #region Page controls

        protected ALAppElementsCollection<ALAppPageControl> CreatePageControlsList(SyntaxList<PageAreaSyntax> nodeList)
        {
            ALAppElementsCollection<ALAppPageControl> controls = new ALAppElementsCollection<ALAppPageControl>();
            foreach (PageAreaSyntax controlSyntax in nodeList)
            {
                controls.Add(this.CreatePageControl(controlSyntax));
            }
            return controls;
        }

        protected ALAppElementsCollection<ALAppPageControl> CreatePageControlsList(SyntaxList<ControlBaseSyntax> nodeList)
        {
            ALAppElementsCollection<ALAppPageControl> controls = new ALAppElementsCollection<ALAppPageControl>();
            foreach (ControlBaseSyntax controlSyntax in nodeList)
            {
                controls.Add(this.CreatePageControl(controlSyntax));
            }
            return controls;
        }

        protected ALAppPageControl CreatePageControl(ControlBaseSyntax node)
        {
            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
            ALAppPageControl control = new ALAppPageControl();

            control.Name = node.GetNameStringValue();
            if (node.PropertyList != null)
                control.Properties = this.CreatePropertiesList(node.PropertyList.Properties);
            if (node.ControlKeyword != null)
                control.Kind = this.GetPageControlKind(kind, node.ControlKeyword.ToString().NotNull().ToLower());

            switch (kind)
            {
                case ConvertedSyntaxKind.PageField:
                    this.ProcessPageField(control, (PageFieldSyntax)node);
                    break;
                case ConvertedSyntaxKind.PageArea:
                case ConvertedSyntaxKind.PageGroup:
                    this.ProcessPageGroup(control, node as ControlGroupBaseSyntax);
                    break;
            }

            return control;
        }

        private void ProcessPageField(ALAppPageControl control, PageFieldSyntax node)
        {
            if (node.Expression != null)
                control.SetSourceExpression(node.Expression.ToString());
        }

        private void ProcessPageGroup(ALAppPageControl control, ControlGroupBaseSyntax node)
        {
            if ((node != null) && (node.Controls != null))
                control.Controls = this.CreatePageControlsList(node.Controls);
        }

        private ALAppPageControlKind GetPageControlKind(ConvertedSyntaxKind kind, string controlKeyword)
        {
            switch (kind)
            {
                case ConvertedSyntaxKind.PageArea:
                    return ALAppPageControlKind.Area;
                case ConvertedSyntaxKind.PageGroup:
                    if (controlKeyword.Equals("group"))
                        return ALAppPageControlKind.Group;
                    if (controlKeyword.Equals("repeater"))
                        return ALAppPageControlKind.Repeater;
                    if (controlKeyword.Equals("cuegroup"))
                        return ALAppPageControlKind.CueGroup;
                    if (controlKeyword.Equals("grid"))
                        return ALAppPageControlKind.Grid;
                    if (controlKeyword.Equals("fixed"))
                        return ALAppPageControlKind.Fixed;
                    return ALAppPageControlKind.Group;
                case ConvertedSyntaxKind.PageField:
                    return ALAppPageControlKind.Field;
                case ConvertedSyntaxKind.PageLabel:
                    return ALAppPageControlKind.Label;
                case ConvertedSyntaxKind.PageSystemPart:
                    return ALAppPageControlKind.SystemPart;
                case ConvertedSyntaxKind.PageChartPart:
                    return ALAppPageControlKind.Chart;
                case ConvertedSyntaxKind.PagePart:
                    return ALAppPageControlKind.Part;
                case ConvertedSyntaxKind.PageUserControl:
                    return ALAppPageControlKind.UserControl;
            }
            return ALAppPageControlKind.Group;
        }

        #endregion

        #region Page actions

        protected ALAppElementsCollection<ALAppPageAction> CreatePageActionsList(SyntaxList<PageActionAreaSyntax> nodesList)
        {
            ALAppElementsCollection<ALAppPageAction> actionsList = new ALAppElementsCollection<ALAppPageAction>();
            foreach (PageActionAreaSyntax actionSyntax in nodesList)
            {
                actionsList.Add(CreatePageAction(actionSyntax));
            }
            return actionsList;
        }

        protected ALAppElementsCollection<ALAppPageAction> CreatePageActionsList(SyntaxList<ActionBaseSyntax> nodesList)
        {
            ALAppElementsCollection<ALAppPageAction> actionsList = new ALAppElementsCollection<ALAppPageAction>();
            foreach (ActionBaseSyntax actionSyntax in nodesList)
            {
                actionsList.Add(CreatePageAction(actionSyntax));
            }
            return actionsList;
        }

        protected ALAppPageAction CreatePageAction(ActionBaseSyntax node)
        {
            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
            ALAppPageAction action = new ALAppPageAction();

            action.Name = node.GetNameStringValue();
            if (node.PropertyList != null)
                action.Properties = this.CreatePropertiesList(node.PropertyList.Properties);
            action.Kind = this.GetActionKind(kind);

            if ((kind == ConvertedSyntaxKind.PageActionGroup) || (kind == ConvertedSyntaxKind.PageActionArea))
            {
                ActionGroupBaseSyntax groupSyntax = node as ActionGroupBaseSyntax;
                if ((groupSyntax != null) && (groupSyntax.Actions != null))
                    action.Actions = this.CreatePageActionsList(groupSyntax.Actions);
            }

            return action;
        }

        private ALAppPageActionKind GetActionKind(ConvertedSyntaxKind kind)
        {
            switch (kind)
            {
                case ConvertedSyntaxKind.PageAction:
                    return ALAppPageActionKind.Action;
                case ConvertedSyntaxKind.PageActionArea:
                    return ALAppPageActionKind.Area;
                case ConvertedSyntaxKind.PageActionGroup:
                    return ALAppPageActionKind.Group;
                case ConvertedSyntaxKind.PageActionSeparator:
                    return ALAppPageActionKind.Separator;
            }
            return ALAppPageActionKind.Group;
        }

        #endregion

        #endregion

        #region Report methods

        protected ALAppReport CreateReport(ReportSyntax node)
        {
            ALAppReport alObject = new ALAppReport();

            this.ProcessApplicationObject(alObject, node);

            if (node.RequestPage != null)
                alObject.RequestPage = this.CreateRequestPage(node.RequestPage);

            if (node.DataSet != null)
                alObject.DataItems = this.CreateReportDataItemsList(node.DataSet.DataItems);

            return alObject;
        }

        #region Request page

        protected ALAppRequestPage CreateRequestPage(RequestPageSyntax node)
        {
            ALAppRequestPage page = new ALAppRequestPage();
            page.Name = node.GetNameStringValue();

            if (node.PropertyList != null)
                page.Properties = this.CreatePropertiesList(node.PropertyList.Properties);
            if (node.Layout != null)
                page.Controls = this.CreatePageControlsList(node.Layout.Areas);
            if (node.Actions != null)
                page.Actions = this.CreatePageActionsList(node.Actions.Areas);

            return page;
        }

        #endregion

        #region Report data items

        protected ALAppElementsCollection<ALAppReportDataItem> CreateReportDataItemsList(SyntaxList<ReportDataItemSyntax> nodesList)
        {
            ALAppElementsCollection<ALAppReportDataItem> list = new ALAppElementsCollection<ALAppReportDataItem>();
            foreach (ReportDataItemSyntax dataItemSyntax in nodesList)
            {
                list.Add(this.CreateReportDataItem(dataItemSyntax));
            }
            return list;
        }

        protected ALAppReportDataItem CreateReportDataItem(ReportDataItemSyntax node)
        {
            ALAppReportDataItem dataItem = new ALAppReportDataItem();
            dataItem.Name = node.GetNameStringValue();
            if (node.DataItemTable != null)
                dataItem.RelatedTable = ALSyntaxHelper.DecodeName(node.DataItemTable.ToString());

            if ((node.Elements != null) && (node.Elements.Count > 0))
            {
                foreach (ReportDataItemElementSyntax elementSyntax in node.Elements)
                {
                    this.ProcessReportDataItemElement(dataItem, elementSyntax);
                }    
            }

            return dataItem;
        }

        protected void ProcessReportDataItemElement(ALAppReportDataItem dataItem, ReportDataItemElementSyntax elementSyntax)
        {
            ConvertedSyntaxKind kind = elementSyntax.Kind.ConvertToLocalType();
            switch (kind)
            {
                case ConvertedSyntaxKind.ReportDataItem:
                    if (dataItem.DataItems == null)
                        dataItem.DataItems = new ALAppElementsCollection<ALAppReportDataItem>();
                    dataItem.DataItems.Add(this.CreateReportDataItem((ReportDataItemSyntax)elementSyntax));
                    break;
                case ConvertedSyntaxKind.ReportColumn:
                    if (dataItem.Columns == null)
                        dataItem.Columns = new ALAppElementsCollection<ALAppReportColumn>();
                    dataItem.Columns.Add(this.CreateReportColumn((ReportColumnSyntax)elementSyntax));
                    break;
            }
        }

        protected ALAppReportColumn CreateReportColumn(ReportColumnSyntax node)
        {
            ALAppReportColumn column = new ALAppReportColumn();
            column.Name = node.GetNameStringValue();
            if (node.SourceExpression != null)
                column.SourceExpression = node.SourceExpression.ToString();
            return column;
        }

        #endregion

        #endregion

        #region Report extension methods

#if BC

        protected ALAppReportExtension CreateReportExtension(ReportExtensionSyntax node)
        {
            ALAppReportExtension alObject = new ALAppReportExtension();
            this.ProcessApplicationObject(alObject, node);

            if (node.BaseObject != null)
                alObject.Target = ALSyntaxHelper.DecodeName(node.BaseObject.ToString());

            if (node.RequestPage != null)
                alObject.RequestPage = this.CreateRequestPageExtension(node.RequestPage);

            if ((node.DataSet != null) && (node.DataSet.Changes != null))
                this.ProcessReportExtensionDataSetChanges(alObject, node.DataSet.Changes);

            return alObject;
        }


        #region Request page extension

        protected ALAppRequestPageExtension CreateRequestPageExtension(RequestPageExtensionSyntax node)
        {
            ALAppRequestPageExtension alObject = new ALAppRequestPageExtension();
            alObject.Name = node.GetNameStringValue();

            if (node.PropertyList != null)
                alObject.Properties = this.CreatePropertiesList(node.PropertyList.Properties);
            if (node.Layout != null)
                alObject.ControlChanges = this.CreatePageExtensionControlChangesList(node.Layout.Changes);
            if (node.Actions != null)
                alObject.ActionChanges = this.CreatePageExtensionActionChangesList(node.Actions.Changes);
            return alObject;
        }

        #endregion

        #region Report changes

        protected void ProcessReportExtensionDataSetChanges(ALAppReportExtension alObject, SyntaxList<ReportExtensionDataSetChangeBaseSyntax> changesList)
        {
            foreach (ReportExtensionDataSetChangeBaseSyntax change in changesList)
            {
                ConvertedSyntaxKind kind = change.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.ReportExtensionAddColumnChange:
                        this.ProcessReportExtensionAddColumn(alObject, (ReportExtensionDataSetAddColumnSyntax)change);
                        break;
                    case ConvertedSyntaxKind.ReportExtensionAddDataItemChange:
                        this.ProcessReportExtensionAddDataItem(alObject, (ReportExtensionDataSetAddDataItemSyntax)change);
                        break;
                }
            }
        }

        protected void ProcessReportExtensionAddColumn(ALAppReportExtension alObject, ReportExtensionDataSetAddColumnSyntax change)
        {
            if ((change.Columns != null) && (change.Columns.Count > 0))
            {
                string parentName = (change.Anchor != null) ? ALSyntaxHelper.DecodeName(change.Anchor.ToString()) : "";
                if (alObject.Columns == null)
                    alObject.Columns = new ALAppElementsCollection<ALAppReportColumn>();
                foreach (ReportColumnSyntax columnSyntax in change.Columns)
                {
                    ALAppReportColumn column = CreateReportColumn(columnSyntax);
                    column.OwningDataItemName = parentName;
                    alObject.Columns.Add(column);
                }
            }
        }

        protected void ProcessReportExtensionAddDataItem(ALAppReportExtension alObject, ReportExtensionDataSetAddDataItemSyntax change)
        {
            if ((change.DataItems != null) && (change.DataItems.Count > 0))
            {
                string parentName = (change.Anchor != null) ? ALSyntaxHelper.DecodeName(change.Anchor.ToString()) : "";
                if (alObject.DataItems == null)
                    alObject.DataItems = new ALAppElementsCollection<ALAppReportDataItem>();
                foreach (ReportDataItemSyntax dataItemSyntax in change.DataItems)
                {
                    ALAppReportDataItem dataItem = CreateReportDataItem(dataItemSyntax);
                    dataItem.OwningDataItemName = parentName;
                    alObject.DataItems.Add(dataItem);
                }
            }
        }

        #endregion


#endif

        #endregion

        #region XmlPort methods

        protected ALAppXmlPort CreateXmlPort(XmlPortSyntax node)
        {
            ALAppXmlPort alObject = new ALAppXmlPort();
            this.ProcessApplicationObject(alObject, node);

            if (node.XmlPortRequestPage != null)
                alObject.RequestPage = this.CreateRequestPage(node.XmlPortRequestPage);
            
            //add elements
            if ((node.XmlPortSchema != null) && (node.XmlPortSchema.XmlPortSchema != null))
            {
                alObject.Schema = new ALAppElementsCollection<ALAppXmlPortNode>();
                foreach (XmlPortNodeSyntax childNode in node.XmlPortSchema.XmlPortSchema)
                {
                    alObject.Schema.Add(CreateXmlPortNode(childNode));
                }
            }

            return alObject;
        }

        protected ALAppXmlPortNode CreateXmlPortNode(XmlPortNodeSyntax node)
        {
            ALAppXmlPortNode alXmlPortNode = new ALAppXmlPortNode();
            alXmlPortNode.Name = node.GetNameStringValue();
            
            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
            switch (kind)
            {
                case ConvertedSyntaxKind.XmlPortFieldElement:
                    alXmlPortNode.Kind = ALAppXmlPortNodeKind.FieldElement;
                    ProcessXmlPortFieldNode(alXmlPortNode, (XmlPortFieldNodeSyntax)node);
                    break;
                case ConvertedSyntaxKind.XmlPortFieldAttribute:
                    alXmlPortNode.Kind = ALAppXmlPortNodeKind.FieldAttribute;
                    ProcessXmlPortFieldNode(alXmlPortNode, (XmlPortFieldNodeSyntax)node);
                    break;
                case ConvertedSyntaxKind.XmlPortTableElement:
                    alXmlPortNode.Kind = ALAppXmlPortNodeKind.TableElement;
                    ProcessXmlPortTableElement(alXmlPortNode, (XmlPortTableElementSyntax)node);
                    break;
                case ConvertedSyntaxKind.XmlPortTextElement:
                    alXmlPortNode.Kind = ALAppXmlPortNodeKind.TextElement;
                    break;
                case ConvertedSyntaxKind.XmlPortTextAttribute:
                    alXmlPortNode.Kind = ALAppXmlPortNodeKind.TextAttribute;
                    break;
            }

            if ((node.Schema != null) && (node.Schema.Count > 0))
            {
                alXmlPortNode.Schema = new ALAppElementsCollection<ALAppXmlPortNode>();
                foreach (XmlPortNodeSyntax childNode in node.Schema)
                {
                    alXmlPortNode.Schema.Add(CreateXmlPortNode(childNode));
                }
            }

            return alXmlPortNode;
        }

        protected void ProcessXmlPortFieldNode(ALAppXmlPortNode alXmlPortNode, XmlPortFieldNodeSyntax node)
        {
            if (node.SourceField != null)
                alXmlPortNode.Expression = ALSyntaxHelper.DecodeName(node.SourceField.ToString()).Trim();
        }

        protected void ProcessXmlPortTableElement(ALAppXmlPortNode alAppXmlPortNode, XmlPortTableElementSyntax node)
        {
            if (node.SourceTable != null)
                alAppXmlPortNode.Expression = ALSyntaxHelper.DecodeName(node.SourceTable.ToString()).Trim();
        }


#endregion

        #region Query methods

        protected ALAppQuery CreateQuery(QuerySyntax node)
        {
            ALAppQuery alObject = new ALAppQuery();

            this.ProcessApplicationObject(alObject, node);

            if (node.Elements != null)
                alObject.Elements = this.CreateQueryDataItemsList(node.Elements.DataItems);

            return alObject;
        }

#region Query data items

        protected ALAppElementsCollection<ALAppQueryDataItem> CreateQueryDataItemsList(SyntaxList<QueryDataItemSyntax> nodesList)
        {
            ALAppElementsCollection<ALAppQueryDataItem> list = new ALAppElementsCollection<ALAppQueryDataItem>();
            foreach (QueryDataItemSyntax dataItemSyntax in nodesList)
            {
                list.Add(this.CreateQueryDataItem(dataItemSyntax));
            }
            return list;
        }

        protected ALAppQueryDataItem CreateQueryDataItem(QueryDataItemSyntax node)
        {
            ALAppQueryDataItem dataItem = new ALAppQueryDataItem();

            dataItem.Name = node.GetNameStringValue();
            if (node.DataItemTable != null)
                dataItem.RelatedTable = ALSyntaxHelper.DecodeName(node.DataItemTable.ToString());

            if ((node.Elements != null) && (node.Elements.Count > 0))
            {
                foreach (QueryDataItemElementSyntax elementSyntax in node.Elements)
                {
                    this.ProcessQueryDataItemElement(dataItem, elementSyntax);        
                }
            }

            return dataItem;
        }

        protected void ProcessQueryDataItemElement(ALAppQueryDataItem dataItem, QueryDataItemElementSyntax elementSyntax)
        {
            ConvertedSyntaxKind kind = elementSyntax.Kind.ConvertToLocalType();
            switch (kind)
            {
                case ConvertedSyntaxKind.QueryColumn:
                    if (dataItem.Columns == null)
                        dataItem.Columns = new ALAppElementsCollection<ALAppQueryColumn>();
                    dataItem.Columns.Add(this.CreateQueryColumn((QueryColumnSyntax)elementSyntax));
                    break;
                case ConvertedSyntaxKind.QueryDataItem:
                    if (dataItem.DataItems == null)
                        dataItem.DataItems = new ALAppElementsCollection<ALAppQueryDataItem>();
                    dataItem.DataItems.Add(this.CreateQueryDataItem((QueryDataItemSyntax)elementSyntax));
                    break;
                case ConvertedSyntaxKind.QueryFilter:
                    if (dataItem.Filters == null)
                        dataItem.Filters = new ALAppElementsCollection<ALAppQueryFilter>();
                    dataItem.Filters.Add(this.CreateQueryFilter((QueryFilterSyntax)elementSyntax));
                    break;
            }
        }

        protected ALAppQueryColumn CreateQueryColumn(QueryColumnSyntax node)
        {
            ALAppQueryColumn column = new ALAppQueryColumn();
            column.Name = node.GetNameStringValue();
            if (node.RelatedField != null)
                column.SourceColumn = ALSyntaxHelper.DecodeName(node.RelatedField.ToString());
            return column;
        }

        protected ALAppQueryFilter CreateQueryFilter(QueryFilterSyntax node)
        {
            ALAppQueryFilter filter = new ALAppQueryFilter();
            filter.Name = node.GetNameStringValue();
            return filter;
        }

#endregion

#endregion

        #region Codeunit methods

        protected ALAppCodeunit CreateCodeunit(CodeunitSyntax node)
        {
            ALAppCodeunit alObject = new ALAppCodeunit();

            this.ProcessApplicationObject(alObject, node);

            return alObject;
        }

#endregion

        #region ControlAddIn methods

        protected ALAppControlAddIn CreateControlAddIn(ControlAddInSyntax node)
        {
            ALAppControlAddIn alObject = new ALAppControlAddIn();

            this.ProcessObject(alObject, node);

            return alObject;
        }

#endregion

        #region Page Extension methods

        protected ALAppPageExtension CreatePageExtension(PageExtensionSyntax node)
        {
            ALAppPageExtension alObject = new ALAppPageExtension();

            this.ProcessApplicationObject(alObject, node);

            alObject.TargetObject = ALSyntaxHelper.DecodeName(node.BaseObject.ToString());

            if (node.Layout != null)
                alObject.ControlChanges = this.CreatePageExtensionControlChangesList(node.Layout.Changes);
            if (node.Actions != null)
                alObject.ActionChanges = this.CreatePageExtensionActionChangesList(node.Actions.Changes);

            return alObject;
        }

#region Control changes

        protected ALAppElementsCollection<ALAppPageControlChange> CreatePageExtensionControlChangesList(SyntaxList<ControlChangeBaseSyntax> nodesList)
        {
            ALAppElementsCollection<ALAppPageControlChange> list = new ALAppElementsCollection<ALAppPageControlChange>();
            foreach (ControlChangeBaseSyntax changeSyntax in nodesList)
            {
                list.Add(CreatePageExtensionControlChange(changeSyntax));
            }
            return list;
        }

        protected ALAppPageControlChange CreatePageExtensionControlChange(ControlChangeBaseSyntax node)
        {
            ALAppPageControlChange change = new ALAppPageControlChange();

            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
            switch (kind)
            {
                case ConvertedSyntaxKind.ControlAddChange:
                    ProcessPageExtensionControlChange(change, (ControlAddChangeSyntax)node);
                    break;
                case ConvertedSyntaxKind.ControlModifyChange:
                    ProcessPageExtensionControlChange(change, (ControlModifyChangeSyntax)node);
                    break;
                case ConvertedSyntaxKind.ControlMoveChange:
                    ProcessPageExtensionControlChange(change, (ControlMoveChangeSyntax)node);
                    break;
            }

            return change;
        }

        protected void ProcessPageExtensionControlChange(ALAppPageControlChange change, ControlAddChangeSyntax node)
        {
            change.Name = "Add";
            if ((node.Controls != null) && (node.Controls.Count > 0))
                change.Controls = CreatePageControlsList(node.Controls);
        }

        protected void ProcessPageExtensionControlChange(ALAppPageControlChange change, ControlModifyChangeSyntax node)
        {
            change.Name = "Modify";
        }

        protected void ProcessPageExtensionControlChange(ALAppPageControlChange change, ControlMoveChangeSyntax node)
        {
            change.Name = "Move";
        }

#endregion

#region Action changes

        protected ALAppElementsCollection<ALAppPageActionChange> CreatePageExtensionActionChangesList(SyntaxList<ActionChangeBaseSyntax> nodesList)
        {
            ALAppElementsCollection<ALAppPageActionChange> list = new ALAppElementsCollection<ALAppPageActionChange>();
            foreach (ActionChangeBaseSyntax changeSyntax in nodesList)
            {
                list.Add(CreatePageExtensionActionChange(changeSyntax));
            }
            return list;
        }

        protected ALAppPageActionChange CreatePageExtensionActionChange(ActionChangeBaseSyntax node)
        {
            ALAppPageActionChange change = new ALAppPageActionChange();
            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();

            switch (kind)
            {
                case ConvertedSyntaxKind.ActionAddChange:
                    this.ProcessPageExtensionActionChange(change, (ActionAddChangeSyntax)node);
                    break;
                case ConvertedSyntaxKind.ActionModifyChange:
                    this.ProcessPageExtensionActionChange(change, (ActionModifyChangeSyntax)node);
                    break;
                case ConvertedSyntaxKind.ActionMoveChange:
                    this.ProcessPageExtensionActionChange(change, (ActionMoveChangeSyntax)node);
                    break;
            }

            return change;
        }

        protected void ProcessPageExtensionActionChange(ALAppPageActionChange change, ActionAddChangeSyntax node)
        {
            change.Name = "Add";
            if ((node.Actions != null) && (node.Actions.Count > 0))
                change.Actions = this.CreatePageActionsList(node.Actions);
        }

        protected void ProcessPageExtensionActionChange(ALAppPageActionChange change, ActionModifyChangeSyntax node)
        {
            change.Name = "Modify";
        }

        protected void ProcessPageExtensionActionChange(ALAppPageActionChange change, ActionMoveChangeSyntax node)
        {
            change.Name = "Move";
        }

#endregion

#endregion

        #region Table extension methods

        protected ALAppTableExtension CreateTableExtension(TableExtensionSyntax node)
        {
            ALAppTableExtension alObject = new ALAppTableExtension();

            this.ProcessApplicationObject(alObject, node);

            alObject.TargetObject = ALSyntaxHelper.DecodeName(node.BaseObject.ToString());

            if (node.Fields != null)
            {
                ALAppElementsCollection<ALAppTableField> modifiedFields = null;
                alObject.Fields = this.CreateTableExtensionFieldsList(node.Fields.Fields, out modifiedFields);
                alObject.FieldModifications = modifiedFields;
            }
            
            if (node.Keys != null)
                alObject.Keys = this.CreateTableKeysList(node.Keys.Keys);

        #if BC
            if (node.FieldGroups != null)
                alObject.FieldGroups = this.CreateTableExtensionFieldsGroups(node.FieldGroups.Changes);
        #endif

            return alObject;
        }

        #region Fields

        protected ALAppElementsCollection<ALAppTableField> CreateTableExtensionFieldsList(SyntaxList<FieldBaseSyntax> nodesList, out ALAppElementsCollection<ALAppTableField> modifiedFields)
        {
            modifiedFields = null;
            if ((nodesList != null) && (nodesList.Count > 0))
            {
                ALAppElementsCollection<ALAppTableField> list = new ALAppElementsCollection<ALAppTableField>();
                foreach (FieldBaseSyntax node in nodesList)
                {
                    ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
                    switch (kind)
                    {
                        case ConvertedSyntaxKind.Field:
                            FieldSyntax fieldNode = node as FieldSyntax;
                            if (fieldNode != null)
                                list.Add(this.CreateTableField(fieldNode));
                            break;
                        case ConvertedSyntaxKind.FieldModification:
                            FieldModificationSyntax fieldModification = node as FieldModificationSyntax;
                            if (fieldModification != null)
                            {
                                if (modifiedFields == null)
                                    modifiedFields = new ALAppElementsCollection<ALAppTableField>();
                                modifiedFields.Add(this.CreateTableFieldModification(fieldModification));
                            }
                            break;
                    }
                }
                return list;
            }
            return null;
        }

        protected ALAppTableField CreateTableFieldModification(FieldModificationSyntax node)
        {
            ALAppTableField alElement = new ALAppTableField();
            alElement.Name = node.GetNameStringValue();
            if (node.PropertyList != null)
                alElement.Properties = this.CreatePropertiesList(node.PropertyList.Properties);
            return alElement;
        }

        #endregion

        #region Field groups

#if BC
        protected ALAppElementsCollection<ALAppFieldGroup> CreateTableExtensionFieldsGroups(SyntaxList<FieldGroupChangeBaseSyntax> nodesList)
        {
            return null;
        }
#endif

        #endregion

        #endregion

        #region Profile methods

        protected ALAppProfile CreateProfile(ProfileSyntax node)
        {
            ALAppProfile alObject = new ALAppProfile();
#if BC
            this.ProcessObject(alObject, node);
#else
            this.ProcessObject(alObject, node);
#endif
            return alObject;
        }

#endregion

#region Page customization methods

        protected ALAppPageCustomization CreatePageCustomization(PageCustomizationSyntax node)
        {
            ALAppPageCustomization alObject = new ALAppPageCustomization();

            this.ProcessApplicationObject(alObject, node);

            return alObject;
        }

#endregion

#region DotNetPackage methods

#if BC
        protected ALAppDotNetPackage CreateDotNetPackage(DotNetPackageSyntax node)
        {
            ALAppDotNetPackage alObject = new ALAppDotNetPackage();

            this.ProcessObject(alObject, node);

            alObject.AssemblyDeclarations = this.CreateAssemblyDeclarationsList(node.Assemblies);

            return alObject;
        }
#endif

#region Assembly declaration

#if BC
        protected ALAppElementsCollection<ALAppDotNetAssemblyDeclaration> CreateAssemblyDeclarationsList(SyntaxList<DotNetAssemblySyntax> nodesList)
        {
            return null;
        }
#endif

#endregion

#endregion

#region Enum methods

#if BC
        protected ALAppEnum CreateEnumType(EnumTypeSyntax node)
        {
            ALAppEnum alObject = new ALAppEnum();

            this.ProcessApplicationObject(alObject, node);

            alObject.Values = this.CreateEnumValuesList(node.Values);

            return alObject;
        }
#endif

#region Enum values

#if BC
        protected ALAppElementsCollection<ALAppEnumValue> CreateEnumValuesList(SyntaxList<EnumValueSyntax> nodesList)
        {
            if ((nodesList != null) && (nodesList.Count > 0))
            {
                ALAppElementsCollection<ALAppEnumValue> list = new ALAppElementsCollection<ALAppEnumValue>();
                foreach (EnumValueSyntax node in nodesList)
                {
                    list.Add(this.CreateEnumValue(node));
                }
                return list;
            }
            return null;
        }

        protected ALAppEnumValue CreateEnumValue(EnumValueSyntax node)
        {
            ALAppEnumValue alElement = new ALAppEnumValue();
            if (node.Id != null)
                alElement.Ordinal = this.GetIntValue(node.Id.ValueText);
            alElement.Name = node.GetNameStringValue();
            return alElement;
        }
#endif

#endregion

#endregion

#region Enum extension methods

#if BC
        protected ALAppEnumExtension CreateEnumExtensionType(EnumExtensionTypeSyntax node)
        {
            ALAppEnumExtension alObject = new ALAppEnumExtension();

            this.ProcessApplicationObject(alObject, node);

            alObject.TargetObject = ALSyntaxHelper.DecodeName(node.BaseObject.ToString());

            alObject.Values = this.CreateEnumValuesList(node.Values);

            return alObject;
        }
#endif

#endregion

#region interface methods

#if BC
        protected ALAppInterface CreateInterface(InterfaceSyntax node)
        {
            ALAppInterface alObject = new ALAppInterface();

            this.ProcessObject(alObject, node);

            return alObject;
        }
#endif

        #endregion

        #region PermissionSet methods

#if BC

        protected ALAppPermissionSet CreatePermissionSet(PermissionSetSyntax node)
        {
            ALAppPermissionSet alObject = new ALAppPermissionSet();
            this.ProcessApplicationObject(alObject, node);
            alObject.Permissions = CreatePermissionsList(node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax);
            return alObject;
        }

        public ALAppElementsCollection<ALAppPermission> CreatePermissionsList(PermissionPropertyValueSyntax permissionsPropertyValue)
        {
            if (permissionsPropertyValue == null)
                return null;

            ALAppElementsCollection<ALAppPermission> alAppPermissionsCollection = new ALAppElementsCollection<ALAppPermission>();

            if ((permissionsPropertyValue.PermissionProperties != null) && (permissionsPropertyValue.PermissionProperties.Count > 0))
                foreach (var permissionProperty in permissionsPropertyValue.PermissionProperties)
                {
                    var objectType = ALSyntaxHelper.DecodeName(permissionProperty.ObjectType.ToString());
                    var asteriskPermission = permissionProperty.AsteriskToken.ToString();
                    var objectName = ALSyntaxHelper.DecodeName(permissionProperty.ObjectReference?.ToString());
                    var objectPermission = ALSyntaxHelper.DecodeName(permissionProperty.Permissions.ToString());
                    var objectId = -1;
                    if ((String.IsNullOrWhiteSpace(objectName)) && (!String.IsNullOrWhiteSpace(asteriskPermission)))
                    {
                        objectName = "*";
                        objectId = 0;
                    }

                    if ((!String.IsNullOrWhiteSpace(objectType)) && (!String.IsNullOrWhiteSpace(objectName)))
                    {
                        ALAppPermission alAppPermission = new ALAppPermission()
                        {
                            PermissionObject = ALAppPermissionObjectTypeExtensions.FromString(objectType),
                            Id = objectId,
                            Value = ALAppPermissionValueExtensions.FromString(objectPermission),
                            ObjectName = objectName
                        };
                        alAppPermissionsCollection.Add(alAppPermission);
                    }
                }

            return alAppPermissionsCollection;
        }

#endif

        #endregion

        #region PermissionSetExtension methods
#if BC

        protected ALAppPermissionSetExtension CreatePermissionSetExtension(PermissionSetExtensionSyntax node)
        {
            ALAppPermissionSetExtension alObject = new ALAppPermissionSetExtension();
            this.ProcessApplicationObject(alObject, node);
            if (node.BaseObject != null)
                alObject.TargetObject = ALSyntaxHelper.DecodeName(node.BaseObject.ToString());
            alObject.Permissions = CreatePermissionsList(node.GetPropertyValue("Permissions") as PermissionPropertyValueSyntax);
            return alObject;
        }
#endif

        #endregion

        #region AL Object methods

        protected void ProcessObject(ALAppObject alObject, ObjectSyntax node)
        {
            alObject.ReferenceSourceFileName = node.SyntaxTree.FilePath;
            alObject.Name = node.GetNameStringValue();
            alObject.INT_Parsed = true;

            if (node.PropertyList != null)
                alObject.Properties = this.CreatePropertiesList(node.PropertyList.Properties);

            this.ProcessApplicationObjectMembers(alObject, node.Members);
        }

        protected void ProcessApplicationObject(ALAppObject alObject, ApplicationObjectSyntax node)
        {
            if ((node.ObjectId != null) && (node.ObjectId.Value != null))
                alObject.Id = this.GetIntValue(node.ObjectId.Value.ValueText);
            this.ProcessObject(alObject, node);   
        }

        protected void ProcessApplicationObjectMembers(ALAppObject alObject, SyntaxList<MemberSyntax> nodeList)
        {
            if ((nodeList != null) && (nodeList.Count > 0))
            {
                alObject.Methods = new ALAppElementsCollection<ALAppMethod>();

                foreach (MemberSyntax memberNode in nodeList)
                {
                    ConvertedSyntaxKind convertedSyntaxKind = memberNode.Kind.ConvertToLocalType();
                    switch (convertedSyntaxKind)
                    {
                        case ConvertedSyntaxKind.MethodDeclaration:
                            alObject.Methods.Add(this.CreateMethodDeclaration((MethodDeclarationSyntax)memberNode));
                            break;
                        case ConvertedSyntaxKind.EventDeclaration:
                            alObject.Methods.Add(this.CreateMethodDeclaration((EventDeclarationSyntax)memberNode));
                            break;
                        //                        case ConvertedSyntaxKind.TriggerDeclaration:
                        //                        case ConvertedSyntaxKind.EventTriggerDeclaration:
                        //                            break;
#if BC
                        case ConvertedSyntaxKind.GlobalVarSection:
                            alObject.Variables = this.CreateGlobalVarSection((GlobalVarSectionSyntax)memberNode);
                            break;
#endif
                    }
                }
            }
        }

#endregion

#region Code methods

        protected ALAppMethod CreateMethodDeclaration(EventDeclarationSyntax node)
        {
            ALAppMethod method = new ALAppMethod();
            method.Name = node.GetNameStringValue();
            method.MethodKind = 5;
            method.Parameters = CreateMethodParameters(node.ParameterList);
            method.Attributes = this.CreateAttributes(node.Attributes);

            return method;
        }

        protected ALAppMethod CreateMethodDeclaration(MethodDeclarationSyntax node)
        {
            ALAppMethod method = new ALAppMethod();
            method.Name = node.GetNameStringValue();
            method.MethodKind = 0;
            method.Parameters = CreateMethodParameters(node.ParameterList);
            if ((node.ReturnValue != null) && (node.ReturnValue.Kind.ConvertToLocalType() != ConvertedSyntaxKind.None))
                method.ReturnTypeDefinition = this.CreateTypeDefinition(node.ReturnValue);
            method.Attributes = this.CreateAttributes(node.Attributes);
            //access modifier
            method.AccessModifier = null;
#if BC
            if (node.AccessModifier != null)
            {
                method.AccessModifier = node.AccessModifier.ToString();
                if (method.AccessModifier != null)
                {
                    method.AccessModifier = method.AccessModifier.ToLower();
                    method.IsLocal = method.AccessModifier.Equals("local");
                    method.IsProtected = method.AccessModifier.Equals("protected");
                    method.IsInternal = method.AccessModifier.Equals("internal");
                }
            }
#endif
            return method;
        }

        protected ALAppElementsCollection<ALAppMethodParameter> CreateMethodParameters(ParameterListSyntax parameterListSyntax)
        {
            if ((parameterListSyntax != null) && (parameterListSyntax.Parameters != null))
            {
                ALAppElementsCollection<ALAppMethodParameter> parameters = new ALAppElementsCollection<ALAppMethodParameter>();
                foreach (ParameterSyntax parameterSyntax in parameterListSyntax.Parameters)
                {
                    parameters.Add(this.CreateMethodParameter(parameterSyntax));
                }
                return parameters;
            }
            return null;
        }

        protected ALAppMethodParameter CreateMethodParameter(ParameterSyntax node)
        {
            ALAppMethodParameter parameter = new ALAppMethodParameter();
            parameter.Name = node.GetNameStringValue();
            parameter.TypeDefinition = CreateTypeDefinition(node.Type);
            parameter.IsVar = ((node.VarKeyword != null) && (node.VarKeyword.Kind.ConvertToLocalType() == ConvertedSyntaxKind.VarKeyword));

            return parameter;
        }

        protected ALAppElementsCollection<ALAppAttribute> CreateAttributes(SyntaxList<MemberAttributeSyntax> attributeListSyntax)
        {
            if ((attributeListSyntax != null) && (attributeListSyntax.Count > 0))
            {
                ALAppElementsCollection<ALAppAttribute> attributeList = new ALAppElementsCollection<ALAppAttribute>();
                foreach (MemberAttributeSyntax attributeSyntax in attributeListSyntax)
                {
                    ALAppAttribute attribute = new ALAppAttribute();
                    attribute.Name = attributeSyntax.GetNameStringValue();
                    attributeList.Add(attribute);
                }
                return attributeList;
            }
            return null;
        }

#if BC
        protected ALAppElementsCollection<ALAppVariable> CreateGlobalVarSection(GlobalVarSectionSyntax node)
        {
            if ((node.Variables != null) && (node.Variables.Count > 0))
            {
                var accessModifier = ALSymbolAccessModifier.Public;
                if (node.AccessModifier != null)
                {

                }

                ALAppElementsCollection<ALAppVariable> variables = new ALAppElementsCollection<ALAppVariable>();
                foreach (VariableDeclarationBaseSyntax variableSyntax in node.Variables)
                {
                    variables.Add(CreateVariable(variableSyntax));
                }
                return variables;
            }
            return null;
        }

        protected ALAppVariable CreateVariable(VariableDeclarationBaseSyntax node)
        {
            ALAppVariable variable = new ALAppVariable();
            variable.Name = node.GetNameStringValue();
            variable.TypeDefinition = CreateTypeDefinition(node.Type);
            return variable;
        }
#endif

#endregion

#region Data type methods

        protected ALAppTypeDefinition CreateTypeDefinition(TypeReferenceBaseSyntax node)
        {
            if ((node != null) && (node.DataType != null))
            {
                ALAppTypeDefinition type = CreateTypeDefinition(node.DataType);
                if (node.Array != null)
                    ProcessDataType(type, node.Array);

                ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
                if (kind == ConvertedSyntaxKind.RecordTypeReference)
                    this.ProcessDataType(type, (RecordTypeReferenceSyntax)node);

                return type;
            }
            return null;
        }

        protected ALAppTypeDefinition CreateTypeDefinition(ReturnValueSyntax node)
        {
            if (node.DataType != null)
                return this.CreateTypeDefinition(node.DataType);
            return null;
        }

        protected ALAppTypeDefinition CreateTypeDefinition(DataTypeSyntax node)
        {
            ALAppTypeDefinition alElement = new ALAppTypeDefinition();
            alElement.Name = node.TypeName.ToString();

            ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
            switch (kind)
            {
                case ConvertedSyntaxKind.OptionDataType:
                    this.ProcessDataType(alElement, (OptionDataTypeSyntax)node);
                    break;
#if BC
                case ConvertedSyntaxKind.EnumDataType:
                    this.ProcessDataType(alElement, (EnumDataTypeSyntax)node);
                    break;
#endif
                case ConvertedSyntaxKind.LengthDataType:
                    this.ProcessDataType(alElement, (LengthDataTypeSyntax)node);
                    break;
                case ConvertedSyntaxKind.SubtypedDataType:
                    this.ProcessDataType(alElement, (SubtypedDataTypeSyntax)node);
                    break;
                case ConvertedSyntaxKind.GenericDataType:
                    this.ProcessDataType(alElement, (GenericNamedDataTypeSyntax)node);
                    break;
            }
            return alElement;
        }

        protected void ProcessDataType(ALAppTypeDefinition alType, RecordTypeReferenceSyntax node)
        {
            if (node.Temporary != null)
            {
                string temporary = node.Temporary.ToString();
                alType.Temporary = ((temporary != null) && (temporary.Equals("temporary", StringComparison.CurrentCultureIgnoreCase)));

                if (node.DataType != null)
                {
                    ConvertedSyntaxKind kind = node.DataType.Kind.ConvertToLocalType();
                    if (kind == ConvertedSyntaxKind.SubtypedDataType)
                    {
                        SubtypedDataTypeSyntax subtypedDataTypeSyntax = node.DataType as SubtypedDataTypeSyntax;
                        if (subtypedDataTypeSyntax != null)
                        {
                            alType.Subtype = new ALAppSubtypeDefinition();
                            alType.Subtype.Name = ALSyntaxHelper.DecodeName(subtypedDataTypeSyntax.Subtype.ToString());
                        }
                    }
                    
                }
            
                
            }
        }

        protected void ProcessDataType(ALAppTypeDefinition alType, ArraySyntax node)
        {
            if ((node.DimensionList != null) && (node.DimensionList.Dimensions != null))
            {
                alType.ArrayDimensions = new List<int>();
                foreach (DimensionSyntax dimensionSyntax in node.DimensionList.Dimensions)
                {
                    string valueText = dimensionSyntax.Value.Text;
                    int value;
                    if (Int32.TryParse(valueText, out value))
                        alType.ArrayDimensions.Add(value);
                }
            }
        }

#if BC
        protected void ProcessDataType(ALAppTypeDefinition alType, EnumDataTypeSyntax node)
        {
            alType.Subtype = new ALAppSubtypeDefinition();
            alType.Subtype.Name = ALSyntaxHelper.DecodeName(node.EnumTypeName.ToString());
        }
#endif

        protected void ProcessDataType(ALAppTypeDefinition alType, GenericNamedDataTypeSyntax node)
        {
            if (node.TypeArguments != null)
            {
                alType.TypeArguments = new List<ALAppTypeDefinition>();
                foreach (DataTypeSyntax argumentSyntax in node.TypeArguments)
                {
                    alType.TypeArguments.Add(CreateTypeDefinition(argumentSyntax));
                }
            }
        }

        protected void ProcessDataType(ALAppTypeDefinition alType, LengthDataTypeSyntax node)
        {
            alType.Name = node.ToString();
        }

        protected void ProcessDataType(ALAppTypeDefinition alType, OptionDataTypeSyntax node)
        {
            if ((node.OptionValues != null) && (node.OptionValues.Options != null))
            {
                alType.OptionMembers = new List<string>();
                foreach (IdentifierNameOrEmptySyntax optionValueSyntax in node.OptionValues.Options)
                {
                    alType.OptionMembers.Add(optionValueSyntax.ToString());
                }
            }
        }

        protected void ProcessDataType(ALAppTypeDefinition alType, SubtypedDataTypeSyntax node)
        {
            alType.Subtype = new ALAppSubtypeDefinition();
            alType.Subtype.Name = ALSyntaxHelper.DecodeName(node.Subtype.ToString());
        }

#endregion

#region Base methods

#if NAV2018
        protected ALAppPropertiesCollection CreatePropertiesList(SyntaxList<PropertySyntax> nodesList)
        {
            if ((nodesList != null) && (nodesList.Count > 0))
            {
                ALAppPropertiesCollection list = new ALAppPropertiesCollection();

                foreach (PropertySyntax node in nodesList)
                {
                    PropertySyntax propertyNode = node as PropertySyntax;
                    if (propertyNode != null)
                        this.AddProperty(list, propertyNode);
                }

                return list;
            }
            return null;
        }
#else
        protected ALAppPropertiesCollection CreatePropertiesList(SyntaxList<PropertySyntaxOrEmpty> nodesList)
        {
            if ((nodesList != null) && (nodesList.Count > 0))
            {
                ALAppPropertiesCollection list = new ALAppPropertiesCollection();

                foreach (PropertySyntaxOrEmpty node in nodesList)
                {
                    PropertySyntax propertyNode = node as PropertySyntax;
                    if (propertyNode != null)
                        this.AddProperty(list, propertyNode);
                        //list.Add(this.CreateProperty(propertyNode));
                }

                return list;
            }
            return null;
        }
#endif

        protected void AddProperty(ALAppPropertiesCollection list, PropertySyntax node)
        {
            string name = null;
            if (node.Name != null)
                name = ALSyntaxHelper.DecodeName(node.Name.ToString());
            if (name != null)
                name = name.Trim();
            
            if (node.Value != null)
            {
                PropertyValueSyntax propertyValue = node.Value;
                ConvertedSyntaxKind propertyValueKind = propertyValue.Kind.ConvertToLocalType();
                switch (propertyValueKind)
                {
                    case ConvertedSyntaxKind.LabelPropertyValue:
                        AddProperty(list, name, propertyValue as LabelPropertyValueSyntax);
                        break;
                    default:
                        list.Add(new ALAppProperty(name, ALSyntaxHelper.DecodeStringOrName(node.Value.ToString())));
                        break;
                }
            }
        }

        protected void AddProperty(ALAppPropertiesCollection list, string name, LabelPropertyValueSyntax node)
        {
            LabelSyntax labelSyntax = node.Value;
            if (labelSyntax != null)
            {
                if (labelSyntax.LabelText != null)
                    list.Add(new ALAppProperty(name, ALSyntaxHelper.DecodeString(labelSyntax.LabelText.ToString())));
                
                //add property arguments
                if ((labelSyntax.Properties != null) && (labelSyntax.Properties.Values != null))
                {
                    foreach (IdentifierEqualsLiteralSyntax propertySyntax in labelSyntax.Properties.Values)
                    {
                        if ((propertySyntax.Identifier != null) && (propertySyntax.Literal != null))
                        {
                            list.Add(new ALAppProperty(
                                name + "." + propertySyntax.Identifier.ToString().Trim(),
                                ALSyntaxHelper.DecodeStringOrName(propertySyntax.Literal.ToString())));
                        }
                    }
                }
            }
        }

#endregion

        private int GetIntValue(string text)
        {
            int value;
            if (Int32.TryParse(text, out value))
                return value;
            return 0;
        }

    }
}
 