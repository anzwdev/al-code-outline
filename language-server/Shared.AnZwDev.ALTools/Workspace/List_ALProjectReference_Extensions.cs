using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public static class List_ALProjectReference_Extensions
    {

        public static ALProjectReference FindProjectReference(this List<ALProjectReference> projectReferences, string id, string name, string publisher)
        {
            if (name == null)
                name = "";
            if (publisher == null)
                publisher = "";
            bool checkId = !String.IsNullOrWhiteSpace(id);

            if (projectReferences != null)
            {
                for (int i = 0; i < projectReferences.Count; i++)
                {
                    ALProjectReference projectRef = projectReferences[i];
                    if (
                        ((checkId) && (id.Equals(projectRef.Id, StringComparison.CurrentCultureIgnoreCase))) ||
                        (
                            ((!checkId) || (String.IsNullOrWhiteSpace(projectRef.Id))) &&
                            (name.Equals(projectRef.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                            (publisher.Equals(projectRef.Publisher, StringComparison.CurrentCultureIgnoreCase))
                        ))
                        return projectRef;
                }
            }

            return null;
        }

    }
}
