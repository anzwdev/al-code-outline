using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class QueryInformationProvider : BaseObjectInformationProvider<ALAppQuery>
    {

        public QueryInformationProvider() : base(x => x.Queries)
        {
        }

        #region Get list of queries

        public List<QueryInformation> GetQueries(ALProject project)
        {
            var infoList = new List<QueryInformation>();
            var objectsCollection = GetALAppObjectsCollection(project);
            var objectsEnumerable = objectsCollection.GetAll();
            foreach (var obj in objectsEnumerable)
                infoList.Add(new QueryInformation(obj));
            return infoList;
        }

        #endregion

        protected ALAppQuery FindQuery(ALProject project, ALObjectReference objectReference)
        {
            return GetALAppObjectsCollection(project)
                .FindFirst(objectReference);
        }

        #region Get query data item details

        public QueryDataItemInformation GetQueryDataItemInformationDetails(ALProject project, ALObjectReference queryReference, string dataItemName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppQuery query = this.FindQuery(project, queryReference);
            if ((query == null) || (query.Elements == null))
                return null;
            ALAppQueryDataItem queryDataItem = query.FindDataItem(dataItemName);
            if (queryDataItem == null)
                return null;

            QueryDataItemInformation queryDataItemInformation = new QueryDataItemInformation(queryDataItem);
            var tableReference = new ALObjectReference(query.Usings, queryDataItem.RelatedTable);
            if ((!tableReference.IsEmpty()) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, tableReference, false, false, true, true, false, false, null);

                Dictionary<string, TableFieldInformaton> availableTableFieldsDict = allTableFieldsList.ToDictionary();
                List<TableFieldInformaton> reportDataItemFields = new List<TableFieldInformaton>();

                if (queryDataItem.Columns != null)
                    this.CollectQueryDataItemFields(queryDataItemInformation.Name, queryDataItem.Columns, availableTableFieldsDict, reportDataItemFields);

                if (getExistingFields)
                    queryDataItemInformation.ExistingTableFields = reportDataItemFields;
                if (getAvailableFields)
                    queryDataItemInformation.AvailableTableFields = availableTableFieldsDict.Values.ToList();
            }

            return queryDataItemInformation;
        }

        protected void CollectQueryDataItemFields(string dataItemName, ALAppSymbolsCollection<ALAppQueryColumn> columnsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> reportDataItemFields)
        {
            for (int i = 0; i < columnsList.Count; i++)
            {
                ALAppQueryColumn queryColumn = columnsList[i];
                if (!String.IsNullOrWhiteSpace(queryColumn.SourceColumn))
                {
                    string sourceExpression = queryColumn.SourceColumn.ToLower();
                    if ((!String.IsNullOrWhiteSpace(sourceExpression)) && (availableTableFieldsDict.ContainsKey(sourceExpression)))
                    {
                        reportDataItemFields.Add(availableTableFieldsDict[sourceExpression]);
                        availableTableFieldsDict.Remove(sourceExpression);
                    }
                }
            }
        }

        #endregion

        #region Query variables

        public ALAppSymbolsCollection<ALAppVariable> GetQueryVariables(ALProject project, ALObjectReference objectReference)
        {
            ALAppQuery query = this.FindQuery(project, objectReference);
            if ((query != null) && (query.Variables != null) && (query.Variables.Count > 0))
                return query.Variables;
            return null;
        }

        #endregion

    }
}
