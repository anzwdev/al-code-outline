using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class QueryInformationProvider
    {

        public QueryInformationProvider()
        {
        }

        #region Get list of queries

        public List<QueryInformation> GetQueries(ALProject project)
        {
            List<QueryInformation> infoList = new List<QueryInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddQueries(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddQueries(infoList, project.Symbols);
            return infoList;
        }

        private void AddQueries(List<QueryInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.Queries != null)
            {
                for (int i = 0; i < symbols.Reports.Count; i++)
                    infoList.Add(new QueryInformation(symbols.Queries[i]));
            }
        }

        #endregion

        #region Find query

        protected ALAppQuery FindQuery(ALProject project, string name)
        {
            ALAppQuery query = FindQuery(project.Symbols, name);
            if (query != null)
                return query;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                query = FindQuery(dependency.Symbols, name);
                if (query != null)
                    return query;
            }
            return null;
        }

        protected ALAppQuery FindQuery(ALProject project, int id)
        {
            ALAppQuery query = FindQuery(project.Symbols, id);
            if (query != null)
                return query;
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                query = FindQuery(dependency.Symbols, id);
                if (query != null)
                    return query;
            }
            return null;
        }

        protected ALAppQuery FindQuery(ALAppSymbolReference symbols, string name)
        {
            if ((symbols != null) && (symbols.Queries != null))
                return symbols.Queries
                    .Where(p => (name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
            return null;
        }

        protected ALAppQuery FindQuery(ALAppSymbolReference symbols, int id)
        {
            if ((symbols != null) && (symbols.Queries != null))
                return symbols.Queries
                    .Where(p => (p.Id == id))
                    .FirstOrDefault();
            return null;
        }

        #endregion

        #region Get query data item details

        public QueryDataItemInformation GetQueryDataItemInformationDetails(ALProject project, string queryName, string dataItemName, bool getExistingFields, bool getAvailableFields)
        {
            ALAppQuery query = this.FindQuery(project, queryName);
            if ((query == null) || (query.Elements == null))
                return null;
            ALAppQueryDataItem queryDataItem = query.FindDataItem(dataItemName);
            if (queryDataItem == null)
                return null;

            QueryDataItemInformation queryDataItemInformation = new QueryDataItemInformation(queryDataItem);
            if ((!String.IsNullOrWhiteSpace(queryDataItem.RelatedTable)) && (getExistingFields || getAvailableFields))
            {
                TableInformationProvider tableInformationProvider = new TableInformationProvider();
                List<TableFieldInformaton> allTableFieldsList = tableInformationProvider.GetTableFields(project, queryDataItem.RelatedTable, false, false, true, true, false, false, null);

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

        protected void CollectQueryDataItemFields(string dataItemName, ALAppElementsCollection<ALAppQueryColumn> columnsList, Dictionary<string, TableFieldInformaton> availableTableFieldsDict, List<TableFieldInformaton> reportDataItemFields)
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

        public List<ALAppVariable> GetQueryVariables(ALProject project, string name)
        {
            ALAppQuery query = this.FindQuery(project, name);
            if ((query != null) && (query.Variables != null) && (query.Variables.Count > 0))
                return query.Variables;
            return null;
        }

        #endregion

    }
}
