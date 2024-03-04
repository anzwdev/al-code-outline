using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

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
            var objectsCollection = GetALAppObjectsCollection(project);
            var objectsEnumerable = objectsCollection.GetAll();
            foreach (ALAppInterface item in objectsEnumerable)
                infoList.Add(new InterfaceInformation(item));
            return infoList;
        }

    }
}
