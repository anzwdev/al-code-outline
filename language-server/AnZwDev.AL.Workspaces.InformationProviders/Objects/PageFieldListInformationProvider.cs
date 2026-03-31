using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.EnumerableExtensions;
using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Objects
{
    internal class PageFieldListInformationProvider
    {

        public static IEnumerable<PageControlSymbol> GetPageFields(Project project, ObjectIdentifier objectIdentifier, HashSet<string>? appIdFilter = null)
        {
            return GetPageFields(project, project.Symbols.Pages.FindFirst(objectIdentifier, appIdFilter), appIdFilter);
        }

        public static IEnumerable<PageControlSymbol> GetPageFields(Project project, ObjectReference objectReference, HashSet<string>? appIdFilter = null)
        {
            return GetPageFields(project, project.Symbols.Pages.FindFirst(objectReference, appIdFilter), appIdFilter);
        }

        public static IEnumerable<PageControlSymbol> GetPageFields(Project project, PageSymbol? page, HashSet<string>? appIdFilter = null)
        {
            if (page != null)
            {
                var kindFilter = new HashSet<PageControlKind>() { PageControlKind.Field };
                var controlsEnumerable = page.GetAllControls(kindFilter);
                if (controlsEnumerable != null)
                    foreach (var control in controlsEnumerable)
                        yield return control;

                var extensionsEnumerable = project.Symbols.PageExtensions.FindExtensions(page.Identifier, appIdFilter);
                if (extensionsEnumerable != null)
                    foreach (var extension in extensionsEnumerable)
                    {
                        controlsEnumerable = extension.GetAllControls(kindFilter);
                        if (controlsEnumerable != null)
                            foreach (var control in controlsEnumerable)
                                yield return control;
                    }
            }
        }

        public static PageTableFields? GetPageTableFields(Project project, ObjectReference pageReference, HashSet<FieldClass>? fieldClassFilter = null)
        {
            var page = project.Symbols.Pages.FindFirst(pageReference);
            if (page != null)
                return GetPageTableFields(project, page, fieldClassFilter);
            return null;
        }

        public static PageTableFields? GetPageTableFields(Project project, ObjectIdentifier pageIdentifier, HashSet<FieldClass>? fieldClassFilter = null)
        {
            var page = project.Symbols.Pages.FindFirst(pageIdentifier);
            if (page != null)
                return GetPageTableFields(project, page, fieldClassFilter);
            return null;
        }

        public static PageTableFields? GetPageTableFields(Project project, PageSymbol pageSymbol, HashSet<FieldClass>? fieldClassFilter = null)
        {
            if (pageSymbol.SourceTable.IsEmpty())
                return null;

            var tableSymbol = project.Symbols.Tables.FindFirst(pageSymbol.SourceTable);
            if (tableSymbol == null)
                return null;

            //initialize
            var pageTableFields = new PageTableFields(pageSymbol.Identifier, tableSymbol.Identifier);

            //get page fields
            var pageFieldsNames = GetPageFieldsNames(project, pageSymbol);

            //get table fields
            var tableFieldsEnumerable = TableFieldListInformationProvider.GetTableFields(project, tableSymbol, fieldClassFilter);
            foreach (var tableField in tableFieldsEnumerable)
                if (pageFieldsNames.Contains(tableField.Name))
                    pageTableFields.AddedFields.Add(tableField);
                else
                    pageTableFields.AvailableFields.Add(tableField);

            return pageTableFields;
        }

        private static HashSet<string> GetPageFieldsNames(Project project, PageSymbol pageSymbol)
        {
            var pageFieldsNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var controlsEnumerable = GetPageFields(project, pageSymbol);
            if (controlsEnumerable != null)
                foreach (var control in controlsEnumerable)
                {
                    var tableFieldNameExpression = ALSymbolExpressionParser.ParseTableFieldExpressionReference(control.Properties?.SourceExpression);
                    if ((!String.IsNullOrEmpty(tableFieldNameExpression)) && (!pageFieldsNames.Contains(tableFieldNameExpression)))
                        pageFieldsNames.Add(tableFieldNameExpression);
                }
            return pageFieldsNames;
        }

    }
}
