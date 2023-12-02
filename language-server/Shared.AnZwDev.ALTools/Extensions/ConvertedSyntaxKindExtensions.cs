using AnZwDev.ALTools.ALSymbols.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    internal static class ConvertedSyntaxKindExtensions
    {

        public static bool IsApplicationObject(this ConvertedSyntaxKind kind)
        {
            switch (kind)
            {
                case ConvertedSyntaxKind.TableObject:
                case ConvertedSyntaxKind.CodeunitObject:
                case ConvertedSyntaxKind.PageObject:
                case ConvertedSyntaxKind.ReportObject:
                case ConvertedSyntaxKind.QueryObject:
                case ConvertedSyntaxKind.XmlPortObject:
                case ConvertedSyntaxKind.PageExtensionObject:
                case ConvertedSyntaxKind.TableExtensionObject:
                case ConvertedSyntaxKind.ProfileObject:
                case ConvertedSyntaxKind.ControlAddInObject:
                case ConvertedSyntaxKind.DotNetTypeDeclaration:
                case ConvertedSyntaxKind.PageCustomizationObject:
                case ConvertedSyntaxKind.EnumDataType:
                case ConvertedSyntaxKind.EnumExtensionType:
                case ConvertedSyntaxKind.Interface:
                case ConvertedSyntaxKind.PermissionSet:
                case ConvertedSyntaxKind.PermissionSetExtension:
                    return true;
            }
            return false;
        }

    }
}
