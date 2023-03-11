using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class EnumInformationProvider
    {

        public List<EnumInformation> GetEnums(ALProject project)
        {
            List<EnumInformation> infoList = new List<EnumInformation>();
            foreach (ALProjectDependency dependency in project.Dependencies)
            {
                if (dependency.Symbols != null)
                    AddEnums(infoList, dependency.Symbols);
            }
            if (project.Symbols != null)
                AddEnums(infoList, project.Symbols);
            return infoList;
        }

        private void AddEnums(List<EnumInformation> infoList, ALAppSymbolReference symbols)
        {
            if (symbols.EnumTypes != null)
            {
                for (int i = 0; i < symbols.EnumTypes.Count; i++)
                    infoList.Add(new EnumInformation(symbols.EnumTypes[i]));
            }
        }

    }
}
