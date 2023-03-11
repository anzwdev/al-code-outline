using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableInformationProvider : BaseExtendableObjectInformationProvider<ALAppTable, ALAppTableExtension>
    {

        #region Objects list and search

        protected override MergedALAppObjectsCollection<ALAppTable> GetALAppObjectsCollection(ALProject project)
        {
            return project.AllSymbols.Tables;
        }

        protected override MergedALAppObjectExtensionsCollection<ALAppTableExtension> GetALAppObjectExtensionsCollection(ALProject project)
        {
            return project.AllSymbols.TableExtensions;
        }

        #endregion

        #region Get list of tables

        public List<TableInformation> GetTables(ALProject project)
        {
            List<TableInformation> infoList = new List<TableInformation>();
            IEnumerable<ALAppTable> tablesCollection = this.GetALAppObjectsCollection(project)?.GetObjects();
            if (tablesCollection != null)
                foreach (ALAppTable table in tablesCollection)
                    infoList.Add(new TableInformation(table));
            return infoList;
        }

        /*
        public List<TableInformation> GetTables(ALProject project)
        {
            List<TableInformation> infoList = new List<TableInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddTables(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddTables(infoList, project.Symbols);
            return infoList;
        }

        private void AddTables(List<TableInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Tables != null)
            {
                for (int i = 0; i < symbols.Tables.Count; i++)
                    infoList.Add(new TableInformation(symbols.Tables[i]));
            }
        }
        */

        #endregion

        #region Get table details

        public TableInformation GetTableInformation(ALProject project, string tableName, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters, bool includeToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            (var alTable, var fields) = GetTableFieldsWithALAppTable(project, tableName, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters, includeToolTips, toolTipsSourceDependencies);
            
            if (alTable == null)
                return null;

            TableInformation information = new TableInformation(alTable);
            information.Fields = fields;
            information.PrimaryKeys = new List<TableFieldInformaton>();

            var pk = alTable.GetPrimaryKey();
            if (pk?.FieldNames != null)
                for (int i = 0; i < pk.FieldNames.Length; i++)
                    if (pk.FieldNames[i] != null)
                    {
                        var field = information.Fields
                            .Where(p => (p.Name != null) && (pk.FieldNames[i].Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                            .FirstOrDefault();
                        if (field != null)
                            information.PrimaryKeys.Add(field);
                    }

            return information;
        }

        #endregion

        #region Find table

        protected (ALAppTable, ALProject) FindTableWithSourceProject(ALProject project, string name)
        {
            ALAppTable table = project.Symbols?.Tables?
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            if (table != null)
                return (table, project);

            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                table = dependency.Symbols?.Tables?
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
                if (table != null)
                    return (table, dependency.SourceProject);
            }
            return (null, null);
        }

        #endregion

        #region Find table extension

        protected ALAppTableExtension FindTableExtension(ALAppSymbolReference symbols, string tableName)
        {
            if ((symbols != null) && (symbols.TableExtensions != null))
                return symbols.TableExtensions
                    .Where(p => (tableName.Equals(p.TargetObject, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Get table fields

        public List<TableFieldInformaton> GetTableFields(ALProject project, string tableName, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters, bool includeToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            (var _, var fields) = GetTableFieldsWithALAppTable(project, tableName, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters, includeToolTips, toolTipsSourceDependencies);
            return fields;
        }

        private (ALAppTable, List<TableFieldInformaton>) GetTableFieldsWithALAppTable(ALProject project, string tableName, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters, bool includeToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            List<TableFieldInformaton> fields = new List<TableFieldInformaton>();

            //find table
            (ALAppTable table, ALProject tableSourceProject) = this.FindTableWithSourceProject(project, tableName);
            if (table == null)
                return (null, fields);

            //add fields from table
            this.AddFields(fields, tableSourceProject, table.Fields, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters);

            //add table extension fields
            ALAppTableExtension tableExtension = this.FindTableExtension(project.Symbols, tableName);
            if (tableExtension != null)
            {
                this.AddFields(fields, project, tableExtension.Fields, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters);
                this.UpdateFields(fields, tableExtension.FieldModifications);
            }

            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                tableExtension = FindTableExtension(dependency.Symbols, tableName);
                if (tableExtension != null)
                    this.AddFields(fields, dependency.SourceProject, tableExtension.Fields, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters);
            }

            //collect tooltips
            if (includeToolTips)
            {
                PageInformationProvider pageInformationProvider = new PageInformationProvider();
                string[] tables = { tableName };
                Dictionary<string, Dictionary<string, List<string>>> toolTips = pageInformationProvider.CollectTableFieldsToolTips(project, tables, toolTipsSourceDependencies);
                string tableKey = tableName.ToLower();
                if (toolTips.ContainsKey(tableKey))
                {
                    Dictionary<string, List<string>> fieldsToolTips = toolTips[tableKey];
                    for (int i = 0; i < fields.Count; i++)
                    {
                        string fieldKey = fields[i].Name?.ToLower();
                        if ((!String.IsNullOrWhiteSpace(fieldKey)) && (fieldsToolTips.ContainsKey(fieldKey)) && (fieldsToolTips[fieldKey].Count > 0))
                            fields[i].ToolTips = fieldsToolTips[fieldKey];
                    }
                }
            }

            //add virtual system fields
            this.AddSystemFields(fields);

            return (table, fields);
        }

        protected void AddFields(List<TableFieldInformaton> fields, ALProject project, ALAppElementsCollection<ALAppTableField> fieldReferencesList, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters)
        {
            if (fieldReferencesList != null)
            {
                foreach (ALAppTableField fieldReference in fieldReferencesList)
                {
                    ALAppTableFieldClass fieldClass = fieldReference.GetFieldClass();
                    ALAppTableFieldState fieldState = fieldReference.GetFieldState();

                    bool validState =
                        (fieldState == ALAppTableFieldState.Active) ||
                        (fieldState == ALAppTableFieldState.ObsoletePending) ||
                        ((fieldState == ALAppTableFieldState.ObsoleteRemoved) && (includeObsolete)) ||
                        ((fieldState == ALAppTableFieldState.Disabled) && (includeDisabled));
                    bool validClass =
                        ((fieldClass == ALAppTableFieldClass.Normal) && (includeNormal)) ||
                        ((fieldClass == ALAppTableFieldClass.FlowField) && (includeFlowFields)) ||
                        ((fieldClass == ALAppTableFieldClass.FlowFilter) && (includeFlowFilters));

                    if (validState && validClass)
                        fields.Add(new TableFieldInformaton(project, fieldReference));
                }
            }
        }
        
        protected void UpdateFields(List<TableFieldInformaton> fields, ALAppElementsCollection<ALAppTableField> fieldModificationsCollection)
        {
            if (fieldModificationsCollection != null)
            {
                foreach (ALAppTableField fieldModification in fieldModificationsCollection)
                {
                    TableFieldInformaton field = fields
                        .Where(p => ((p.Name != null) && (p.Name.Equals(fieldModification.Name, StringComparison.CurrentCultureIgnoreCase))))
                        .FirstOrDefault();
                    if (field != null)
                        field.UpdateProperties(fieldModification.Properties);
                }
            }

        }

        protected void AddSystemFields(List<TableFieldInformaton> fields)
        {
            fields.Add(new TableFieldInformaton(2000000000, "SystemId", "Guid"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemCreatedAt", "DateTime"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemCreatedBy", "Guid"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemModifiedAt", "DateTime"));
            fields.Add(new TableFieldInformaton(2000000000, "SystemModifiedBy", "Guid"));
        }

        #endregion

    }
}
