using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppSymbolReference : ALAppBaseElement
    {

        public string ReferenceSourceFileName { get; set; }

        public string AppId { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Version { get; set; }

        public List<ALAppNamespace> Namespaces { get; set; }

        #region Objects collections

        public ALAppObjectsCollection<ALAppTable> Tables
        {
            get => _objectsCollections.Tables.Collection;
            set => _objectsCollections.Tables.Collection = value;
        }

        public ALAppObjectsCollection<ALAppPage> Pages
        {
            get => _objectsCollections.Pages.Collection;
            set => _objectsCollections.Pages.Collection = value;
        }

        public ALAppObjectsCollection<ALAppReport> Reports
        {
            get => _objectsCollections.Reports.Collection;
            set => _objectsCollections.Reports.Collection = value;
        }

        public ALAppObjectsCollection<ALAppXmlPort> XmlPorts
        {
            get => _objectsCollections.XmlPorts.Collection;
            set => _objectsCollections.XmlPorts.Collection = value;
        }

        public ALAppObjectsCollection<ALAppQuery> Queries
        {
            get => _objectsCollections.Queries.Collection;
            set => _objectsCollections.Queries.Collection = value;
        }

        public ALAppObjectsCollection<ALAppCodeunit> Codeunits
        {
            get => _objectsCollections.Codeunits.Collection;
            set => _objectsCollections.Codeunits.Collection = value;
        }

        public ALAppObjectsCollection<ALAppControlAddIn> ControlAddIns
        {
            get => _objectsCollections.ControlAddIns.Collection;
            set => _objectsCollections.ControlAddIns.Collection = value;
        }

        public ALAppObjectsCollection<ALAppPageExtension> PageExtensions
        {
            get => _objectsCollections.PageExtensions.Collection;
            set => _objectsCollections.PageExtensions.Collection = value;
        }

        public ALAppObjectsCollection<ALAppTableExtension> TableExtensions
        {
            get => _objectsCollections.TableExtensions.Collection;
            set => _objectsCollections.TableExtensions.Collection = value;
        }

        public ALAppObjectsCollection<ALAppProfile> Profiles
        {
            get => _objectsCollections.Profiles.Collection;
            set => _objectsCollections.Profiles.Collection = value;
        }

        public ALAppObjectsCollection<ALAppPageCustomization> PageCustomizations
        {
            get => _objectsCollections.PageCustomizations.Collection;
            set => _objectsCollections.PageCustomizations.Collection = value;
        }

        public ALAppObjectsCollection<ALAppDotNetPackage> DotNetPackages
        {
            get => _objectsCollections.DotNetPackages.Collection;
            set => _objectsCollections.DotNetPackages.Collection = value;
        }

        public ALAppObjectsCollection<ALAppEnum> EnumTypes
        {
            get => _objectsCollections.EnumTypes.Collection;
            set => _objectsCollections.EnumTypes.Collection = value;
        }

        public ALAppObjectsCollection<ALAppEnumExtension> EnumExtensionTypes
        {
            get => _objectsCollections.EnumExtensionTypes.Collection;
            set => _objectsCollections.EnumExtensionTypes.Collection = value;
        }

        public ALAppObjectsCollection<ALAppInterface> Interfaces
        {
            get => _objectsCollections.Interfaces.Collection;
            set => _objectsCollections.Interfaces.Collection = value;
        }

        public ALAppObjectsCollection<ALAppReportExtension> ReportExtensions
        {
            get => _objectsCollections.ReportExtensions.Collection;
            set => _objectsCollections.ReportExtensions.Collection = value;
        }

        public ALAppObjectsCollection<ALAppPermissionSet> PermissionSets
        {
            get => _objectsCollections.PermissionSets.Collection;
            set => _objectsCollections.PermissionSets.Collection = value;
        }

        public ALAppObjectsCollection<ALAppPermissionSetExtension> PermissionSetExtensions
        {
            get => _objectsCollections.PermissionSetExtensions.Collection;
            set => _objectsCollections.PermissionSetExtensions.Collection = value;
        }

        public ALAppAllObjectsCollection AllObjects { get => _allObjects; }

        #endregion

        #region Internal members

        private ALSymbol _alSymbolCache = null;
        private bool _idReferencesReplaced = false;
        private readonly ALAppObjectsCollectionsContainersCollection _objectsCollections;
        private readonly ALAppAllObjectsCollection _allObjects;

        #endregion

        public ALAppSymbolReference()
        {
            _objectsCollections = new ALAppObjectsCollectionsContainersCollection();
            _allObjects = new ALAppAllObjectsCollection(_objectsCollections);
        }

        #region Objects processing

        public void ReplaceObjects(List<ALAppObject> alObjectsList)
        {
            for (int i = 0; i < alObjectsList.Count; i++)
            {
                this.ReplaceObject(alObjectsList[i]);
            }
        }

        public void ReplaceObject(ALAppObject alObject)
        {
            IALAppObjectsCollection alObjectsCollection = _objectsCollections.GetOrCreateCollection(alObject.GetALObjectType());
            if (alObjectsCollection != null)
            {
                alObjectsCollection.Replace(alObject);
                this.ClearALSymbolCache();
            }
        }

        public T FindObjectByName<T>(ALAppSymbolsCollection<T> collection, string name, bool parsed) where T: ALAppObject
        {
            return FindObjectByName(collection, null, null, name, parsed);
        }

        public T FindObjectByName<T>(ALAppSymbolsCollection<T> collection, HashSet<string> usings, string namespaceName, string name, bool parsed) where T : ALAppObject
        {
            if ((collection == null) || (String.IsNullOrWhiteSpace(name)))
                return null;
            
            T alObject = InternalFindObjectByNamespaceAndName(collection, usings, namespaceName, name, parsed);

            if ((alObject != null) && (parsed) && (!alObject.INT_Parsed))
            {
                this.ParseObject(alObject);
                alObject = InternalFindObjectByNamespaceAndName(collection, usings, namespaceName, name, parsed);
            }

            return alObject;
        }

        private T InternalFindObjectByNamespaceAndName<T>(ALAppSymbolsCollection<T> collection, HashSet<string> usings, string namespaceName, string name, bool parsed) where T : ALAppObject
        {
            if (!String.IsNullOrWhiteSpace(namespaceName))
                return collection
                    .Where(p => (
                        (name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)) &&
                        (namespaceName.Equals(p.NamespaceName))))
                    .FirstOrDefault();

            T foundALObject = null;
            bool hasUsings = ((usings != null) && (usings.Count > 0));
            for (int i=0; i<collection.Count; i++)
            {
                var alObject = collection[i];

                if (name.Equals(alObject.Name, StringComparison.OrdinalIgnoreCase))
                {
                    if ((String.IsNullOrWhiteSpace(alObject.NamespaceName)) || (usings.Contains(alObject.NamespaceName)))
                    {
                        if (foundALObject != null)
                            throw new Exception("AL object exists in multiple namespaces");
                        foundALObject = alObject;
                    }
                }
            }

            return foundALObject;
        }

        public ALAppObject FindObjectById(ALSymbolKind symbolKind, int id, bool parsed)
        {
            switch (symbolKind)
            {
                case ALSymbolKind.TableObject:
                    return this.FindObjectById(this.Tables, id, parsed);
                case ALSymbolKind.PageObject:
                    return this.FindObjectById(this.Pages, id, parsed);
                case ALSymbolKind.ReportObject:
                    return this.FindObjectById(this.Reports, id, parsed);
                case ALSymbolKind.XmlPortObject:
                    return this.FindObjectById(this.XmlPorts, id, parsed);
                case ALSymbolKind.QueryObject:
                    return this.FindObjectById(this.Queries, id, parsed);
                case ALSymbolKind.CodeunitObject:
                    return this.FindObjectById(this.Codeunits, id, parsed);
                case ALSymbolKind.ControlAddInObject:
                    return this.FindObjectById(this.ControlAddIns, id, parsed);
                case ALSymbolKind.PageExtensionObject:
                    return this.FindObjectById(this.PageExtensions, id, parsed);
                case ALSymbolKind.TableExtensionObject:
                    return this.FindObjectById(this.TableExtensions, id, parsed);
                case ALSymbolKind.ProfileObject:
                    return this.FindObjectById(this.Profiles, id, parsed);
                case ALSymbolKind.PageCustomizationObject:
                    return this.FindObjectById(this.PageCustomizations, id, parsed);
                case ALSymbolKind.DotNetPackage:
                    return this.FindObjectById(this.DotNetPackages, id, parsed);
                case ALSymbolKind.EnumType:
                    return this.FindObjectById(this.EnumTypes, id, parsed);
                case ALSymbolKind.EnumExtensionType:
                    return this.FindObjectById(this.EnumExtensionTypes, id, parsed);
                case ALSymbolKind.Interface:
                    return this.FindObjectById(this.Interfaces, id, parsed);
                case ALSymbolKind.ReportExtensionObject:
                    return this.FindObjectById(this.ReportExtensions, id, parsed);
                case ALSymbolKind.PermissionSet:
                    return this.FindObjectById(this.PermissionSets, id, parsed);
                case ALSymbolKind.PermissionSetExtension:
                    return this.FindObjectById(this.PermissionSetExtensions, id, parsed);
            }

            return null;
        }

        public T FindObjectById<T>(ALAppSymbolsCollection<T> collection, int id, bool parsed) where T : ALAppObject
        {
            if ((collection == null) || (id == 0))
                return null;
            T alObject = collection
                .Where(p => (id == p.Id))
                .FirstOrDefault();

            if ((alObject != null) && (parsed) && (!alObject.INT_Parsed))
            {
                this.ParseObject(alObject);
                alObject = collection
                    .Where(p => (id == p.Id))
                    .FirstOrDefault();
            }

            return alObject;
        }

        #endregion

        #region ALSymbolInformation conversion

        protected void ClearALSymbolCache()
        {
            _alSymbolCache = null;
        }

        public ALSymbolsLibrary ToALSymbolsLibrary()
        {
            return new ALSymbolsLibrary(new ALAppSymbolsLibrarySource(this), this.ToALSymbol());
        }

        public override ALSymbol ToALSymbol()
        {
            if (_alSymbolCache == null)
                _alSymbolCache = base.ToALSymbol();
            return _alSymbolCache;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            return new ALSymbol(ALSymbolKind.Package, StringExtensions.Merge(this.Publisher, this.Name, this.Version));
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            base.AddChildALSymbols(symbol);

            this.Tables?.AddCollectionToALSymbol(symbol, ALSymbolKind.TableObjectList);
            this.Pages?.AddCollectionToALSymbol(symbol, ALSymbolKind.PageObjectList);
            this.Reports?.AddCollectionToALSymbol(symbol, ALSymbolKind.ReportObjectList);
            this.XmlPorts?.AddCollectionToALSymbol(symbol, ALSymbolKind.XmlPortObjectList);
            this.Queries?.AddCollectionToALSymbol(symbol, ALSymbolKind.QueryObjectList);
            this.Codeunits?.AddCollectionToALSymbol(symbol, ALSymbolKind.CodeunitObjectList);
            this.ControlAddIns?.AddCollectionToALSymbol(symbol, ALSymbolKind.ControlAddInObjectList);
            this.PageExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.PageExtensionObjectList);
            this.TableExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.TableExtensionObjectList);
            this.Profiles?.AddCollectionToALSymbol(symbol, ALSymbolKind.ProfileObjectList);
            this.PageCustomizations?.AddCollectionToALSymbol(symbol, ALSymbolKind.PageCustomizationObjectList);
            this.DotNetPackages?.AddCollectionToALSymbol(symbol, ALSymbolKind.DotNetPackageList);
            this.EnumTypes?.AddCollectionToALSymbol(symbol, ALSymbolKind.EnumTypeList);
            this.EnumExtensionTypes?.AddCollectionToALSymbol(symbol, ALSymbolKind.EnumExtensionTypeList);
            this.Interfaces?.AddCollectionToALSymbol(symbol, ALSymbolKind.InterfaceObjectList);

            this.ReportExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.ReportExtensionObjectList);
            this.PermissionSets?.AddCollectionToALSymbol(symbol, ALSymbolKind.PermissionSetList);
            this.PermissionSetExtensions?.AddCollectionToALSymbol(symbol, ALSymbolKind.PermissionSetExtensionList);

        }

        #endregion

        #region Enumerators

        public IEnumerable<long> GetIdsEnumerable(ALSymbolKind symbolKind)
        {
            switch (symbolKind)
            {
                case ALSymbolKind.TableObject:
                    return this.Tables?.GetIdsEnumerable();
                case ALSymbolKind.PageObject:
                    return this.Pages?.GetIdsEnumerable();
                case ALSymbolKind.ReportObject:
                    return this.Reports?.GetIdsEnumerable();
                case ALSymbolKind.XmlPortObject:
                    return this.XmlPorts?.GetIdsEnumerable();
                case ALSymbolKind.QueryObject:
                    return this.Queries?.GetIdsEnumerable();
                case ALSymbolKind.CodeunitObject:
                    return this.Codeunits?.GetIdsEnumerable();
                case ALSymbolKind.ControlAddInObject:
                    return this.ControlAddIns?.GetIdsEnumerable();
                case ALSymbolKind.PageExtensionObject:
                    return this.PageExtensions?.GetIdsEnumerable();
                case ALSymbolKind.TableExtensionObject:
                    return this.TableExtensions?.GetIdsEnumerable();
                case ALSymbolKind.ProfileObject:
                    return this.Profiles?.GetIdsEnumerable();
                case ALSymbolKind.PageCustomizationObject:
                    return this.PageCustomizations?.GetIdsEnumerable();
                case ALSymbolKind.DotNetPackage:
                    return this.DotNetPackages?.GetIdsEnumerable();
                case ALSymbolKind.EnumType:
                    return this.EnumTypes?.GetIdsEnumerable();
                case ALSymbolKind.EnumExtensionType:
                    return this.EnumExtensionTypes?.GetIdsEnumerable();
                case ALSymbolKind.Interface:
                    return this.Interfaces?.GetIdsEnumerable();
                case ALSymbolKind.ReportExtensionObject:
                    return this.ReportExtensions?.GetIdsEnumerable();
                case ALSymbolKind.PermissionSet:
                    return this.PermissionSets?.GetIdsEnumerable();
                case ALSymbolKind.PermissionSetExtension:
                    return this.PermissionSetExtensions?.GetIdsEnumerable();
            }

            return null;
        }

        public IEnumerable<ALAppObject> GetAllALAppObjectsEnumerable(HashSet<ALSymbolKind> includeObjects)
        {
            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.TableObject))) && (this.Tables != null)) 
                foreach (ALAppObject alAppObject in this.Tables) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.TableExtensionObject))) && (this.TableExtensions != null))
                foreach (ALAppObject alAppObject in this.TableExtensions) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.CodeunitObject))) && (this.Codeunits != null))
                foreach (ALAppObject alAppObject in this.Codeunits) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.ControlAddInObject))) && (this.ControlAddIns != null))
                foreach (ALAppObject alAppObject in this.ControlAddIns) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.DotNetPackage))) && (this.DotNetPackages != null))
                foreach (ALAppObject alAppObject in this.DotNetPackages) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.EnumType))) && (this.EnumTypes != null))
                foreach (ALAppObject alAppObject in this.EnumTypes) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.EnumExtensionType))) && (this.EnumExtensionTypes != null))
                foreach (ALAppObject alAppObject in this.EnumExtensionTypes) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.Interface))) && (this.Interfaces != null))
                foreach (ALAppObject alAppObject in this.Interfaces) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.PageObject))) && (this.Pages != null)) 
                foreach (ALAppObject alAppObject in this.Pages) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.PageCustomizationObject))) && (this.PageCustomizations != null))
                foreach (ALAppObject alAppObject in this.PageCustomizations) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.PageExtensionObject))) && (this.PageExtensions != null))
                foreach (ALAppObject alAppObject in this.PageExtensions) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.PermissionSet))) && (this.PermissionSets != null))
                foreach (ALAppObject alAppObject in this.PermissionSets) { yield return alAppObject; }
            
            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.PermissionSetExtension))) && (this.PermissionSetExtensions != null))
                foreach (ALAppObject alAppObject in this.PermissionSetExtensions) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.ProfileObject))) && (this.Profiles != null))
                foreach (ALAppObject alAppObject in this.Profiles) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.QueryObject))) && (this.Queries != null))
                foreach (ALAppObject alAppObject in this.Queries) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.ReportObject))) && (this.Reports != null)) 
                foreach (ALAppObject alAppObject in this.Reports) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.ReportExtensionObject))) && (this.ReportExtensions != null))
                foreach (ALAppObject alAppObject in this.ReportExtensions) { yield return alAppObject; }

            if (((includeObjects == null) || (includeObjects.Contains(ALSymbolKind.XmlPortObject))) && (this.XmlPorts != null)) 
                foreach (ALAppObject alAppObject in this.XmlPorts) { yield return alAppObject; }
        }

        #endregion

        #region Id to name references change

        public void AddToObjectsIdMap(ALAppObjectIdMap idMap)
        {
            idMap.AddRange(idMap.TableIdMap, this.Tables);
            idMap.AddRange(idMap.ReportIdMap, this.Reports);
            idMap.AddRange(idMap.CodeunitIdMap, this.Codeunits);
            idMap.AddRange(idMap.XmlPortIdMap, this.XmlPorts);
            idMap.AddRange(idMap.PageIdMap, this.Pages);
            idMap.AddRange(idMap.QueryIdMap, this.Queries);
        }

        public bool IdReferencesReplaced()
        {
            return this._idReferencesReplaced;
        }

        public void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
            if (this.Pages != null)
                for (int i = 0; i < this.Pages.Count; i++)
                    this.Pages[i].ReplaceIdReferences(idMap);

            if (this.PermissionSets != null)
                for (int i = 0; i < this.PermissionSets.Count; i++)
                    this.PermissionSets[i].ReplaceIdReferences(idMap);

            if (this.PermissionSetExtensions != null)
                for (int i = 0; i < this.PermissionSetExtensions.Count; i++)
                    this.PermissionSetExtensions[i].ReplaceIdReferences(idMap);

            this._idReferencesReplaced = true;
        }

        #endregion

        #region Parse object

        public void ParseObject(ALAppObject alAppObject)
        {
            if ((!String.IsNullOrWhiteSpace(alAppObject.ReferenceSourceFileName)) &&
                (!String.IsNullOrWhiteSpace(alAppObject.ReferenceSourceFileName)) &&
                (this.ReferenceSourceFileName.EndsWith(".app", StringComparison.OrdinalIgnoreCase)))
            {
                string content = AppFileHelper.GetAppFileContent(this.ReferenceSourceFileName, alAppObject.ReferenceSourceFileName);
                if (!String.IsNullOrWhiteSpace(content))
                {
                    ALSymbolReferenceCompiler compiler = new ALSymbolReferenceCompiler();
                    List<ALAppObject> objectsList = compiler.CreateObjectsList(alAppObject.ReferenceSourceFileName, content);
                    this.ReplaceObjects(objectsList);
                }
            }
            alAppObject.INT_Parsed = true;
        }

        #endregion

        public string GetNameWithPublisher()
        {
            return this.Publisher.NotNull() + " - " + this.Name.NotNull();
        }

        public bool InternalsVisibleToApp(string appId)
        {
            if (String.IsNullOrWhiteSpace(appId))
                return true;
            appId = appId.Trim();
            if (appId.Equals(this.AppId, StringComparison.OrdinalIgnoreCase))
                return true;

            //check InternalsVisibleTo setting
            return false;
        }

        public void OnAfterDeserialized()
        {
            ProcessNamespaces();
        }

        private void ProcessNamespaces()
        {
            if (Namespaces != null)
                for (int i = 0; i < Namespaces.Count; i++)
                    Namespaces[i].Process(this, null);
        }

    }
}
