using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews
{
    public partial class SyntaxTreeSymbolsTreeViewBuilder
    {

        private bool _hasTableKeys = false;

        protected SyntaxTreeSymbolsTreeViewNode? CreateSymbol(SyntaxNode syntaxNode, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            switch (syntaxNode)
            {
                // Code

                case MethodDeclarationSyntax methodDeclarationSyntax:
                    return MethodDeclarationSymbolFactory.CreateSymbol(methodDeclarationSyntax, parentNode);
                case TriggerDeclarationSyntax triggerDeclarationSyntax:
                    return TriggerDeclarationSymbolFactory.CreateSymbol(triggerDeclarationSyntax, parentNode);
                case EventTriggerDeclarationSyntax eventTriggerDeclarationSyntax:
                    return EventTriggerDeclarationSymbolFactory.CreateSymbol(eventTriggerDeclarationSyntax, parentNode);
                case ParameterListSyntax parameterListSyntax:
                    return ParameterListSymbolFactory.CreateSymbol(parameterListSyntax, parentNode);
                case ParameterSyntax parameterSyntax:
                    return ParameterSymbolFactory.CreateSymbol(parameterSyntax, parentNode);
                case VarSectionSyntax varSectionSyntax:
                    return VarSectionSymbolFactory.CreateSymbol(varSectionSyntax, parentNode);
                case GlobalVarSectionSyntax globalVarSectionSyntax:
                    return GlobalVarSectionSymbolFactory.CreateSymbol(globalVarSectionSyntax, parentNode);
                case VariableDeclarationSyntax variableDeclarationSyntax:
                    return VariableDeclarationSymbolFactory.CreateSymbol(variableDeclarationSyntax, parentNode);
                case VariableDeclarationNameSyntax variableDeclarationNameSyntax:
                    return VariableDeclarationNameSymbolFactory.CreateSymbol(variableDeclarationNameSyntax, parentNode);

                // ControlAddIns

                case EventDeclarationSyntax eventDeclarationSyntax:
                    return EventDeclarationSymbolFactory.CreateSymbol(eventDeclarationSyntax, parentNode);

                // DotNet

                case DotNetAssemblySyntax dotNetAssemblySyntax:
                    return DotNetAssemblySymbolFactory.CreateSymbol(dotNetAssemblySyntax, parentNode);
                case DotNetTypeDeclarationSyntax dotNetTypeDeclarationSyntax:
                    return DotNetTypeDeclarationSymbolFactory.CreateSymbol(dotNetTypeDeclarationSyntax, parentNode);

                // Enums

                case EnumValueSyntax enumValueSyntax:
                    return EnumValueSymbolFactory.CreateSymbol(enumValueSyntax, parentNode);

                // Generic

                case CompilationUnitSyntax compilationUnitSyntax:
                    return CompilationUnitSymbolFactory.CreateSymbol(compilationUnitSyntax, parentNode);
                case UsingDirectiveSyntax usingDirectiveSyntax:
                    return UsingDirectiveSymbolFactory.CreateSymbol(usingDirectiveSyntax, parentNode);
                case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                    return NamespaceDeclarationSymbolFactory.CreateSymbol(namespaceDeclarationSyntax, parentNode);
                case PropertyListSyntax propertyListSyntax:
                    return PropertyListSymbolFactory.CreateSymbol(propertyListSyntax, parentNode);
                case PropertySyntax propertySyntax:
                    return PropertySymbolFactory.CreateSymbol(propertySyntax, parentNode);

                // Objects

                case CodeunitSyntax codeunitSyntax:
                    return CodeunitSymbolFactory.CreateSymbol(codeunitSyntax, parentNode);
                case TableSyntax tableSyntax:
                    _hasTableKeys = false;
                    return TableSymbolFactory.CreateSymbol(tableSyntax, parentNode);
                case TableExtensionSyntax tableExtensionSyntax:
                    return TableExtensionSymbolFactory.CreateSymbol(tableExtensionSyntax, parentNode);
                case PageSyntax pageSyntax:
                    return PageSymbolFactory.CreateSymbol(pageSyntax, parentNode);
                case PageExtensionSyntax pageExtensionSyntax:
                    return PageExtensionSymbolFactory.CreateSymbol(pageExtensionSyntax, parentNode);
                case PageCustomizationSyntax pageCustomizationSyntax:
                    return PageCustomizationSymbolFactory.CreateSymbol(pageCustomizationSyntax, parentNode);
                case ReportSyntax reportSyntax:
                    return ReportSymbolFactory.CreateSymbol(reportSyntax, parentNode);
                case ReportExtensionSyntax reportExtensionSyntax:
                    return ReportExtensionSymbolFactory.CreateSymbol(reportExtensionSyntax, parentNode);
                case XmlPortSyntax xmlPortSyntax:
                    return XmlPortSymbolFactory.CreateSymbol(xmlPortSyntax, parentNode);
                case QuerySyntax querySyntax:
                    return QuerySymbolFactory.CreateSymbol(querySyntax, parentNode);
                case ControlAddInSyntax controlAddInSyntax:
                    return ControlAddInSymbolFactory.CreateSymbol(controlAddInSyntax, parentNode);
                case ProfileSyntax profileSyntax:
                    return ProfileSymbolFactory.CreateSymbol(profileSyntax, parentNode);
                case ProfileExtensionSyntax profileExtensionSyntax:
                    return ProfileExtensionSymbolFactory.CreateSymbol(profileExtensionSyntax, parentNode);
                case DotNetPackageSyntax dotNetPackageSyntax:
                    return DotNetPackageSymbolFactory.CreateSymbol(dotNetPackageSyntax, parentNode);
                case InterfaceSyntax interfaceSyntax:
                    return InterfaceSymbolFactory.CreateSymbol(interfaceSyntax, parentNode);
                case PermissionSetSyntax permissionSetSyntax:
                    return PermissionSetSymbolFactory.CreateSymbol(permissionSetSyntax, parentNode);
                case PermissionSetExtensionSyntax permissionSetExtensionSyntax:
                    return PermissionSetExtensionSymbolFactory.CreateSymbol(permissionSetExtensionSyntax, parentNode);
                case EntitlementSyntax entitlementSyntax:
                    return EntitlementSymbolFactory.CreateSymbol(entitlementSyntax, parentNode);
                case EnumTypeSyntax enumTypeSyntax:
                    return EnumTypeSymbolFactory.CreateSymbol(enumTypeSyntax, parentNode);
                case EnumExtensionTypeSyntax enumExtensionTypeSyntax:
                    return EnumExtensionTypeSymbolFactory.CreateSymbol(enumExtensionTypeSyntax, parentNode);

                // Pages

                case PageLayoutSyntax pageLayoutSyntax:
                    return PageLayoutSymbolFactory.CreateSymbol(pageLayoutSyntax, parentNode);
                case PageAreaSyntax pageAreaSyntax:
                    return PageAreaSymbolFactory.CreateSymbol(pageAreaSyntax, parentNode);
                case PageGroupSyntax pageGroupSyntax:
                    return PageGroupSymbolFactory.CreateSymbol(pageGroupSyntax, parentNode);
                case PageFieldSyntax pageFieldSyntax:
                    return PageFieldSymbolFactory.CreateSymbol(pageFieldSyntax, parentNode);
                case PageLabelSyntax pageLabelSyntax:
                    return PageLabelSymbolFactory.CreateSymbol(pageLabelSyntax, parentNode);
                case PagePartSyntax pagePartSyntax:
                    return PagePartSymbolFactory.CreateSymbol(pagePartSyntax, parentNode);
                case PageChartPartSyntax pageChartPartSyntax:
                    return PageChartPartSymbolFactory.CreateSymbol(pageChartPartSyntax, parentNode);
                case PageSystemPartSyntax pageSystemPartSyntax:
                    return PageSystemPartSymbolFactory.CreateSymbol(pageSystemPartSyntax, parentNode);
                case PageUserControlSyntax pageUserControlSyntax:
                    return PageUserControlSymbolFactory.CreateSymbol(pageUserControlSyntax, parentNode);
                case PageActionListSyntax pageActionListSyntax:
                    return PageActionListSymbolFactory.CreateSymbol(pageActionListSyntax, parentNode);
                case PageActionAreaSyntax pageActionAreaSyntax:
                    return PageActionAreaSymbolFactory.CreateSymbol(pageActionAreaSyntax, parentNode);
                case PageActionGroupSyntax pageActionGroupSyntax:
                    return PageActionGroupSymbolFactory.CreateSymbol(pageActionGroupSyntax, parentNode);
                case PageActionSyntax pageActionSyntax:
                    return PageActionSymbolFactory.CreateSymbol(pageActionSyntax, parentNode);
                case PageFileUploadActionSyntax pageFileUploadActionSyntax:
                    return PageFileUploadActionSymbolFactory.CreateSymbol(pageFileUploadActionSyntax, parentNode);
                case PageActionSeparatorSyntax pageActionSeparatorSyntax:
                    return PageActionSeparatorSymbolFactory.CreateSymbol(pageActionSeparatorSyntax, parentNode);
                case PageActionRefSyntax pageActionRefSyntax:
                    return PageActionRefSymbolFactory.CreateSymbol(pageActionRefSyntax, parentNode);
                case PageCustomActionSyntax pageCustomActionSyntax:
                    return PageCustomActionSymbolFactory.CreateSymbol(pageCustomActionSyntax, parentNode);
                case PageSystemActionSyntax pageSystemActionSyntax:
                    return PageSystemActionSymbolFactory.CreateSymbol(pageSystemActionSyntax, parentNode);
                case PageViewListSyntax pageViewListSyntax:
                    return PageViewListSymbolFactory.CreateSymbol(pageViewListSyntax, parentNode);
                case PageViewSyntax pageViewSyntax:
                    return PageViewSymbolFactory.CreateSymbol(pageViewSyntax, parentNode);
                case GroupActionListSyntax groupActionListSyntax:
                    return GroupActionListSymbolFactory.CreateSymbol(groupActionListSyntax, parentNode);
                case PageExtensionLayoutSyntax pageExtensionLayoutSyntax:
                    return PageExtensionLayoutSymbolFactory.CreateSymbol(pageExtensionLayoutSyntax, parentNode);
                case ControlAddChangeSyntax controlAddChangeSyntax:
                    return ControlAddChangeSymbolFactory.CreateSymbol(controlAddChangeSyntax, parentNode);
                case ControlModifyChangeSyntax controlModifyChangeSyntax:
                    return ControlModifyChangeSymbolFactory.CreateSymbol(controlModifyChangeSyntax, parentNode);
                case ControlMoveChangeSyntax controlMoveChangeSyntax:
                    return ControlMoveChangeSymbolFactory.CreateSymbol(controlMoveChangeSyntax, parentNode);
                case PageExtensionActionListSyntax pageExtensionActionListSyntax:
                    return PageExtensionActionListSymbolFactory.CreateSymbol(pageExtensionActionListSyntax, parentNode);
                case ActionAddChangeSyntax actionAddChangeSyntax:
                    return ActionAddChangeSymbolFactory.CreateSymbol(actionAddChangeSyntax, parentNode);
                case ActionModifyChangeSyntax actionModifyChangeSyntax:
                    return ActionModifyChangeSymbolFactory.CreateSymbol(actionModifyChangeSyntax, parentNode);
                case ActionMoveChangeSyntax actionMoveChangeSyntax:
                    return ActionMoveChangeSymbolFactory.CreateSymbol(actionMoveChangeSyntax, parentNode);
                case PageExtensionViewListSyntax pageExtensionViewListSyntax:
                    return PageExtensionViewListSymbolFactory.CreateSymbol(pageExtensionViewListSyntax, parentNode);
                case ViewAddChangeSyntax viewAddChangeSyntax:
                    return ViewAddChangeSymbolFactory.CreateSymbol(viewAddChangeSyntax, parentNode);
                case ViewModifyChangeSyntax viewModifyChangeSyntax:
                    return ViewModifyChangeSymbolFactory.CreateSymbol(viewModifyChangeSyntax, parentNode);
                case ViewMoveChangeSyntax viewMoveChangeSyntax:
                    return ViewMoveChangeSymbolFactory.CreateSymbol(viewMoveChangeSyntax, parentNode);
                case RequestPageSyntax requestPageSyntax:
                    return RequestPageSymbolFactory.CreateSymbol(requestPageSyntax, parentNode);
                case RequestPageExtensionSyntax requestPageExtensionSyntax:
                    return RequestPageExtensionSymbolFactory.CreateSymbol(requestPageExtensionSyntax, parentNode);

                // Queries

                case QueryElementsSyntax queryElementsSyntax:
                    return QueryElementsSymbolFactory.CreateSymbol(queryElementsSyntax, parentNode);
                case QueryDataItemSyntax queryDataItemSyntax:
                    return QueryDataItemSymbolFactory.CreateSymbol(queryDataItemSyntax, parentNode);
                case QueryColumnSyntax queryColumnSyntax:
                    return QueryColumnSymbolFactory.CreateSymbol(queryColumnSyntax, parentNode);
                case QueryFilterSyntax queryFilterSyntax:
                    return QueryFilterSymbolFactory.CreateSymbol(queryFilterSyntax, parentNode);

                // Reports

                case ReportDataSetSectionSyntax reportDataSetSectionSyntax:
                    return ReportDataSetSectionSymbolFactory.CreateSymbol(reportDataSetSectionSyntax, parentNode);
                case ReportDataItemSyntax reportDataItemSyntax:
                    return ReportDataItemSymbolFactory.CreateSymbol(reportDataItemSyntax, parentNode);
                case ReportColumnSyntax reportColumnSyntax:
                    return ReportColumnSymbolFactory.CreateSymbol(reportColumnSyntax, parentNode);
                case ReportLabelsSectionSyntax reportLabelsSectionSyntax:
                    return ReportLabelsSectionSymbolFactory.CreateSymbol(reportLabelsSectionSyntax, parentNode);
                case ReportLabelSyntax reportLabelSyntax:
                    return ReportLabelSymbolFactory.CreateSymbol(reportLabelSyntax, parentNode);
                case ReportLabelMultilanguageSyntax reportLabelMultilanguageSyntax:
                    return ReportLabelMultilanguageSymbolFactory.CreateSymbol(reportLabelMultilanguageSyntax, parentNode);
                case ReportLayoutSyntax reportLayoutSyntax:
                    return ReportLayoutSymbolFactory.CreateSymbol(reportLayoutSyntax, parentNode);
                case ReportRenderingSectionSyntax reportRenderingSectionSyntax:
                    return ReportRenderingSectionSymbolFactory.CreateSymbol(reportRenderingSectionSyntax, parentNode);
                case ReportExtensionDataSetAddColumnSyntax reportExtensionDataSetAddColumnSyntax:
                    return ReportExtensionDataSetAddColumnSymbolFactory.CreateSymbol(reportExtensionDataSetAddColumnSyntax, parentNode);
                case ReportExtensionDataSetAddDataItemSyntax reportExtensionDataSetAddDataItemSyntax:
                    return ReportExtensionDataSetAddDataItemSymbolFactory.CreateSymbol(reportExtensionDataSetAddDataItemSyntax, parentNode);
                case ReportExtensionDataSetModifySyntax reportExtensionDataSetModifySyntax:
                    return ReportExtensionDataSetModifySymbolFactory.CreateSymbol(reportExtensionDataSetModifySyntax, parentNode);
                case ReportExtensionDataSetSectionSyntax reportExtensionDataSetSectionSyntax:
                    return ReportExtensionDataSetSectionSymbolFactory.CreateSymbol(reportExtensionDataSetSectionSyntax, parentNode);

                // Tables

                case FieldListSyntax fieldListSyntax:
                    return FieldListSymbolFactory.CreateSymbol(fieldListSyntax, parentNode);
                case FieldSyntax fieldSyntax:
                    return FieldSymbolFactory.CreateSymbol(fieldSyntax, parentNode);
                case KeyListSyntax keyListSyntax:
                    return KeyListSymbolFactory.CreateSymbol(keyListSyntax, parentNode);
                case KeySyntax keySyntax:
                    var keySymbol = KeySymbolFactory.CreateSymbol(keySyntax, parentNode, _hasTableKeys);
                    if (keySymbol != null)
                        _hasTableKeys = true;
                    return keySymbol;
                case FieldGroupListSyntax fieldGroupListSyntax:
                    return FieldGroupListSymbolFactory.CreateSymbol(fieldGroupListSyntax, parentNode);
                case FieldGroupSyntax fieldGroupSyntax:
                    return FieldGroupSymbolFactory.CreateSymbol(fieldGroupSyntax, parentNode);
                case FieldExtensionListSyntax fieldExtensionListSyntax:
                    return FieldExtensionListSymbolFactory.CreateSymbol(fieldExtensionListSyntax, parentNode);
                case FieldModificationSyntax fieldModificationSyntax:
                    return FieldModificationSymbolFactory.CreateSymbol(fieldModificationSyntax, parentNode);
                case FieldGroupExtensionListSyntax fieldGroupExtensionListSyntax:
                    return FieldGroupExtensionListSymbolFactory.CreateSymbol(fieldGroupExtensionListSyntax, parentNode);
                case FieldGroupAddChangeSyntax fieldGroupAddChangeSyntax:
                    return FieldGroupAddChangeSymbolFactory.CreateSymbol(fieldGroupAddChangeSyntax, parentNode);

                // XmlPorts

                case XmlPortSchemaSyntax xmlPortSchemaSyntax:
                    return XmlPortSchemaSymbolFactory.CreateSymbol(xmlPortSchemaSyntax, parentNode);
                case XmlPortTableElementSyntax xmlPortTableElementSyntax:
                    return XmlPortTableElementSymbolFactory.CreateSymbol(xmlPortTableElementSyntax, parentNode);
                case XmlPortFieldElementSyntax xmlPortFieldElementSyntax:
                    return XmlPortFieldElementSymbolFactory.CreateSymbol(xmlPortFieldElementSyntax, parentNode);
                case XmlPortTextElementSyntax xmlPortTextElementSyntax:
                    return XmlPortTextElementSymbolFactory.CreateSymbol(xmlPortTextElementSyntax, parentNode);
                case XmlPortFieldAttributeSyntax xmlPortFieldAttributeSyntax:
                    return XmlPortFieldAttributeSymbolFactory.CreateSymbol(xmlPortFieldAttributeSyntax, parentNode);
                case XmlPortTextAttributeSyntax xmlPortTextAttributeSyntax:
                    return XmlPortTextAttributeSymbolFactory.CreateSymbol(xmlPortTextAttributeSyntax, parentNode);
            }

            return null;
        }


    }
}
