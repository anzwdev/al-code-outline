using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALBaseSymbolReference : ALAppBaseElement
    {

        public string ReferenceSourceFileName { get; set; }
        public string AppId { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Version { get; set; }

        public List<ALAppNamespace> Namespaces { get; set; }

        #region Objects collections

        public ALAppObjectsCollection<ALAppTable> Tables { get; }
        public ALAppObjectsCollection<ALAppPage> Pages { get; }
        public ALAppObjectsCollection<ALAppReport> Reports { get; }
        public ALAppObjectsCollection<ALAppXmlPort> XmlPorts { get; }
        public ALAppObjectsCollection<ALAppQuery> Queries { get; }
        public ALAppObjectsCollection<ALAppCodeunit> Codeunits { get; }
        public ALAppObjectsCollection<ALAppControlAddIn> ControlAddIns { get; }
        public ALAppObjectExtensionsCollection<ALAppPageExtension> PageExtensions { get; }
        public ALAppObjectExtensionsCollection<ALAppTableExtension> TableExtensions { get; }
        public ALAppObjectsCollection<ALAppProfile> Profiles { get; }
        public ALAppObjectsCollection<ALAppPageCustomization> PageCustomizations { get; }
        public ALAppObjectsCollection<ALAppDotNetPackage> DotNetPackages { get; }
        public ALAppObjectsCollection<ALAppEnum> EnumTypes { get; }
        public ALAppObjectExtensionsCollection<ALAppEnumExtension> EnumExtensionTypes { get; }
        public ALAppObjectsCollection<ALAppInterface> Interfaces { get; }
        public ALAppObjectExtensionsCollection<ALAppReportExtension> ReportExtensions { get; }
        public ALAppObjectsCollection<ALAppPermissionSet> PermissionSets { get; }
        public ALAppObjectExtensionsCollection<ALAppPermissionSetExtension> PermissionSetExtensions { get; }

        public ALAppAllObjectsCollection AllObjects { get; }

        #endregion

        private ALSymbol _alSymbolCache = null;

        public ALBaseSymbolReference()
        {
            Tables = new ALAppObjectsCollection<ALAppTable>(this);
            Pages = new ALAppObjectsCollection<ALAppPage>(this);
            Reports = new ALAppObjectsCollection<ALAppReport>(this);
            XmlPorts = new ALAppObjectsCollection<ALAppXmlPort>(this);
            Queries = new ALAppObjectsCollection<ALAppQuery>(this);
            Codeunits = new ALAppObjectsCollection<ALAppCodeunit>(this);
            ControlAddIns = new ALAppObjectsCollection<ALAppControlAddIn>(this);
            PageExtensions = new ALAppObjectExtensionsCollection<ALAppPageExtension>(this);
            TableExtensions = new ALAppObjectExtensionsCollection<ALAppTableExtension>(this);
            Profiles = new ALAppObjectsCollection<ALAppProfile>(this);
            PageCustomizations = new ALAppObjectsCollection<ALAppPageCustomization>(this);
            DotNetPackages = new ALAppObjectsCollection<ALAppDotNetPackage>(this);
            EnumTypes = new ALAppObjectsCollection<ALAppEnum>(this);
            EnumExtensionTypes = new ALAppObjectExtensionsCollection<ALAppEnumExtension>(this);
            Interfaces = new ALAppObjectsCollection<ALAppInterface>(this);
            ReportExtensions = new ALAppObjectExtensionsCollection<ALAppReportExtension>(this);
            PermissionSets = new ALAppObjectsCollection<ALAppPermissionSet>(this);
            PermissionSetExtensions = new ALAppObjectExtensionsCollection<ALAppPermissionSetExtension>(this);

            //must be called after collections are created, this constructor uses them
            AllObjects = new ALAppAllObjectsCollection(this);
        }

        internal virtual void OnObjectAdded(ALAppObject alAppObject)
        {
            CrearSymbolCache();
        }

        internal virtual void OnObjectRemoved(ALAppObject alAppObject) 
        {
            CrearSymbolCache();
        }

        internal virtual void OnObjectsClear()
        {
            CrearSymbolCache();
        }

        private void CrearSymbolCache()
        {
            _alSymbolCache = null;
        }

        #region ALSymbolInformation conversion

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
            AllObjects.AddChildALSymbols(symbol);
        }

        #endregion

    }
}
