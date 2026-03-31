using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.AL.Workspaces.InformationProviders.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.ToolTips
{
    public class ToolTipsInformationProvider
    {

        public static TableToolTips? GetTableToolTips(Project project, ObjectIdentifier objectIdentifier, bool includePages, HashSet<string>? pagesAppIdFilter = null)
        {
            var table = project.Symbols.Tables.FindFirst(objectIdentifier);
            if (table == null)
                return null;
            return GetTableToolTips(project, table, includePages, pagesAppIdFilter);
        }

        public static TableToolTips? GetTableToolTips(Project project, ObjectReference objectReference, bool includePages, HashSet<string>? pagesAppIdFilter = null)
        {
            var table = project.Symbols.Tables.FindFirst(objectReference);
            if (table == null)
                return null;
            return GetTableToolTips(project, table, includePages, pagesAppIdFilter);
        }

        public static TableToolTips GetTableToolTips(Project project, TableSymbol table, bool includePages, HashSet<string>? pagesAppIdFilter = null)
        {
            var tableToolTips = new TableToolTips()
            {
                Identifier = table.Identifier
            };

            //collect table fields
            CollectTableFiels(tableToolTips, project, table);

            //collect page fields tooltips
            if (includePages)
                CollectPageFielsToolTips(tableToolTips, project, table, pagesAppIdFilter);

            return tableToolTips;
        }

        private static void CollectTableFiels(TableToolTips tableToolTips, Project project, TableSymbol table)
        {
            var tableFields = TableFieldListInformationProvider.GetTableFields(project, table, null);
            foreach (var field in tableFields)
            {
                if (!tableToolTips.Fields.ContainsKey(field.Name))
                {
                    var fieldToolTips = new FieldToolTips()
                    {
                        Field = field,
                    };

                    if ((field.Properties != null) && (field.Properties.Contains(PropertyKind.ToolTip)))
                    {
                        fieldToolTips.ToolTips.Add(new ToolTip()
                        {
                            SourceObjectIdentifier = table.Identifier,
                            Value = field.Properties.ToolTip
                        });
                    }

                    tableToolTips.Fields.Add(field.Name, fieldToolTips);
                }

            }
        }

        private static void CollectPageFielsToolTips(TableToolTips tableToolTips, Project project, TableSymbol table, HashSet<string>? appIdFilter = null)
        {
            //get all pages for the table
            var pagesEnumerable = PageListInformationProvider.GetPagesForTable(project, table.Identifier);
            if (pagesEnumerable != null)
            {
                foreach (var page in pagesEnumerable)
                {
                    var fieldControlsEnumerable = PageFieldListInformationProvider.GetPageFields(project, page, appIdFilter);
                    foreach (var fieldControl in fieldControlsEnumerable)
                    {
                        var tableFieldNameExpression = ALSymbolExpressionParser.ParseTableFieldExpressionReference(fieldControl.Properties?.SourceExpression);
                        if ((!String.IsNullOrWhiteSpace(tableFieldNameExpression)) && (tableToolTips.Fields.ContainsKey(tableFieldNameExpression)))
                        {
                            var fieldToolTips = tableToolTips.Fields[tableFieldNameExpression];

                            if ((fieldControl.Properties != null) && (fieldControl.Properties.Contains(PropertyKind.ToolTip)))
                            {
                                fieldToolTips.ToolTips.Add(new ToolTip()
                                {
                                    SourceObjectIdentifier = page.Identifier,
                                    Value = fieldControl.Properties.ToolTip
                                });
                            }
                        }
                    }
                }
            }
        }

    }
}
