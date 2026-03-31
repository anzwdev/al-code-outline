using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.ObjectIds
{
    public static class ObjectIdInformationProvider
    {


        public static int GetNextFreeId(Project project, ObjectKind kind)
        {
            var projectSymbols = project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols();

            if (projectSymbols != null)
            {
                //collect all IDs
                HashSet<int> usedIds = new HashSet<int>();
                var objectsEnumerable = projectSymbols.AllObjects.Filter(kind);
                foreach (var obj in objectsEnumerable)
                    if (!usedIds.Contains(obj.Identifier.Id))
                        usedIds.Add(obj.Identifier.Id);

                //check ranges
                for (int rangeIndex = 0; rangeIndex < projectSymbols.Metadata.IdRanges.Count; rangeIndex++)
                {
                    var range = projectSymbols.Metadata.IdRanges[rangeIndex];
                    for (int id = range.From; id <= range.To; id++)
                        if (!usedIds.Contains(id))
                            return id;
                }
            }

            return -1;
        }


    }
}
