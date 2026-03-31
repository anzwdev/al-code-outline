using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Objects
{
    public class TableFieldListInformationProvider
    {

        public static IEnumerable<TableFieldSymbol> GetTableFields(Project project, ObjectIdentifier objectIdentifier, HashSet<FieldClass>? fieldClassFilter)
        {
            return GetTableFields(project, project.Symbols.Tables.FindFirst(objectIdentifier), fieldClassFilter);
        }

        public static IEnumerable<TableFieldSymbol> GetTableFields(Project project, ObjectReference objectReference, HashSet<FieldClass>? fieldClassFilter)
        {
            return GetTableFields(project, project.Symbols.Tables.FindFirst(objectReference), fieldClassFilter);
        }

        public static IEnumerable<TableFieldSymbol> GetTableFields(Project project, TableSymbol? table, HashSet<FieldClass>? fieldClassFilter)
        {
            if (table != null)
            {
                for (int i = 0; i < table.Fields.Count; i++)
                    if (FieldFilterMatch(table.Fields[i], fieldClassFilter))
                        yield return table.Fields[i];

                var extensionsEnumerable = project.Symbols.TableExtensions.FindExtensions(table.Identifier);
                if (extensionsEnumerable != null)
                    foreach (var extension in extensionsEnumerable)
                        if (extension.Fields != null)
                            for (int i = 0; i < extension.Fields.Count; i++)
                                if (FieldFilterMatch(extension.Fields[i], fieldClassFilter))
                                    yield return extension.Fields[i];
            }
        }

        private static bool FieldFilterMatch(TableFieldSymbol field, HashSet<FieldClass>? fieldClassFilter)
        {
            return
                (fieldClassFilter == null) ||
                (field.Properties == null) ||
                (fieldClassFilter.Contains(field.Properties.FieldClass));
        }

    }
}
