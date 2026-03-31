using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public class ProjectSymbolsView
    {
        public ProjectObjectSymbolsCollection<TableSymbol> Tables { get; }
        public ProjectObjectSymbolsCollection<CodeunitSymbol> Codeunits { get; }
        public ProjectObjectSymbolsCollection<PageSymbol> Pages { get; }
        public ProjectObjectExtensionSymbolsCollection<PageExtensionSymbol> PageExtensions { get; }
        public ProjectObjectExtensionSymbolsCollection<PageCustomizationSymbol> PageCustomizations { get; }
        public ProjectObjectSymbolsCollection<ReportSymbol> Reports { get; }
        public ProjectObjectExtensionSymbolsCollection<ReportExtensionSymbol> ReportExtensions { get; }
        public ProjectObjectSymbolsCollection<XmlPortSymbol> XmlPorts { get; }
        public ProjectObjectSymbolsCollection<QuerySymbol> Queries { get; }
        public ProjectObjectSymbolsCollection<ControlAddInSymbol> ControlAddIns { get; }
        public ProjectObjectSymbolsCollection<EnumTypeSymbol> EnumTypes { get; }
        public ProjectObjectSymbolsCollection<DotNetPackageSymbol> DotNetPackages { get; }
        public ProjectObjectSymbolsCollection<InterfaceSymbol> Interfaces { get; }
        public ProjectObjectSymbolsCollection<PermissionSetSymbol> PermissionSets { get; }
        public ProjectObjectExtensionSymbolsCollection<PermissionSetExtensionSymbol> PermissionSetExtensions { get; }
        public ProjectObjectExtensionSymbolsCollection<EnumExtensionTypeSymbol> EnumExtensionTypes { get; }
        public ProjectObjectExtensionSymbolsCollection<TableExtensionSymbol> TableExtensions { get; }
        public ProjectObjectSymbolsCollection<ProfileSymbol> Profiles { get; }
        public ProjectObjectExtensionSymbolsCollection<ProfileExtensionSymbol> ProfileExtensions { get; }
        public ProjectAllObjectsSymbolCollection AllObjects { get; }

        public ProjectSymbolsView(ProjectSymbolsProvider symbolsProvider)
        {
            Tables = new ProjectObjectSymbolsCollection<TableSymbol>(symbolsProvider, (app) => app.Tables);
            Codeunits = new ProjectObjectSymbolsCollection<CodeunitSymbol>(symbolsProvider, (app) => app.Codeunits);
            Pages = new ProjectObjectSymbolsCollection<PageSymbol>(symbolsProvider, (app) => app.Pages);
            PageExtensions = new ProjectObjectExtensionSymbolsCollection<PageExtensionSymbol>(symbolsProvider, (app) => app.PageExtensions);
            PageCustomizations = new ProjectObjectExtensionSymbolsCollection<PageCustomizationSymbol>(symbolsProvider, (app) => app.PageCustomizations);
            Reports = new ProjectObjectSymbolsCollection<ReportSymbol>(symbolsProvider, (app) => app.Reports);
            ReportExtensions = new ProjectObjectExtensionSymbolsCollection<ReportExtensionSymbol>(symbolsProvider, (app) => app.ReportExtensions);
            XmlPorts = new ProjectObjectSymbolsCollection<XmlPortSymbol>(symbolsProvider, (app) => app.XmlPorts);
            Queries = new ProjectObjectSymbolsCollection<QuerySymbol>(symbolsProvider, (app) => app.Queries);
            ControlAddIns = new ProjectObjectSymbolsCollection<ControlAddInSymbol>(symbolsProvider, (app) => app.ControlAddIns);
            EnumTypes = new ProjectObjectSymbolsCollection<EnumTypeSymbol>(symbolsProvider, (app) => app.EnumTypes);
            DotNetPackages = new ProjectObjectSymbolsCollection<DotNetPackageSymbol>(symbolsProvider, (app) => app.DotNetPackages);
            Interfaces = new ProjectObjectSymbolsCollection<InterfaceSymbol>(symbolsProvider, (app) => app.Interfaces);
            PermissionSets = new ProjectObjectSymbolsCollection<PermissionSetSymbol>(symbolsProvider, (app) => app.PermissionSets);
            PermissionSetExtensions = new ProjectObjectExtensionSymbolsCollection<PermissionSetExtensionSymbol>(symbolsProvider, (app) => app.PermissionSetExtensions);
            EnumExtensionTypes = new ProjectObjectExtensionSymbolsCollection<EnumExtensionTypeSymbol>(symbolsProvider, (app) => app.EnumExtensionTypes);
            TableExtensions = new ProjectObjectExtensionSymbolsCollection<TableExtensionSymbol>(symbolsProvider, (app) => app.TableExtensions);
            Profiles = new ProjectObjectSymbolsCollection<ProfileSymbol>(symbolsProvider, (app) => app.Profiles);
            ProfileExtensions = new ProjectObjectExtensionSymbolsCollection<ProfileExtensionSymbol>(symbolsProvider, (app) => app.ProfileExtensions);

            AllObjects = new ProjectAllObjectsSymbolCollection(this);
        }

    }
}
