using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal static class SymbolFactoryInstances
    {

        public static readonly ProjectDefinitionSymbolFactory ProjectDefinitionSymbolFactory = new ProjectDefinitionSymbolFactory();
        public static readonly ApplicationSymbolFactory ApplicationSymbolFactory = new ApplicationSymbolFactory();

        public static readonly CodeunitSymbolFactory CodeunitSymbolFactory = new CodeunitSymbolFactory();
        public static readonly TableSymbolFactory TableSymbolFactory = new TableSymbolFactory();
        public static readonly TableExtensionSymbolFactory TableExtensionSymbolFactory = new TableExtensionSymbolFactory();
        public static readonly PageSymbolFactory PageSymbolFactory = new PageSymbolFactory();
        public static readonly PageExtensionSymbolFactory PageExtensionSymbolFactory = new PageExtensionSymbolFactory();
        public static readonly PageCustomizationSymbolFactory PageCustomizationSymbolFactory = new PageCustomizationSymbolFactory();
        public static readonly ReportSymbolFactory ReportSymbolFactory = new ReportSymbolFactory();
        public static readonly ReportExtensionSymbolFactory ReportExtensionSymbolFactory = new ReportExtensionSymbolFactory();
        public static readonly XmlPortSymbolFactory XmlPortSymbolFactory = new XmlPortSymbolFactory();
        public static readonly QuerySymbolFactory QuerySymbolFactory = new QuerySymbolFactory();
        public static readonly ControlAddInSymbolFactory ControlAddInSymbolFactory = new ControlAddInSymbolFactory();
        public static readonly EnumTypeSymbolFactory EnumTypeSymbolFactory = new EnumTypeSymbolFactory();
        public static readonly EnumExtensionTypeSymbolFactory EnumExtensionTypeSymbolFactory = new EnumExtensionTypeSymbolFactory();
        public static readonly DotNetPackageSymbolFactory DotNetPackageSymbolFactory = new DotNetPackageSymbolFactory();
        public static readonly InterfaceSymbolFactory InterfaceSymbolFactory = new InterfaceSymbolFactory();
        public static readonly PermissionSetSymbolFactory PermissionSetSymbolFactory = new PermissionSetSymbolFactory();
        public static readonly PermissionSetExtensionSymbolFactory PermissionSetExtensionSymbolFactory = new PermissionSetExtensionSymbolFactory();
        public static readonly ProfileSymbolFactory ProfileSymbolFactory = new ProfileSymbolFactory();
        public static readonly ProfileExtensionSymbolFactory ProfileExtensionSymbolFactory = new ProfileExtensionSymbolFactory();

        public static readonly GlobalVariableDeclarationSymbolFactory GlobalVariableDeclarationSymbolFactory = new GlobalVariableDeclarationSymbolFactory();

        public static readonly MethodSymbolFactory MethodSymbolFactory = new MethodSymbolFactory();
        public static readonly MethodParameterSymbolFactory MethodParameterSymbolFactory = new MethodParameterSymbolFactory();
        public static readonly EventSymbolFactory EventSymbolFactory = new EventSymbolFactory();

        public static readonly TableFieldSymbolFactory TableFieldSymbolFactory = new TableFieldSymbolFactory();
        public static readonly TableKeySymbolFactory TableKeySymbolFactory = new TableKeySymbolFactory();
        public static readonly TableFieldGroupSymbolFactory TableFieldGroupSymbolFactory = new TableFieldGroupSymbolFactory();

        public static readonly PageControlSymbolFactory PageControlSymbolFactory = new PageControlSymbolFactory();
        public static readonly PageActionSymbolFactory PageActionSymbolFactory = new PageActionSymbolFactory();
        public static readonly PageViewSymbolFactory PageViewSymbolFactory = new PageViewSymbolFactory();

        public static readonly PageControlChangeSymbolFactory PageControlChangeSymbolFactory = new PageControlChangeSymbolFactory();
        public static readonly PageActionChangeSymbolFactory PageActionChangeSymbolFactory = new PageActionChangeSymbolFactory();
        public static readonly PageViewChangeSymbolFactory PageViewChangeSymbolFactory = new PageViewChangeSymbolFactory();

        public static readonly RequestPageSymbolFactory RequestPageSymbolFactory = new RequestPageSymbolFactory();
        public static readonly RequestPageExtensionSymbolFactory RequestPageExtensionSymbolFactory = new RequestPageExtensionSymbolFactory();

        public static readonly ReportColumnSymbolFactory ReportColumnSymbolFactory = new ReportColumnSymbolFactory();
        public static readonly ReportDataItemSymbolFactory ReportDataItemSymbolFactory = new ReportDataItemSymbolFactory();
        public static readonly ReportLabelSymbolFactory ReportLabelSymbolFactory = new ReportLabelSymbolFactory();
        public static readonly ReportLayoutSymbolFactory ReportLayoutSymbolFactory = new ReportLayoutSymbolFactory();

        public static readonly TableFieldGroupExtensionSymbolFactory TableFieldGroupExtensionSymbolFactory = new TableFieldGroupExtensionSymbolFactory();

        public static readonly QueryColumnSymbolFactory QueryColumnSymbolFactory = new QueryColumnSymbolFactory();
        public static readonly QueryDataItemSymbolFactory QueryDataItemSymbolFactory = new QueryDataItemSymbolFactory();

        public static readonly EnumValueSymbolFactory EnumValueSymbolFactory = new EnumValueSymbolFactory();

        public static readonly DotNetAssemblyDeclarationSymbolFactory DotNetAssemblyDeclarationSymbolFactory = new DotNetAssemblyDeclarationSymbolFactory();
        public static readonly DotNetTypeDeclarationSymbolFactory DotNetTypeDeclarationSymbolFactory = new DotNetTypeDeclarationSymbolFactory();

        public static readonly PermissionSymbolFactory PermissionSymbolFactory = new PermissionSymbolFactory();

        public static readonly XmlPortNodeSymbolFactory XmlPortNodeSymbolFactory = new XmlPortNodeSymbolFactory();

    }
}
