using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using Microsoft.Dynamics.Nav.CodeAnalysis;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class EnumInformationProvider : BaseObjectInformationProvider<ALAppEnum>
    {

        public EnumInformationProvider() : base(x => x.EnumTypes)
        {
        }

        public List<EnumInformation> GetEnums(ALProject project)
        {
            List<EnumInformation> infoList = new List<EnumInformation>();
            var objectsEnumerable = GetALAppObjectsCollection(project).GetAll();
            foreach (var obj in objectsEnumerable)
                infoList.Add(new EnumInformation(obj));
            return infoList;
        }

    }
}
