using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppAllObjectsCollection
    {

        public ALAppSymbolReference Symbols { get; }

        public ALAppAllObjectsCollection(ALAppSymbolReference symbols)
        {
            Symbols = symbols;
        }

        public IALAppObjectsCollection GetObjectsCollection(ALObjectType objectType)
        {
            switch (objectType)
            {
                case ALObjectType.Table: return Symbols.Tables;
                case ALObjectType.Page: return Symbols.Pages;
                case ALObjectType.Report: return Symbols.Reports;
                case ALObjectType.XmlPort: return Symbols.XmlPorts;
                case ALObjectType.Query: return Symbols.Queries;
                case ALObjectType.Codeunit: return Symbols.Codeunits;
                case ALObjectType.ControlAddIn: return Symbols.ControlAddIns;
                case ALObjectType.PageExtension: return Symbols.PageExtensions;
                case ALObjectType.TableExtension: return Symbols.TableExtensions;
                case ALObjectType.Profile: return Symbols.Profiles;
                case ALObjectType.PageCustomization: return Symbols.PageCustomizations;
                case ALObjectType.DotNetPackage: return Symbols.DotNetPackages;
                case ALObjectType.EnumType: return Symbols.EnumTypes;
                case ALObjectType.EnumExtensionType: return Symbols.EnumExtensionTypes;
                case ALObjectType.Interface: return Symbols.Interfaces;
                case ALObjectType.ReportExtension: return Symbols.ReportExtensions;
                case ALObjectType.PermissionSet: return Symbols.PermissionSets;
                case ALObjectType.PermissionSetExtension: return Symbols.PermissionSetExtensions;
                default:
                    throw new ArgumentOutOfRangeException("objectType");
            }
        }

        public IALAppObjectsCollection GetOrCreateObjectsCollection(ALObjectType objectType)
        {
            switch (objectType)
            {
                case ALObjectType.Table:
                    if (Symbols.Tables == null)
                        Symbols.Tables = new ALAppObjectsCollection<ALAppTable>();
                    return Symbols.Tables;
                case ALObjectType.Page:
                    if (Symbols.Pages == null)
                        Symbols.Pages = new ALAppObjectsCollection<ALAppPage>();
                    return Symbols.Pages;
                case ALObjectType.Report:
                    if (Symbols.Reports == null)
                        Symbols.Reports = new ALAppObjectsCollection<ALAppReport>();
                    return Symbols.Reports;
                case ALObjectType.XmlPort:
                    if (Symbols.XmlPorts == null)
                        Symbols.XmlPorts = new ALAppObjectsCollection<ALAppXmlPort>();
                    return Symbols.XmlPorts;
                case ALObjectType.Query:
                    if (Symbols.Queries == null)
                        Symbols.Queries = new ALAppObjectsCollection<ALAppQuery>();
                    return Symbols.Queries;
                case ALObjectType.Codeunit:
                    if (Symbols.Codeunits == null)
                        Symbols.Codeunits = new ALAppObjectsCollection<ALAppCodeunit>();
                    return Symbols.Codeunits;
                case ALObjectType.ControlAddIn:
                    if (Symbols.ControlAddIns == null)
                        Symbols.ControlAddIns = new ALAppObjectsCollection<ALAppControlAddIn>();
                    return Symbols.ControlAddIns;
                case ALObjectType.PageExtension:
                    if (Symbols.PageExtensions == null)
                        Symbols.PageExtensions = new ALAppObjectExtensionsCollection<ALAppPageExtension>();
                    return Symbols.PageExtensions;
                case ALObjectType.TableExtension:
                    if (Symbols.TableExtensions == null)
                        Symbols.TableExtensions = new ALAppObjectExtensionsCollection<ALAppTableExtension>();
                    return Symbols.TableExtensions;
                case ALObjectType.Profile:
                    if (Symbols.Profiles == null)
                        Symbols.Profiles = new ALAppObjectsCollection<ALAppProfile>();
                    return Symbols.Profiles;
                case ALObjectType.PageCustomization:
                    if (Symbols.PageCustomizations == null)
                        Symbols.PageCustomizations = new ALAppObjectsCollection<ALAppPageCustomization>();
                    return Symbols.PageCustomizations;
                case ALObjectType.DotNetPackage:
                    if (Symbols.DotNetPackages == null)
                        Symbols.DotNetPackages = new ALAppObjectsCollection<ALAppDotNetPackage>();
                    return Symbols.DotNetPackages;
                case ALObjectType.EnumType:
                    if (Symbols.EnumTypes == null)
                        Symbols.EnumTypes = new ALAppObjectsCollection<ALAppEnum>();
                    return Symbols.EnumTypes;
                case ALObjectType.EnumExtensionType:
                    if (Symbols.EnumExtensionTypes == null)
                        Symbols.EnumExtensionTypes = new ALAppObjectExtensionsCollection<ALAppEnumExtension>();
                    return Symbols.EnumExtensionTypes;
                case ALObjectType.Interface:
                    if (Symbols.Interfaces == null)
                        Symbols.Interfaces = new ALAppObjectsCollection<ALAppInterface>();
                    return Symbols.Interfaces;
                case ALObjectType.ReportExtension:
                    if (Symbols.ReportExtensions == null)
                        Symbols.ReportExtensions = new ALAppObjectExtensionsCollection<ALAppReportExtension>();
                    return Symbols.ReportExtensions;
                case ALObjectType.PermissionSet:
                    if (Symbols.PermissionSets == null)
                        Symbols.PermissionSets = new ALAppObjectsCollection<ALAppPermissionSet>();
                    return Symbols.PermissionSets;
                case ALObjectType.PermissionSetExtension:
                    if (Symbols.PermissionSetExtensions == null)
                        Symbols.PermissionSetExtensions = new ALAppObjectExtensionsCollection<ALAppPermissionSetExtension>();
                    return Symbols.PermissionSetExtensions;
                default:
                    throw new ArgumentOutOfRangeException("objectType");
            }
        }

        public IEnumerable<IALAppObjectsCollection> GetAllObjectCollections()
        {
            if (Symbols.Tables != null)
                yield return Symbols.Tables;
            if (Symbols.Pages != null)
                yield return Symbols.Pages;
            if (Symbols.Reports != null)
                yield return Symbols.Reports;
            if (Symbols.XmlPorts != null)
                yield return Symbols.XmlPorts;
            if (Symbols.Queries != null)
                yield return Symbols.Queries;
            if (Symbols.Codeunits != null)
                yield return Symbols.Codeunits;
            if (Symbols.ControlAddIns != null)
                yield return Symbols.ControlAddIns;
            if (Symbols.PageExtensions != null)
                yield return Symbols.PageExtensions;
            if (Symbols.TableExtensions != null)
                yield return Symbols.TableExtensions;
            if (Symbols.Profiles != null)
                yield return Symbols.Profiles;
            if (Symbols.PageCustomizations != null)
                yield return Symbols.PageCustomizations;
            if (Symbols.DotNetPackages != null)
                yield return Symbols.DotNetPackages;
            if (Symbols.EnumTypes != null)
                yield return Symbols.EnumTypes;
            if (Symbols.EnumExtensionTypes != null)
                yield return Symbols.EnumExtensionTypes;
            if (Symbols.Interfaces != null)
                yield return Symbols.Interfaces;
            if (Symbols.ReportExtensions != null)
                yield return Symbols.ReportExtensions;
            if (Symbols.PermissionSets != null)
                yield return Symbols.PermissionSets;
            if (Symbols.PermissionSetExtensions != null)
                yield return Symbols.PermissionSetExtensions;
        }

        private ALAppObjectsCollection<T> CreateIfNull<T>(ALAppObjectsCollection<T> objectsCollection) where T : ALAppObject
        {
            if (objectsCollection == null)
                objectsCollection = new ALAppObjectsCollection<T>();
            return objectsCollection;
        }

        public void AddRange(IEnumerable<ALAppObject> objects)
        {
            foreach (var obj in objects)
            {
                IALAppObjectsCollection objectsCollection = this.GetOrCreateObjectsCollection(obj.GetALObjectType());
                objectsCollection.Add(obj);
            }
        }

        public void RemoveRange(IEnumerable<ALAppObject> objects)
        {
            foreach (var obj in objects)
            {
                IALAppObjectsCollection objectsCollection = this.GetOrCreateObjectsCollection(obj.GetALObjectType());
                objectsCollection.Remove(obj);
            }
        }

        public bool UsesNamespaces()
        {
            var allObjectCollections = GetAllObjectCollections();
            if (allObjectCollections != null)
                foreach (var objectsCollection in allObjectCollections)
                    if (objectsCollection.UsesNamespaces())
                        return true;
            return false;
        }

    }
}
