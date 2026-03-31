using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class ApplicationSymbolFactory : ApplicationSymbolFactory<ApplicationSymbol>
    {
    }

    internal class ApplicationSymbolFactory<T> : SymbolFactory<T> where T : ApplicationSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.Package;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var name = String.Join(" ", symbol.Publisher, symbol.Name, symbol.Version.ToString());
            var node = base.CreateNode(symbol, kind);

            node.Name = name;
            node.FullName = name;

            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            base.CreateChildNodes(node, symbol);

            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Tables, ALSyntaxNodeKind.TableObjectList, "Tables", SymbolFactoryInstances.TableSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Codeunits, ALSyntaxNodeKind.CodeunitObjectList, "Codeunits", SymbolFactoryInstances.CodeunitSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Pages, ALSyntaxNodeKind.PageObjectList, "Pages", SymbolFactoryInstances.PageSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.PageExtensions, ALSyntaxNodeKind.PageExtensionObjectList, "Page Extensions", SymbolFactoryInstances.PageExtensionSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.PageCustomizations, ALSyntaxNodeKind.PageCustomizationObjectList, "Page Customizations", SymbolFactoryInstances.PageCustomizationSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Reports, ALSyntaxNodeKind.ReportObjectList, "Reports", SymbolFactoryInstances.ReportSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ReportExtensions, ALSyntaxNodeKind.ReportExtensionObjectList, "Report Extensions", SymbolFactoryInstances.ReportExtensionSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.XmlPorts, ALSyntaxNodeKind.XmlPortObjectList, "XmlPorts", SymbolFactoryInstances.XmlPortSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Queries, ALSyntaxNodeKind.QueryObjectList, "Queries", SymbolFactoryInstances.QuerySymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ControlAddIns, ALSyntaxNodeKind.ControlAddInObjectList, "Control AddIns", SymbolFactoryInstances.ControlAddInSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.EnumTypes, ALSyntaxNodeKind.EnumTypeList, "Enums", SymbolFactoryInstances.EnumTypeSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.DotNetPackages, ALSyntaxNodeKind.DotNetPackageList, "DotNet Packages", SymbolFactoryInstances.DotNetPackageSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Interfaces, ALSyntaxNodeKind.InterfaceObjectList, "Interfaces", SymbolFactoryInstances.InterfaceSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.PermissionSets, ALSyntaxNodeKind.PermissionSetList, "PermissionSets", SymbolFactoryInstances.PermissionSetSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.PermissionSetExtensions, ALSyntaxNodeKind.PermissionSetExtensionList, "PermissionSets Extensions", SymbolFactoryInstances.PermissionSetExtensionSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.EnumExtensionTypes, ALSyntaxNodeKind.EnumExtensionTypeList, "Enum Extensions", SymbolFactoryInstances.EnumExtensionTypeSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.TableExtensions, ALSyntaxNodeKind.TableExtensionObjectList, "Table Extensions", SymbolFactoryInstances.TableExtensionSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Profiles, ALSyntaxNodeKind.ProfileObjectList, "Profiles", SymbolFactoryInstances.ProfileSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ProfileExtensions, ALSyntaxNodeKind.ProfileExtensionObjectList, "Profile Extensions", SymbolFactoryInstances.ProfileExtensionSymbolFactory));
        }

    }
}
