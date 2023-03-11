using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ObjectIdInformationProvider
    {

        public ObjectIdInformationProvider()
        {
        }

        public long GetNextObjectId(ALProject project, string objectType)
        {
            if ((project.Properties == null) || (project.Properties.Ranges == null) || (project.Properties.Ranges.Count == 0))
                return 0;

            ALSymbolKind symbolKind = ALSymbolKindExtension.FromObjectTypeName(objectType);
            if (symbolKind == ALSymbolKind.Undefined)
                return 0;

            //collect object ids
            ObjectIdRangeIdInformationCollection rangesInformationCollection = new ObjectIdRangeIdInformationCollection(project.Properties.Ranges);
            if (project.Symbols != null)
                rangesInformationCollection.AddIds(project.Symbols.GetIdsEnumerable(symbolKind));
            if (project.Dependencies != null)
            {
                foreach (ALProjectDependency dependency in project.Dependencies)
                {
                    if (dependency.Symbols != null)
                        rangesInformationCollection.AddIds(dependency.Symbols.GetIdsEnumerable(symbolKind));
                }
            }

            //find next free id
            return rangesInformationCollection.FindNextFreeId();
        }


    }
}
