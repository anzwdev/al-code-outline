using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class MergedALAppSymbolReference
    {

        public ISymbolReferencesList AllSymbolReferences { get; }
        public MergedALAppObjectsCollection<ALAppTable> Tables { get; }
        public MergedALAppObjectsCollection<ALAppPage> Pages { get; }
        public MergedALAppObjectsCollection<ALAppReport> Reports { get; }
        public MergedALAppObjectsCollection<ALAppXmlPort> XmlPorts { get; }
        public MergedALAppObjectsCollection<ALAppQuery> Queries { get; }
        public MergedALAppObjectsCollection<ALAppCodeunit> Codeunits { get; }
        public MergedALAppObjectsCollection<ALAppControlAddIn> ControlAddIns { get; }
        public MergedALAppObjectExtensionsCollection<ALAppPageExtension> PageExtensions { get; }
        public MergedALAppObjectExtensionsCollection<ALAppTableExtension> TableExtensions { get; }
        public MergedALAppObjectsCollection<ALAppProfile> Profiles { get; }
        public MergedALAppObjectsCollection<ALAppPageCustomization> PageCustomizations { get; }
        public MergedALAppObjectsCollection<ALAppDotNetPackage> DotNetPackages { get; }
        public MergedALAppObjectsCollection<ALAppEnum> EnumTypes { get; }
        public MergedALAppObjectExtensionsCollection<ALAppEnumExtension> EnumExtensionTypes { get; }
        public MergedALAppObjectsCollection<ALAppInterface> Interfaces { get; }
        public MergedALAppObjectExtensionsCollection<ALAppReportExtension> ReportExtensions { get; }
        public MergedALAppObjectsCollection<ALAppPermissionSet> PermissionSets { get; }
        public MergedALAppObjectExtensionsCollection<ALAppPermissionSetExtension> PermissionSetExtensions { get; }

        public MergedALAppSymbolReference(ISymbolReferencesList allSymbolReferences)
        {
            this.AllSymbolReferences = allSymbolReferences;
            this.Tables = new MergedALAppObjectsCollection<ALAppTable>(this.AllSymbolReferences, ALSymbolKind.TableObject, x => x?.Tables);
            this.Pages = new MergedALAppObjectsCollection<ALAppPage>(this.AllSymbolReferences, ALSymbolKind.PageObject, x => x?.Pages);
            this.Reports = new MergedALAppObjectsCollection<ALAppReport>(this.AllSymbolReferences, ALSymbolKind.ReportObject, x => x?.Reports);
            this.XmlPorts = new MergedALAppObjectsCollection<ALAppXmlPort>(this.AllSymbolReferences, ALSymbolKind.XmlPortObject, x => x?.XmlPorts);
            this.Queries = new MergedALAppObjectsCollection<ALAppQuery>(this.AllSymbolReferences, ALSymbolKind.QueryObject, x => x?.Queries);
            this.Codeunits = new MergedALAppObjectsCollection<ALAppCodeunit>(this.AllSymbolReferences, ALSymbolKind.CodeunitObject, x => x?.Codeunits);
            this.ControlAddIns = new MergedALAppObjectsCollection<ALAppControlAddIn>(this.AllSymbolReferences, ALSymbolKind.ControlAddInObject, x => x?.ControlAddIns);
            this.PageExtensions = new MergedALAppObjectExtensionsCollection<ALAppPageExtension>(this.AllSymbolReferences, ALSymbolKind.PageExtensionObject, x => x?.PageExtensions);
            this.TableExtensions = new MergedALAppObjectExtensionsCollection<ALAppTableExtension>(this.AllSymbolReferences, ALSymbolKind.TableExtensionObject, x => x?.TableExtensions);
            this.Profiles = new MergedALAppObjectsCollection<ALAppProfile>(this.AllSymbolReferences, ALSymbolKind.ProfileObject, x => x?.Profiles);
            this.PageCustomizations = new MergedALAppObjectsCollection<ALAppPageCustomization>(this.AllSymbolReferences, ALSymbolKind.PageCustomizationObject, x => x?.PageCustomizations);
            this.DotNetPackages = new MergedALAppObjectsCollection<ALAppDotNetPackage>(this.AllSymbolReferences, ALSymbolKind.DotNetPackage, x => x?.DotNetPackages);
            this.EnumTypes = new MergedALAppObjectsCollection<ALAppEnum>(this.AllSymbolReferences, ALSymbolKind.EnumType, x => x?.EnumTypes);
            this.EnumExtensionTypes = new MergedALAppObjectExtensionsCollection<ALAppEnumExtension>(this.AllSymbolReferences, ALSymbolKind.EnumExtensionType, x => x?.EnumExtensionTypes);
            this.Interfaces = new MergedALAppObjectsCollection<ALAppInterface>(this.AllSymbolReferences, ALSymbolKind.Interface, x => x?.Interfaces);
            this.ReportExtensions = new MergedALAppObjectExtensionsCollection<ALAppReportExtension>(this.AllSymbolReferences, ALSymbolKind.ReportExtensionObject, x => x?.ReportExtensions);
            this.PermissionSets = new MergedALAppObjectsCollection<ALAppPermissionSet>(this.AllSymbolReferences, ALSymbolKind.PermissionSet, x => x?.PermissionSets);
            this.PermissionSetExtensions = new MergedALAppObjectExtensionsCollection<ALAppPermissionSetExtension>(this.AllSymbolReferences, ALSymbolKind.PermissionSetExtension, x => x?.PermissionSetExtensions);
        }



    }
}
