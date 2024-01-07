using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectsCollectionsContainersCollection
    {

        #region Internal private members

        private readonly Dictionary<ALObjectType, IALAppObjectsCollectionContainer> _collectionsContainersByObjectType;
        private readonly List<IALAppObjectsCollectionContainer> _collectionsContainers;

        #endregion

        public ALAppObjectsCollectionContainer<ALAppTable> Tables { get; }
        public ALAppObjectsCollectionContainer<ALAppPage> Pages { get; }
        public ALAppObjectsCollectionContainer<ALAppReport> Reports { get; }
        public ALAppObjectsCollectionContainer<ALAppXmlPort> XmlPorts { get; }
        public ALAppObjectsCollectionContainer<ALAppQuery> Queries { get; }
        public ALAppObjectsCollectionContainer<ALAppCodeunit> Codeunits { get; }
        public ALAppObjectsCollectionContainer<ALAppControlAddIn> ControlAddIns { get; }
        public ALAppObjectsCollectionContainer<ALAppPageExtension> PageExtensions { get; }
        public ALAppObjectsCollectionContainer<ALAppTableExtension> TableExtensions { get; }
        public ALAppObjectsCollectionContainer<ALAppProfile> Profiles { get; }
        public ALAppObjectsCollectionContainer<ALAppPageCustomization> PageCustomizations { get; }
        public ALAppObjectsCollectionContainer<ALAppDotNetPackage> DotNetPackages { get; }
        public ALAppObjectsCollectionContainer<ALAppEnum> EnumTypes { get; }
        public ALAppObjectsCollectionContainer<ALAppEnumExtension> EnumExtensionTypes { get; }
        public ALAppObjectsCollectionContainer<ALAppInterface> Interfaces { get; }
        public ALAppObjectsCollectionContainer<ALAppReportExtension> ReportExtensions { get; }
        public ALAppObjectsCollectionContainer<ALAppPermissionSet> PermissionSets { get; }
        public ALAppObjectsCollectionContainer<ALAppPermissionSetExtension> PermissionSetExtensions { get; }
        public ReadOnlyCollection<IALAppObjectsCollectionContainer> AllCollections { get; }

        public ALAppObjectsCollectionsContainersCollection()
        {
            _collectionsContainersByObjectType = new Dictionary<ALObjectType, IALAppObjectsCollectionContainer>();
            _collectionsContainers = new List<IALAppObjectsCollectionContainer>();
            AllCollections = new ReadOnlyCollection<IALAppObjectsCollectionContainer>(_collectionsContainers);

            Tables = CreateCollectionContainer<ALAppTable>(ALObjectType.Table);
            Pages = CreateCollectionContainer<ALAppPage>(ALObjectType.Page);
            Reports = CreateCollectionContainer<ALAppReport>(ALObjectType.Report);
            XmlPorts = CreateCollectionContainer<ALAppXmlPort>(ALObjectType.XmlPort);
            Queries = CreateCollectionContainer<ALAppQuery>(ALObjectType.Query);
            Codeunits = CreateCollectionContainer<ALAppCodeunit>(ALObjectType.Codeunit);
            ControlAddIns = CreateCollectionContainer<ALAppControlAddIn>(ALObjectType.ControlAddIn);
            PageExtensions = CreateCollectionContainer<ALAppPageExtension>(ALObjectType.PageExtension);
            TableExtensions = CreateCollectionContainer<ALAppTableExtension>(ALObjectType.TableExtension);
            Profiles = CreateCollectionContainer<ALAppProfile>(ALObjectType.Profile);
            PageCustomizations = CreateCollectionContainer<ALAppPageCustomization>(ALObjectType.PageCustomization);
            DotNetPackages = CreateCollectionContainer<ALAppDotNetPackage>(ALObjectType.DotNetPackage);
            EnumTypes = CreateCollectionContainer<ALAppEnum>(ALObjectType.EnumType);
            EnumExtensionTypes = CreateCollectionContainer<ALAppEnumExtension>(ALObjectType.EnumExtensionType);
            Interfaces = CreateCollectionContainer<ALAppInterface>(ALObjectType.Interface);
            ReportExtensions = CreateCollectionContainer<ALAppReportExtension>(ALObjectType.ReportExtension);
            PermissionSets = CreateCollectionContainer<ALAppPermissionSet>(ALObjectType.PermissionSet);
            PermissionSetExtensions = CreateCollectionContainer<ALAppPermissionSetExtension>(ALObjectType.PermissionSetExtension);
        }

        private ALAppObjectsCollectionContainer<T> CreateCollectionContainer<T>(ALObjectType objectType) where T : ALAppObject
        {
            var collectionContainer = new ALAppObjectsCollectionContainer<T>(objectType);
            _collectionsContainersByObjectType.Add(objectType, collectionContainer);
            _collectionsContainers.Add(collectionContainer);
            return collectionContainer;
        }

        public IALAppObjectsCollectionContainer GetCollectionContainer(ALObjectType objectType)
        {
            if (_collectionsContainersByObjectType.ContainsKey(objectType))
                return _collectionsContainersByObjectType[objectType];
            return null;
        }

        public IALAppObjectsCollection GetOrCreateCollection(ALObjectType objectType)
        {
            return GetCollectionContainer(objectType)?.GetOrCreateObjectsCollection();
        }

        public IALAppObjectsCollection GetCollection(ALObjectType objectType)
        {
            return GetCollectionContainer(objectType)?.Objects;
        }

    }
}
