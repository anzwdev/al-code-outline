using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal abstract class AppObjectsContainerSymbol
    {

        [JsonPropertyName("Namespaces")]
        public AppNamespaceSymbol[]? Namespaces { get; set; }

        [JsonPropertyName("Tables")]
        public AppTableSymbol[]? Tables { get; set; }

        [JsonPropertyName("Codeunits")]
        public AppCodeunitSymbol[]? Codeunits { get; set; }

        [JsonPropertyName("Pages")]
        public AppPageSymbol[]? Pages { get; set; }

        [JsonPropertyName("PageExtensions")]
        public AppPageExtensionSymbol[]? PageExtensions { get; set; }

        [JsonPropertyName("Reports")]
        public AppReportSymbol[]? Reports { get; set; }

        [JsonPropertyName("ReportExtensions")]
        public AppReportExtensionSymbol[]? ReportExtensions { get; set; }

        [JsonPropertyName("XmlPorts")]
        public AppXmlPortSymbol[]? XmlPorts { get; set; }

        [JsonPropertyName("Queries")]
        public AppQuerySymbol[]? Queries { get; set; }

        [JsonPropertyName("ControlAddIns")]
        public AppControlAddInSymbol[]? ControlAddIns { get; set; }

        [JsonPropertyName("EnumTypes")]
        public AppEnumTypeSymbol[]? EnumTypes { get; set; }

        [JsonPropertyName("DotNetPackages")]
        public AppDotNetPackageSymbol[]? DotNetPackages { get; set; }

        [JsonPropertyName("Interfaces")]
        public AppInterfaceSymbol[]? Interfaces { get; set; }

        [JsonPropertyName("PermissionSets")]
        public AppPermissionSetSymbol[]? PermissionSets { get; set; }

        [JsonPropertyName("PermissionSetExtensions")]
        public AppPermissionSetExtensionSymbol[]? PermissionSetExtensions { get; set; }

        [JsonPropertyName("EnumExtensionTypes")]
        public AppEnumExtensionTypeSymbol[]? EnumExtensionTypes { get; set; }

        [JsonPropertyName("TableExtensions")]
        public AppTableExtensionSymbol[]? TableExtensions { get; set; }

        [JsonPropertyName("Profiles")]
        public AppProfileSymbol[]? Profiles { get; set; }

        [JsonPropertyName("ProfileExtensions")]
        public AppProfileExtensionSymbol[]? ProfileExtensions { get; set; }

        [JsonPropertyName("PageCustomizations")]
        public AppPageCustomizationSymbol[]? PageCustomizations { get; set; }


        public void ProcessCollections(ApplicationSymbol applicationSymbol, string? parentNamespace)
        {
            var ns = GetNamespace(parentNamespace);

            ProcessObjectsCollection(ns, Tables, applicationSymbol.Tables);
            ProcessObjectsCollection(ns, Codeunits, applicationSymbol.Codeunits);
            ProcessObjectsCollection(ns, Pages, applicationSymbol.Pages);
            ProcessObjectsCollection(ns, PageExtensions, applicationSymbol.PageExtensions);
            ProcessObjectsCollection(ns, Reports, applicationSymbol.Reports);
            ProcessObjectsCollection(ns, ReportExtensions, applicationSymbol.ReportExtensions);
            ProcessObjectsCollection(ns, XmlPorts, applicationSymbol.XmlPorts);
            ProcessObjectsCollection(ns, Queries, applicationSymbol.Queries);
            ProcessObjectsCollection(ns, ControlAddIns, applicationSymbol.ControlAddIns);
            ProcessObjectsCollection(ns, EnumTypes, applicationSymbol.EnumTypes);
            ProcessObjectsCollection(ns, DotNetPackages, applicationSymbol.DotNetPackages);
            ProcessObjectsCollection(ns, Interfaces, applicationSymbol.Interfaces);
            ProcessObjectsCollection(ns, PermissionSets, applicationSymbol.PermissionSets);
            ProcessObjectsCollection(ns, PermissionSetExtensions, applicationSymbol.PermissionSetExtensions);
            ProcessObjectsCollection(ns, EnumExtensionTypes, applicationSymbol.EnumExtensionTypes);
            ProcessObjectsCollection(ns, TableExtensions, applicationSymbol.TableExtensions);
            ProcessObjectsCollection(ns, Profiles, applicationSymbol.Profiles);
            ProcessObjectsCollection(ns, ProfileExtensions, applicationSymbol.ProfileExtensions);
            ProcessObjectsCollection(ns, PageCustomizations, applicationSymbol.PageCustomizations);

            ProcessNamespaces(ns, applicationSymbol);
        }

        private void ProcessObjectsCollection<TAppSymbol, TSymbol>(string? ns, TAppSymbol[]? sourceCollection, ObjectSymbolsCollection<TSymbol> targetCollection) where TAppSymbol : AppObjectSymbol<TSymbol> where TSymbol : ObjectSymbol
        {
            if (sourceCollection != null)
                for (int i = 0; i < sourceCollection.Length; i++)
                    targetCollection.Add(sourceCollection[i].CreateSymbol(ns));
        }

        private void ProcessNamespaces(string? ns, ApplicationSymbol applicationSymbol)
        {
            if (Namespaces != null)
                for (int i = 0; i < Namespaces.Length; i++)
                    Namespaces[i].ProcessCollections(applicationSymbol, ns);
        }

        protected abstract string? GetNamespace(string? parentNamespace);



    }
}
