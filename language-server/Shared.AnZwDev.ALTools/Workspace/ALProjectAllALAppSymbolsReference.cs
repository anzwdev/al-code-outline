using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectAllALAppSymbolsReference
    {

        public ALProject Project { get; }

        public ALProjectAllALAppObjectsCollection<ALAppTable> Tables { get; }
        public ALProjectAllALAppObjectsCollection<ALAppPage> Pages { get; }
        public ALProjectAllALAppObjectsCollection<ALAppReport> Reports { get; }
        public ALProjectAllALAppObjectsCollection<ALAppXmlPort> XmlPorts { get; }
        public ALProjectAllALAppObjectsCollection<ALAppQuery> Queries { get; }
        public ALProjectAllALAppObjectsCollection<ALAppCodeunit> Codeunits { get; }
        public ALProjectAllALAppObjectsCollection<ALAppControlAddIn> ControlAddIns { get; }
        public ALProjectAllALAppObjectExtensionsCollection<ALAppPageExtension> PageExtensions { get; }
        public ALProjectAllALAppObjectExtensionsCollection<ALAppTableExtension> TableExtensions { get; }
        public ALProjectAllALAppObjectsCollection<ALAppProfile> Profiles { get; }
        public ALProjectAllALAppObjectsCollection<ALAppPageCustomization> PageCustomizations { get; }
        public ALProjectAllALAppObjectsCollection<ALAppDotNetPackage> DotNetPackages { get; }
        public ALProjectAllALAppObjectsCollection<ALAppEnum> EnumTypes { get; }
        public ALProjectAllALAppObjectExtensionsCollection<ALAppEnumExtension> EnumExtensionTypes { get; }
        public ALProjectAllALAppObjectsCollection<ALAppInterface> Interfaces { get; }
        public ALProjectAllALAppObjectExtensionsCollection<ALAppReportExtension> ReportExtensions { get; }
        public ALProjectAllALAppObjectsCollection<ALAppPermissionSet> PermissionSets { get; }
        public ALProjectAllALAppObjectExtensionsCollection<ALAppPermissionSetExtension> PermissionSetExtensions { get; }

        private readonly Dictionary<ALObjectType, IALProjectAllALAppObjectsCollection> _allObjects;

        public ALProjectAllALAppSymbolsReference(ALProject project)
        {
            _allObjects = new Dictionary<ALObjectType, IALProjectAllALAppObjectsCollection>();

            Project = project;
            Tables = CreateObjectsCollection<ALAppTable>(Project, ALObjectType.Table, x => x.Tables);
            Pages = CreateObjectsCollection<ALAppPage>(Project, ALObjectType.Page, x => x.Pages);
            Reports = CreateObjectsCollection<ALAppReport>(Project, ALObjectType.Report, x => x.Reports);
            XmlPorts = CreateObjectsCollection<ALAppXmlPort>(Project, ALObjectType.XmlPort, x => x.XmlPorts);
            Queries = CreateObjectsCollection<ALAppQuery>(Project, ALObjectType.Query, x => x.Queries);
            Codeunits = CreateObjectsCollection<ALAppCodeunit>(Project, ALObjectType.Codeunit, x => x.Codeunits);
            ControlAddIns = CreateObjectsCollection<ALAppControlAddIn>(Project, ALObjectType.ControlAddIn, x => x.ControlAddIns);
            PageExtensions = CreateObjectsExtensionsCollection<ALAppPageExtension>(Project, ALObjectType.PageExtension, x => x.PageExtensions);
            TableExtensions = CreateObjectsExtensionsCollection<ALAppTableExtension>(Project, ALObjectType.TableExtension, x => x.TableExtensions);
            Profiles = CreateObjectsCollection<ALAppProfile>(Project, ALObjectType.Profile, x => x.Profiles);
            PageCustomizations = CreateObjectsCollection<ALAppPageCustomization>(Project, ALObjectType.PageCustomization, x => x.PageCustomizations);
            DotNetPackages = CreateObjectsCollection<ALAppDotNetPackage>(Project, ALObjectType.DotNetPackage, x => x.DotNetPackages);
            EnumTypes = CreateObjectsCollection<ALAppEnum>(Project, ALObjectType.EnumType, x => x.EnumTypes);
            EnumExtensionTypes = CreateObjectsExtensionsCollection<ALAppEnumExtension>(Project, ALObjectType.EnumExtensionType, x => x.EnumExtensionTypes);
            Interfaces = CreateObjectsCollection<ALAppInterface>(Project, ALObjectType.Interface, x => x.Interfaces);
            ReportExtensions = CreateObjectsExtensionsCollection<ALAppReportExtension>(Project, ALObjectType.ReportExtension, x => x.ReportExtensions);
            PermissionSets = CreateObjectsCollection<ALAppPermissionSet>(Project, ALObjectType.PermissionSet, x => x.PermissionSets);
            PermissionSetExtensions = CreateObjectsExtensionsCollection<ALAppPermissionSetExtension>(Project, ALObjectType.PermissionSetExtension, x => x.PermissionSetExtensions);
        }

        private ALProjectAllALAppObjectsCollection<T> CreateObjectsCollection<T>(ALProject project, ALObjectType objectType, Func<ALAppSymbolReference, ALAppObjectsCollection<T>> objectsCollection) where T : ALAppObject
        {
            var objects = new ALProjectAllALAppObjectsCollection<T>(project, objectType, objectsCollection);
            _allObjects.Add(objectType, objects);
            return objects;
        }

        private ALProjectAllALAppObjectExtensionsCollection<T> CreateObjectsExtensionsCollection<T>(ALProject project, ALObjectType objectType, Func<ALAppSymbolReference, ALAppObjectExtensionsCollection<T>> objectsCollection) where T : ALAppObject, IALAppObjectExtension
        {
            var objects = new ALProjectAllALAppObjectExtensionsCollection<T>(project, objectType, objectsCollection);
            _allObjects.Add(objectType, objects);
            return objects;
        }

        public ALAppObject FindFirst(ALObjectType objectType, ALObjectReference objectReference, bool includeInaccessible = false)
        {
            var objectsCollection = _allObjects[objectType];
            return objectsCollection.FindFirst(objectReference, includeInaccessible);
        }

        public IALProjectAllALAppObjectsCollection GetObjectsCollection(ALObjectType objectType)
        {
            if (_allObjects.ContainsKey(objectType))
                return _allObjects[objectType];
            return null;
        }

    }
}
