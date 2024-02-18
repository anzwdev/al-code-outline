using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BasePageWithSourceSyntaxRewriter : ALSyntaxRewriterWithNamespaces
    {

        protected TableInformationProvider TableInformationProvider { get; } = new TableInformationProvider();
        protected List<TableFieldInformaton> TableFields { get; set; } = null;
        protected ALAppTable Table { get; set; } = null;

        protected ALSymbolKind _globalVarOwnerKind = ALSymbolKind.Undefined;
        protected ALObjectReference? _globalVarOwner = null;
        protected Dictionary<string, ALAppVariable> _globalVariables = null;
        protected Dictionary<string, List<TableFieldInformaton>> _tablesCollectionWithFields = null;

        public BasePageWithSourceSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.ReportObject, new ALObjectReference(Usings, NamespaceName, node.GetNameStringValue()));
            return base.VisitReport(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.QueryObject, new ALObjectReference(Usings, NamespaceName, node.GetNameStringValue()));
            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.XmlPortObject, new ALObjectReference(Usings, NamespaceName, node.GetNameStringValue()));
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
            this.Table = null;

            //get list of table fields
            if (node.BaseObject != null)
            {
                var pageReference = new ALObjectReference(Usings, node.BaseObject.ToString());
                this.SetGlobalVarOwner(ALSymbolKind.PageObject, pageReference);
                if (!pageReference.IsEmpty())
                {
                    var page = this.Project
                        .GetAllSymbolReferences()
                        .GetAllObjects<ALAppPage>(x => x.Pages)
                        .FindFirst(pageReference);
                    if (page != null)
                    {
                        SetTable(page.GetSourceTable());
                        PageInformationProvider pageInformationProvider = new PageInformationProvider();
                        this.TableFields = pageInformationProvider.GetAllTableFieldsForPage(this.Project, page, false, false, true, true, true);

                    }
                }
            }
            else
                this.SetGlobalVarOwner(ALSymbolKind.Undefined, null);

            return base.VisitPageExtension(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            this.SetGlobalVarOwner(ALSymbolKind.PageObject, new ALObjectReference(Usings, NamespaceName, node.GetNameStringValue()));
            this.CheckSourceTableProperty(node);
            return base.VisitPage(node);
        }

        protected void CheckSourceTableProperty(SyntaxNode node)
        {
            //try to find current table
            this.TableFields = null;
            this.Table = null;
            PropertyValueSyntax sourceTablePropertyValue = node.GetPropertyValue("SourceTable");
            if (sourceTablePropertyValue != null)
            {
                var sourceTableReference = new ALObjectReference(Usings, sourceTablePropertyValue.ToString());
                if (!sourceTableReference.IsEmpty())
                {
                    SetTable(sourceTableReference);
                    if (Table != null)
                        this.TableFields = this.TableInformationProvider.GetTableFields(this.Project, sourceTableReference, false, false, true, true, true, false, null);
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
                    TableFieldInformaton tableField = fieldsList.Where(p => (fieldName.Equals(p.Name, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
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

        protected void SetGlobalVarOwner(ALSymbolKind kind, ALObjectReference? objectReference)
        {
            this._globalVarOwnerKind = kind;
            this._globalVarOwner = objectReference;
            this._globalVariables = null;
        }

        protected void LoadGlobalVariables()
        {
            if (_globalVarOwner != null)
            {
                switch (_globalVarOwnerKind)
                {
                    case ALSymbolKind.PageObject:
                        PageInformationProvider pageInformationProvider = new PageInformationProvider();
                        this.SetGlobalVariables(pageInformationProvider.GetVariables(this.Project, _globalVarOwner.Value));
                        break;
                    case ALSymbolKind.ReportObject:
                        ReportInformationProvider reportInformationProvider = new ReportInformationProvider();
                        this.SetGlobalVariables(reportInformationProvider.GetReportVariables(this.Project, _globalVarOwner.Value));
                        break;
                    case ALSymbolKind.QueryObject:
                        QueryInformationProvider queryInformationProvider = new QueryInformationProvider();
                        this.SetGlobalVariables(queryInformationProvider.GetQueryVariables(this.Project, _globalVarOwner.Value));
                        break;
                    case ALSymbolKind.XmlPortObject:
                        XmlPortInformationProvider xmlPortInformationProvider = new XmlPortInformationProvider();
                        this.SetGlobalVariables(xmlPortInformationProvider.GetXmlPortVariables(this.Project, _globalVarOwner.Value));
                        break;
                    default:
                        SetGlobalVariables(null);
                        break;
                }
            } else
                SetGlobalVariables(null);
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
                (!variable.TypeDefinition.Name.Equals("Record", StringComparison.OrdinalIgnoreCase)) ||
                (String.IsNullOrWhiteSpace(variable.TypeDefinition.Subtype.Name)))
                return null;

            var fullTableName = variable.TypeDefinition.Subtype.Name;
            var fullTableNameKey = fullTableName.ToLower();
            if (_tablesCollectionWithFields == null)
                _tablesCollectionWithFields = new Dictionary<string, List<TableFieldInformaton>>();
            if (_tablesCollectionWithFields.ContainsKey(fullTableNameKey))
                return _tablesCollectionWithFields[fullTableNameKey];

            var tableReference = new ALObjectReference(Usings, fullTableName);
            List<TableFieldInformaton> fields = this.TableInformationProvider.GetTableFields(this.Project, tableReference, false, false, true, true, true, false, null);
            _tablesCollectionWithFields.Add(fullTableNameKey, fields);
            return fields;
        }

        protected bool ToolTipDisabled(SyntaxNode node)
        {
            PropertySyntax propertySyntax = node.GetProperty("ShowCaption");
            if ((propertySyntax != null) && (propertySyntax.Value != null))
            {
                string value = propertySyntax.Value.ToString();
                return ((value != null) && (value.Equals("false", StringComparison.OrdinalIgnoreCase)));
            }
            return false;
        }

        protected bool HasToolTip(SyntaxNode node)
        {
            return ((node.HasNonEmptyProperty("ToolTip")) || (node.HasProperty("ToolTipML")));
        }

        private void SetTable(ALObjectReference alObjectReference)
        {
            this.Table = Project
                .GetAllSymbolReferences()
                .GetAllObjects<ALAppTable>(x => x.Tables)
                .FindFirst(alObjectReference);
        }


    }
}
