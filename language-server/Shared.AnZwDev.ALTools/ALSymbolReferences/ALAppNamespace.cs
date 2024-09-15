using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppNamespace
    {

        public string Name { get; set; }
        public string FullName { get; set; }

        public List<ALAppNamespace> Namespaces { get; set; }
        public ALAppObjectsCollection<ALAppTable> Tables { get; set; }
        public ALAppObjectsCollection<ALAppPage> Pages { get; set; }
        public ALAppObjectsCollection<ALAppReport> Reports { get; set; }
        public ALAppObjectsCollection<ALAppXmlPort> XmlPorts { get; set; }
        public ALAppObjectsCollection<ALAppQuery> Queries { get; set; }
        public ALAppObjectsCollection<ALAppCodeunit> Codeunits { get; set; } 
        public ALAppObjectsCollection<ALAppControlAddIn> ControlAddIns { get; set; }
        public ALAppObjectsCollection<ALAppPageExtension> PageExtensions { get; set; }
        public ALAppObjectsCollection<ALAppTableExtension> TableExtensions { get; set; }
        public ALAppObjectsCollection<ALAppProfile> Profiles { get; set; }
        public ALAppObjectsCollection<ALAppPageCustomization> PageCustomizations { get; set; }
        public ALAppObjectsCollection<ALAppDotNetPackage> DotNetPackages { get; set; }
        public ALAppObjectsCollection<ALAppEnum> EnumTypes { get; set; }
        public ALAppObjectsCollection<ALAppEnumExtension> EnumExtensionTypes { get; set; }
        public ALAppObjectsCollection<ALAppInterface> Interfaces { get; set; }
        public ALAppObjectsCollection<ALAppReportExtension> ReportExtensions { get; set; }
        public ALAppObjectsCollection<ALAppPermissionSet> PermissionSets { get; set; }
        public ALAppObjectsCollection<ALAppPermissionSetExtension> PermissionSetExtensions { get; set; }

        public void Process(ALAppSymbolReference symbolReference, string parentNamespace)
        {
            if (String.IsNullOrEmpty(parentNamespace))
                FullName = Name;
            else
                FullName = parentNamespace + "." + Name;

            ProcessObjectsCollection(symbolReference);
            ProcessChildNamespaces(symbolReference);
        }

        private void ProcessObjectsCollection(ALAppSymbolReference symbolReference)
        {
            ProcessObjectsCollection(symbolReference.Tables, Tables);
            ProcessObjectsCollection(symbolReference.Pages, Pages);
            ProcessObjectsCollection(symbolReference.Reports, Reports);
            ProcessObjectsCollection(symbolReference.XmlPorts, XmlPorts);
            ProcessObjectsCollection(symbolReference.Queries, Queries);
            ProcessObjectsCollection(symbolReference.Codeunits, Codeunits);
            ProcessObjectsCollection(symbolReference.ControlAddIns, ControlAddIns);
            ProcessObjectExtensionsCollection(symbolReference.PageExtensions, PageExtensions);
            ProcessObjectExtensionsCollection(symbolReference.TableExtensions, TableExtensions);
            ProcessObjectsCollection(symbolReference.Profiles, Profiles);
            ProcessObjectsCollection(symbolReference.PageCustomizations, PageCustomizations);
            ProcessObjectsCollection(symbolReference.DotNetPackages, DotNetPackages);
            ProcessObjectsCollection(symbolReference.EnumTypes, EnumTypes);
            ProcessObjectExtensionsCollection(symbolReference.EnumExtensionTypes, EnumExtensionTypes);
            ProcessObjectsCollection(symbolReference.Interfaces, Interfaces);
            ProcessObjectExtensionsCollection(symbolReference.ReportExtensions, ReportExtensions);
            ProcessObjectsCollection(symbolReference.PermissionSets, PermissionSets);
            ProcessObjectExtensionsCollection(symbolReference.PermissionSetExtensions, PermissionSetExtensions);
        }

        private void ProcessChildNamespaces(ALAppSymbolReference symbolReference)
        {
            if (Namespaces != null)
                for (int i = 0; i < Namespaces.Count; i++)
                    Namespaces[i].Process(symbolReference, FullName);
        }

        private void ProcessObjectsCollection<T>(ALAppObjectsCollection<T> allObjectsCollection, ALAppObjectsCollection<T> namespaceObjectsCollection) where T : ALAppObject
        {
            if ((namespaceObjectsCollection != null) && (namespaceObjectsCollection.Count > 0))
            {
                for (int i = 0; i < namespaceObjectsCollection.Count; i++)
                    namespaceObjectsCollection[i].NamespaceName = FullName;
                allObjectsCollection.AddRange(namespaceObjectsCollection);
            }
        }

        private void ProcessObjectExtensionsCollection<T>(ALAppObjectExtensionsCollection<T> allObjectsCollection, ALAppObjectsCollection<T> namespaceObjectsCollection) where T : ALAppObject, IALAppObjectExtension
        {
            if ((namespaceObjectsCollection != null) && (namespaceObjectsCollection.Count > 0))
            {
                for (int i = 0; i < namespaceObjectsCollection.Count; i++)
                    namespaceObjectsCollection[i].NamespaceName = FullName;
                allObjectsCollection.AddRange(namespaceObjectsCollection);
            }
        }


    }
}
