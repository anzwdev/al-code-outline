using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.Objects
{
    public class PageListInformationProvider
    {

        public static IEnumerable<PageSymbol> GetPagesForTable(Project project, ObjectIdentifier tableIdentifier, HashSet<string>? appIdFilter = null)
        {
            var pagesEnumerable = project.Symbols.Pages.Filter(appIdFilter);
            if (pagesEnumerable != null)
                foreach (var page in pagesEnumerable)
                    if (page.SourceTable.References(tableIdentifier))
                        yield return page;
        }

    }
}
