using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableInformationProvider : BaseExtendableObjectInformationProvider<ALAppTable, ALAppTableExtension>
    {

        public TableInformationProvider() : base(x => x.Tables, x => x.TableExtensions)
        {
        }

        #region Get list of tables

        public List<TableInformation> GetTables(ALProject project)
        {
            List<TableInformation> infoList = new List<TableInformation>();
            var objectsCollection = GetALAppObjectsCollection(project);
            var objectsEnumerable = objectsCollection.GetAll();
            foreach (var table in objectsEnumerable)
                infoList.Add(new TableInformation(table));
            return infoList;
        }

        #endregion

        #region Get table details

        public TableInformation GetTableInformation(ALProject project, ALObjectReference tableReference, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters, bool includeToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            (var alTable, var fields) = GetTableFieldsWithALAppTable(project, tableReference, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters, includeToolTips, toolTipsSourceDependencies);
            
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
                            .Where(p => (p.Name != null) && (pk.FieldNames[i].Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
                            .FirstOrDefault();
                        if (field != null)
                            information.PrimaryKeys.Add(field);
                    }

            return information;
        }

        #endregion

        #region Find table

        protected (ALAppTable, ALProject) FindTableWithSourceProject(ALProject project, ALObjectReference tableReference)
        {

            ALAppTable table = project.Symbols?.Tables?.FindFirst(tableReference);
            if (table != null)
                return (table, project);

            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                table = dependency.Symbols?.Tables?.FindFirst(tableReference);
                if (table != null)
                    return (table, dependency.SourceProject);
            }
            return (null, null);
        }

        #endregion

        #region Get table fields

        public List<TableFieldInformaton> GetTableFields(ALProject project, ALObjectReference tableReference, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters, bool includeToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            (var _, var fields) = GetTableFieldsWithALAppTable(project, tableReference, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters, includeToolTips, toolTipsSourceDependencies);
            return fields;
        }

        private (ALAppTable, List<TableFieldInformaton>) GetTableFieldsWithALAppTable(ALProject project, ALObjectReference tableReference, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters, bool includeToolTips, IEnumerable<string> toolTipsSourceDependencies)
        {
            List<TableFieldInformaton> fields = new List<TableFieldInformaton>();

            //find table
            (ALAppTable table, ALProject tableSourceProject) = this.FindTableWithSourceProject(project, tableReference);
            if (table == null)
                return (null, fields);
            var tableIdentifier = table.GetIdentifier();

            //add fields from table
            this.AddFields(fields, tableSourceProject, table.Fields, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters);

            //add table extension fields
            var tableExtensionsEnumerable = project
                .SymbolsWithDependencies
                .TableExtensions
                .GetObjectExtensions(tableIdentifier);
            if (tableExtensionsEnumerable != null)
                foreach (var tableExtension in tableExtensionsEnumerable)
                {
                    this.AddFields(fields, tableSourceProject, tableExtension.Fields, includeDisabled, includeObsolete, includeNormal, includeFlowFields, includeFlowFilters);
                    this.UpdateFields(fields, tableExtension.FieldModifications);
                }
                
            //collect tooltips
            if (includeToolTips)
            {
                PageInformationProvider pageInformationProvider = new PageInformationProvider();
                ALObjectIdentifier[] tables = { tableIdentifier };
                Dictionary<int, Dictionary<string, List<LabelInformation>>> toolTips = pageInformationProvider.CollectTableFieldsToolTips(project, tables, toolTipsSourceDependencies);
                if (toolTips.ContainsKey(tableIdentifier.Id))
                {
                    Dictionary<string, List<LabelInformation>> fieldsToolTips = toolTips[tableIdentifier.Id];
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

        protected void AddFields(List<TableFieldInformaton> fields, ALProject project, ALAppSymbolsCollection<ALAppTableField> fieldReferencesList, bool includeDisabled, bool includeObsolete, bool includeNormal, bool includeFlowFields, bool includeFlowFilters)
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
        
        protected void UpdateFields(List<TableFieldInformaton> fields, ALAppSymbolsCollection<ALAppTableField> fieldModificationsCollection)
        {
            if (fieldModificationsCollection != null)
            {
                foreach (ALAppTableField fieldModification in fieldModificationsCollection)
                {
                    TableFieldInformaton field = fields
                        .Where(p => ((p.Name != null) && (p.Name.Equals(fieldModification.Name, StringComparison.OrdinalIgnoreCase))))
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
