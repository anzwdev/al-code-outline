using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public static class List_NavxModuleReference_Extensions
    {

        public static NavxModuleReference FindModuleReference(this NavxModuleReference[] modules, string id, string name, string publisher)
        {
            if (name == null)
                name = "";
            if (publisher == null)
                publisher = "";
            bool checkId = !String.IsNullOrWhiteSpace(id);

            if (modules != null)
            {
                for (int i=0; i<modules.Length; i++)
                {
                    NavxModuleReference moduleRef = modules[i];
                    if (
                        ((checkId) && (id.Equals(moduleRef.Id, StringComparison.CurrentCultureIgnoreCase))) ||
                        (
                            ((!checkId) || (String.IsNullOrWhiteSpace(moduleRef.Id))) &&
                            (name.Equals(moduleRef.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                            (publisher.Equals(moduleRef.Publisher, StringComparison.CurrentCultureIgnoreCase))
                        ))
                        return moduleRef;
                }
            }

            return null;
        }


    }
}
