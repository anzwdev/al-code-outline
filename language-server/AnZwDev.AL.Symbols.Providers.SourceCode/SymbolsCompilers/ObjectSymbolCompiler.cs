using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ObjectSymbolCompiler
    {

        public static ObjectSymbol? Compile(ObjectSyntax objectSyntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            switch (objectSyntax)
            {
                case TableSyntax tableSyntax:
                    return TableSymbolCompiler.Compile(tableSyntax, namespaceName, usings, sourceFileName);
                case CodeunitSyntax codeunitSyntax:
                    return CodeunitSymbolCompiler.Compile(codeunitSyntax, namespaceName, usings, sourceFileName);
                case PageSyntax pageSyntax:
                    return PageSymbolCompiler.Compile(pageSyntax, namespaceName, usings, sourceFileName);
                case PageExtensionSyntax pageExtensionSyntax:
                    return PageExtensionSymbolCompiler.Compile(pageExtensionSyntax, namespaceName, usings, sourceFileName);
                case PageCustomizationSyntax pageCustomizationSyntax:
                    return PageCustomizationSymbolCompiler.Compile(pageCustomizationSyntax, namespaceName, usings, sourceFileName);
                case ReportSyntax reportSyntax:
                    return ReportSymbolCompiler.Compile(reportSyntax, namespaceName, usings, sourceFileName);
                case ReportExtensionSyntax reportExtensionSyntax:
                    return ReportExtensionSymbolCompiler.Compile(reportExtensionSyntax, namespaceName, usings, sourceFileName);
                case XmlPortSyntax xmlPortSyntax:
                    return XmlPortSymbolCompiler.Compile(xmlPortSyntax, namespaceName, usings, sourceFileName);
                case QuerySyntax querySyntax:
                    return QuerySymbolCompiler.Compile(querySyntax, namespaceName, usings, sourceFileName);
                case ControlAddInSyntax controlAddInSyntax:
                    return ControlAddInSymbolCompiler.Compile(controlAddInSyntax, namespaceName, usings, sourceFileName);
                case EnumTypeSyntax enumTypeSyntax:
                    return EnumTypeSymbolCompiler.Compile(enumTypeSyntax, namespaceName, usings, sourceFileName);
                case DotNetPackageSyntax dotNetPackageSyntax:
                    return DotNetPackageSymbolCompiler.Compile(dotNetPackageSyntax, namespaceName, usings, sourceFileName);
                case InterfaceSyntax interfaceSyntax:
                    return InterfaceSymbolCompiler.Compile(interfaceSyntax, namespaceName, usings, sourceFileName);
                case PermissionSetSyntax permissionSetSyntax:
                    return PermissionSetSymbolCompiler.Compile(permissionSetSyntax, namespaceName, usings, sourceFileName);
                case PermissionSetExtensionSyntax permissionSetExtensionSyntax:
                    return PermissionSetExtensionSymbolCompiler.Compile(permissionSetExtensionSyntax, namespaceName, usings, sourceFileName);
                case EnumExtensionTypeSyntax enumExtensionTypeSyntax:
                    return EnumExtensionTypeSymbolCompiler.Compile(enumExtensionTypeSyntax, namespaceName, usings, sourceFileName);
                case TableExtensionSyntax tableExtensionSyntax:
                    return TableExtensionSymbolCompiler.Compile(tableExtensionSyntax, namespaceName, usings, sourceFileName);
                case ProfileSyntax profileSyntax:
                    return ProfileSymbolCompiler.Compile(profileSyntax, namespaceName, usings, sourceFileName);
                case ProfileExtensionSyntax profileExtensionSyntax:
                    return ProfileExtensionSymbolCompiler.Compile(profileExtensionSyntax, namespaceName, usings, sourceFileName);
            }

            return null;
        }

    }
}
