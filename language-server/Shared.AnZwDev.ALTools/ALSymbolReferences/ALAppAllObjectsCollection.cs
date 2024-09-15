using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppAllObjectsCollection
    {
        private Dictionary<ALObjectType, IALAppObjectsCollection> _objectsByType;

        public ALAppAllObjectsCollection(ALBaseSymbolReference symbolsReference)
        {
            _objectsByType = new Dictionary<ALObjectType, IALAppObjectsCollection>();

            _objectsByType.Add(ALObjectType.Table, symbolsReference.Tables);
            _objectsByType.Add(ALObjectType.Page, symbolsReference.Pages);
            _objectsByType.Add(ALObjectType.Report, symbolsReference.Reports);
            _objectsByType.Add(ALObjectType.XmlPort, symbolsReference.XmlPorts);
            _objectsByType.Add(ALObjectType.Query, symbolsReference.Queries);
            _objectsByType.Add(ALObjectType.Codeunit, symbolsReference.Codeunits);
            _objectsByType.Add(ALObjectType.ControlAddIn, symbolsReference.ControlAddIns);
            _objectsByType.Add(ALObjectType.PageExtension, symbolsReference.PageExtensions);
            _objectsByType.Add(ALObjectType.TableExtension, symbolsReference.TableExtensions);
            _objectsByType.Add(ALObjectType.Profile, symbolsReference.Profiles);
            _objectsByType.Add(ALObjectType.PageCustomization, symbolsReference.PageCustomizations);
            _objectsByType.Add(ALObjectType.DotNetPackage, symbolsReference.DotNetPackages);
            _objectsByType.Add(ALObjectType.EnumType, symbolsReference.EnumTypes);
            _objectsByType.Add(ALObjectType.EnumExtensionType, symbolsReference.EnumExtensionTypes);
            _objectsByType.Add(ALObjectType.Interface, symbolsReference.Interfaces);
            _objectsByType.Add(ALObjectType.ReportExtension, symbolsReference.ReportExtensions);
            _objectsByType.Add(ALObjectType.PermissionSet, symbolsReference.PermissionSets);
            _objectsByType.Add(ALObjectType.PermissionSetExtension, symbolsReference.PermissionSetExtensions);
        }

        public IALAppObjectsCollection GetObjectsCollection(ALObjectType objectType)
        {
            return _objectsByType[objectType];
        }

        public IALAppObjectsCollection GetObjectsCollection(ALSymbolKind symbolKind)
        {
            var typeInformation = ALObjectTypesInformationCollection.Get(symbolKind);
            if (typeInformation != null)
                return _objectsByType[typeInformation.ALObjectType];
            return null;
        }

        public void AddRange(IEnumerable<ALAppObject> objects)
        {
            foreach (var obj in objects)
                Add(obj);
        }

        public void Add(ALAppObject appObject)
        {
            this.GetObjectsCollection(appObject.GetALObjectType()).Add(appObject);
        }

        public void RemoveRange(IEnumerable<ALAppObject> objects)
        {
            foreach (var obj in objects)
                Remove(obj);
        }

        public void Remove(ALAppObject appObject)
        {
            this.GetObjectsCollection(appObject.GetALObjectType()).Remove(appObject);
        }

        public void ReplaceRange(IEnumerable<ALAppObject> objects)
        {
            foreach (var obj in objects)
            {
                IALAppObjectsCollection objectsCollection = this.GetObjectsCollection(obj.GetALObjectType());
                objectsCollection.Replace(obj);
            }
        }

        public void Replace(ALAppObject appObject)
        {
            this.GetObjectsCollection(appObject.GetALObjectType()).Replace(appObject);
        }

        public IEnumerable<ALAppObject> GetObjects(HashSet<ALSymbolKind> includeObjects)
        {
            foreach (var objectType in _objectsByType.Keys)
            {
                if ((includeObjects == null) || (includeObjects.Contains(ALObjectTypesInformationCollection.Get(objectType).ALSymbolKind)))
                    foreach (var appObject in _objectsByType[objectType])
                        yield return appObject;
            }
        }

        public bool UsesNamespaces()
        {
            foreach (var objectsCollection in _objectsByType.Values)
                if (objectsCollection.UsesNamespaces())
                    return true;
            return false;
        }

        public void AddChildALSymbols(ALSymbol symbol)
        {
            foreach (var objectType in _objectsByType.Keys)
            {
                _objectsByType[objectType].AddCollectionToALSymbol(symbol, ALObjectTypesInformationCollection.Get(objectType).ALSymbolKind);
            }

        }

    }
}
