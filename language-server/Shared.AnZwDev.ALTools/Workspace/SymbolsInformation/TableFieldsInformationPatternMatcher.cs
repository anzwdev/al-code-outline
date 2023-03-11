using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.Workspace.SymbolsInformation.Sorting;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableFieldsInformationPatternMatcher
    {

        protected TableInformationProvider TableInformationProvider { get; } = new TableInformationProvider();

        public TableFieldsInformationPatternMatcher()
        {
        }

        public List<TableFieldInformaton> Match(ALProject project, string tableName, bool includePrimaryKeys, List<string> fieldNamePatternsList, bool includeNormal, bool includeFlowFields, bool includeFlowFilters)
        {
            var tableInformation = TableInformationProvider.GetTableInformation(project, tableName, false, false, includeNormal, includeFlowFields, includeFlowFilters, false, null);

            List<TableFieldInformaton> collectedFields = new List<TableFieldInformaton>();
            if (tableInformation != null)
            {
                tableInformation.Fields.Sort(new SymbolWithIdInformationIdComparer());

                HashSet<string> collectedFieldNames = new HashSet<string>();

                if (includePrimaryKeys)
                    for (int i = 0; i < tableInformation.PrimaryKeys.Count; i++)
                        CollectField(tableInformation.PrimaryKeys[i], collectedFieldNames, collectedFields);

                if (fieldNamePatternsList != null)
                    for (int i = 0; i < fieldNamePatternsList.Count; i++)
                        if (!String.IsNullOrWhiteSpace(fieldNamePatternsList[i]))
                            MatchField(fieldNamePatternsList[i], tableInformation.Fields, collectedFieldNames, collectedFields);
            }

            return collectedFields;
        }

        private void CollectField(TableFieldInformaton field, HashSet<string> collectedFieldNames, List<TableFieldInformaton> collectedFields)
        {
            collectedFieldNames.Add(field.Name);
            collectedFields.Add(field);
        }

        private void MatchField(string pattern, List<TableFieldInformaton> tableFields, HashSet<string> collectedFieldNames, List<TableFieldInformaton> collectedFields)
        {
            var matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher(StringComparison.CurrentCultureIgnoreCase);
            matcher.AddInclude(pattern);

            for (int i = 0; i < tableFields.Count; i++)
            {
                var fieldName = tableFields[i].Name;
                if ((!String.IsNullOrWhiteSpace(fieldName)) && (!collectedFieldNames.Contains(fieldName)))
                {
                    var matchResult = matcher.Match(fieldName);
                    if ((matchResult != null) && (matchResult.HasMatches))
                    {
                        CollectField(tableFields[i], collectedFieldNames, collectedFields);
                        return;
                    }
                }
            }
        }

    }
}
