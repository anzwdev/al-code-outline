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
            symbolReference.Tables = ProcessObjectsCollection(symbolReference.Tables, Tables);
            symbolReference.Pages = ProcessObjectsCollection(symbolReference.Pages, Pages);
            symbolReference.Reports = ProcessObjectsCollection(symbolReference.Reports, Reports);
            symbolReference.XmlPorts = ProcessObjectsCollection(symbolReference.XmlPorts, XmlPorts);
            symbolReference.Queries = ProcessObjectsCollection(symbolReference.Queries, Queries);
            symbolReference.Codeunits = ProcessObjectsCollection(symbolReference.Codeunits, Codeunits);
            symbolReference.ControlAddIns = ProcessObjectsCollection(symbolReference.ControlAddIns, ControlAddIns);
            symbolReference.PageExtensions = ProcessObjectExtensionsCollection(symbolReference.PageExtensions, PageExtensions);
            symbolReference.TableExtensions = ProcessObjectExtensionsCollection(symbolReference.TableExtensions, TableExtensions);
            symbolReference.Profiles = ProcessObjectsCollection(symbolReference.Profiles, Profiles);
            symbolReference.PageCustomizations = ProcessObjectsCollection(symbolReference.PageCustomizations, PageCustomizations);
            symbolReference.DotNetPackages = ProcessObjectsCollection(symbolReference.DotNetPackages, DotNetPackages);
            symbolReference.EnumTypes = ProcessObjectsCollection(symbolReference.EnumTypes, EnumTypes);
            symbolReference.EnumExtensionTypes = ProcessObjectExtensionsCollection(symbolReference.EnumExtensionTypes, EnumExtensionTypes);
            symbolReference.Interfaces = ProcessObjectsCollection(symbolReference.Interfaces, Interfaces);
            symbolReference.ReportExtensions = ProcessObjectExtensionsCollection(symbolReference.ReportExtensions, ReportExtensions);
            symbolReference.PermissionSets = ProcessObjectsCollection(symbolReference.PermissionSets, PermissionSets);
            symbolReference.PermissionSetExtensions = ProcessObjectExtensionsCollection(symbolReference.PermissionSetExtensions, PermissionSetExtensions);
        }

        private void ProcessChildNamespaces(ALAppSymbolReference symbolReference)
        {
            if (Namespaces != null)
                for (int i = 0; i < Namespaces.Count; i++)
                    Namespaces[i].Process(symbolReference, FullName);
        }

        private ALAppObjectsCollection<T> ProcessObjectsCollection<T>(ALAppObjectsCollection<T> allObjectsCollection, ALAppObjectsCollection<T> namespaceObjectsCollection) where T : ALAppObject
        {
            if ((namespaceObjectsCollection != null) && (namespaceObjectsCollection.Count > 0))
            {
                if (allObjectsCollection == null)
                    allObjectsCollection = new ALAppObjectsCollection<T>();

                for (int i = 0; i < namespaceObjectsCollection.Count; i++)
                    namespaceObjectsCollection[i].NamespaceName = FullName;
                allObjectsCollection.AddRange(namespaceObjectsCollection);
            }

            return allObjectsCollection;
        }

        private ALAppObjectExtensionsCollection<T> ProcessObjectExtensionsCollection<T>(ALAppObjectExtensionsCollection<T> allObjectsCollection, ALAppObjectsCollection<T> namespaceObjectsCollection) where T : ALAppObject, IALAppObjectExtension
        {
            if ((namespaceObjectsCollection != null) && (namespaceObjectsCollection.Count > 0))
            {
                if (allObjectsCollection == null)
                    allObjectsCollection = new ALAppObjectExtensionsCollection<T>();

                for (int i = 0; i < namespaceObjectsCollection.Count; i++)
                    namespaceObjectsCollection[i].NamespaceName = FullName;
                allObjectsCollection.AddRange(namespaceObjectsCollection);
            }

            return allObjectsCollection;
        }


    }
}
