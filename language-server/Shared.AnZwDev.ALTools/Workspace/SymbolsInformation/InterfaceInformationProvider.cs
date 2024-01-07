using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class InterfaceInformationProvider : BaseObjectInformationProvider<ALAppInterface>
    {

        public InterfaceInformationProvider() : base(x => x.Interfaces)
        { 
        }

        public List<InterfaceInformation> GetInterfaces(ALProject project)
        {
            List<InterfaceInformation> infoList = new List<InterfaceInformation>();
            IEnumerable<ALAppInterface> objectsCollection = GetALAppObjectsCollection(project);
            foreach (ALAppInterface item in objectsCollection)
                infoList.Add(new InterfaceInformation(item));
            return infoList;
        }

    }
}
