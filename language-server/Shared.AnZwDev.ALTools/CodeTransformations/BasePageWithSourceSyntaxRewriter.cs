using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BasePageWithSourceSyntaxRewriter : ALSyntaxRewriter
    {

        protected TableInformationProvider TableInformationProvider { get; }
        protected List<TableFieldInformaton> TableFields { get; set; }
        protected String TableName { get; set; }

        protected ALSymbolKind _globalVarOwnerKind = ALSymbolKind.Undefined;
        protected string _globalVarOwnerName = null;
        protected Dictionary<string, ALAppVariable> _globalVariables = null;
        protected Dictionary<string, List<TableFieldInformaton>> _tablesCollectionWithFields = null;

        public BasePageWithSourceSyntaxRewriter()
        {
            this.TableInformationProvider = new TableInformationProvider();
            this.TableFields = null;
            this.TableName = null;
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.ReportObject, node.GetNameStringValue());
            return base.VisitReport(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.QueryObject, node.GetNameStringValue());
            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.XmlPortObject, node.GetNameStringValue());
            return base.VisitXmlPort(node);
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            this.CheckSourceTableProperty(node);
            return base.VisitRequestPage(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            this.TableFields = null;
            this.TableName = null;

            //get list of table fields
            if (node.BaseObject != null)
            {
                string pageName = ALSyntaxHelper.DecodeName(node.BaseObject.ToString());

                this.SetGlobalVarOwner(ALSymbolKind.PageObject, pageName);
                if (!String.IsNullOrWhiteSpace(pageName))
                {
                    PageInformationProvider pageInformationProvider = new PageInformationProvider();
                    this.TableFields = pageInformationProvider.GetAllTableFieldsForPage(this.Project, pageName, false, false, true, true, true);
                    this.TableName = this.Project.AllSymbols.Pages.FindObject(pageName)?.GetSourceTable();
                }
            }
            else
                this.SetGlobalVarOwner(ALSymbolKind.Undefined, null);

            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.PageObject, node.GetNameStringValue());
            this.CheckSourceTableProperty(node);
            return base.VisitPage(node);
        }

        protected void CheckSourceTableProperty(SyntaxNode node)
        {
            //try to find current table
            this.TableFields = null;
            this.TableName = null;
            PropertyValueSyntax sourceTablePropertyValue = node.GetPropertyValue("SourceTable");
            if (sourceTablePropertyValue != null)
            {
                string sourceTable = ALSyntaxHelper.DecodeName(sourceTablePropertyValue.ToString());
                if (!String.IsNullOrWhiteSpace(sourceTable))
                {
                    this.TableName = sourceTable;
                    this.TableFields = this.TableInformationProvider.GetTableFields(this.Project, sourceTable, false, false, true, true, true, false, null);
                }
            }
        }
        protected TableFieldCaptionInfo GetFieldCaption(PageFieldSyntax node)
        {
            //try to find source field caption
            string caption = null;
            string description = null;
            if (node.Expression != null)
            {
                List<TableFieldInformaton> fieldsList;
                string fieldName;
                string source = node.Expression.ToString().Trim();
                ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(source);

                //get field name and fields list
                if (String.IsNullOrWhiteSpace(memberAccessExpression.Expression))
                {
                    fieldName = memberAccessExpression.Name;
                    fieldsList = this.TableFields;
                }
                else
                {
                    fieldName = memberAccessExpression.Expression;
                    fieldsList = this.GetVariableFieldsCollection(memberAccessExpression.Name);
                }

                if ((fieldsList != null) && (fieldName != null))
                {
                    TableFieldInformaton tableField = fieldsList.Where(p => (fieldName.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault();
                    if (tableField != null)
                    {
                        description = tableField.Description;
                        if ((tableField.CaptionLabel != null) && (!String.IsNullOrWhiteSpace(tableField.CaptionLabel.Value)))
                            return new TableFieldCaptionInfo(tableField.CaptionLabel, description, fieldName);
                    }
                }

                if ((String.IsNullOrWhiteSpace(caption)) && (fieldName != null))
                    caption = fieldName.Replace("\"", "").RemovePrefixSuffix(
                        this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes, this.Project.AdditionalMandatoryAffixesPatterns);
            }
            return new TableFieldCaptionInfo(new LabelInformation("Caption", caption), description, null);
        }

        protected void SetGlobalVarOwner(ALSymbolKind kind, string name)
        {
            this._globalVarOwnerKind = kind;
            this._globalVarOwnerName = name;
            this._globalVariables = null;
        }

        protected void LoadGlobalVariables()
        {
            switch (_globalVarOwnerKind)
            {
                case ALSymbolKind.PageObject:
                    PageInformationProvider pageInformationProvider = new PageInformationProvider();
                    this.SetGlobalVariables(pageInformationProvider.GetVariables(this.Project, _globalVarOwnerName));
                    break;
                case ALSymbolKind.ReportObject:
                    ReportInformationProvider reportInformationProvider = new ReportInformationProvider();
                    this.SetGlobalVariables(reportInformationProvider.GetReportVariables(this.Project, _globalVarOwnerName));
                    break;
                case ALSymbolKind.QueryObject:
                    QueryInformationProvider queryInformationProvider = new QueryInformationProvider();
                    this.SetGlobalVariables(queryInformationProvider.GetQueryVariables(this.Project, _globalVarOwnerName));
                    break;
                case ALSymbolKind.XmlPortObject:
                    XmlPortInformationProvider xmlPortInformationProvider = new XmlPortInformationProvider();
                    this.SetGlobalVariables(xmlPortInformationProvider.GetXmlPortVariables(this.Project, _globalVarOwnerName));
                    break;
                default:
                    SetGlobalVariables(null);
                    break;
            }
        }

        protected void SetGlobalVariables(IEnumerable<ALAppVariable> variables)
        {
            _globalVariables = new Dictionary<string, ALAppVariable>();
            if (variables != null)
            {
                foreach (ALAppVariable alAppVariable in variables)
                {
                    if (!String.IsNullOrWhiteSpace(alAppVariable.Name))
                    {
                        string name = alAppVariable.Name.ToLower();
                        if (!_globalVariables.ContainsKey(name))
                            _globalVariables.Add(name, alAppVariable);
                    }
                }
            }
        }

        protected List<TableFieldInformaton> GetVariableFieldsCollection(string name)
        {
            name = name.ToLower();

            if (name == "rec")
                return this.TableFields;

            if (_globalVariables == null)
                this.LoadGlobalVariables();

            if (!_globalVariables.ContainsKey(name))
                return null;

            ALAppVariable variable = _globalVariables[name];
            if ((variable.TypeDefinition == null) ||
                (variable.TypeDefinition.Name == null) ||
                (variable.TypeDefinition.Subtype == null) ||                
                (!variable.TypeDefinition.Name.Equals("Record", StringComparison.CurrentCultureIgnoreCase)) ||
                (String.IsNullOrWhiteSpace(variable.TypeDefinition.Subtype.Name)))
                return null;

            string tableName = variable.TypeDefinition.Subtype.Name;
            string tableNameKey = tableName.ToLower();
            if (_tablesCollectionWithFields == null)
                _tablesCollectionWithFields = new Dictionary<string, List<TableFieldInformaton>>();
            if (_tablesCollectionWithFields.ContainsKey(tableNameKey))
                return _tablesCollectionWithFields[tableNameKey];

            List<TableFieldInformaton> fields = this.TableInformationProvider.GetTableFields(this.Project, tableName, false, false, true, true, true, false, null);
            _tablesCollectionWithFields.Add(tableNameKey, fields);
            return fields;
        }

        protected bool ToolTipDisabled(SyntaxNode node)
        {
            PropertySyntax propertySyntax = node.GetProperty("ShowCaption");
            if ((propertySyntax != null) && (propertySyntax.Value != null))
            {
                string value = propertySyntax.Value.ToString();
                return ((value != null) && (value.Equals("false", StringComparison.CurrentCultureIgnoreCase)));
            }
            return false;
        }

        protected bool HasToolTip(SyntaxNode node)
        {
            return ((node.HasNonEmptyProperty("ToolTip")) || (node.HasProperty("ToolTipML")));
        }

    }
}
