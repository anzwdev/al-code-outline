using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Reflection;

namespace AnZwDev.AL.Syntax
{
    public static class ALSyntaxNodeKindExtensions
    {

        private static Dictionary<ALSyntaxNodeKind, string> _nameCache = new Dictionary<ALSyntaxNodeKind, string>();

        public static string ToDescriptionString(this ALSyntaxNodeKind value)
        {
            if (_nameCache.TryGetValue(value, out var name))
                return name;

            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            var description = attr?.Description ?? value.ToString();
            _nameCache[value] = description;

            return description;
        }

        public static bool IsObjectTypeKind(this ALSyntaxNodeKind kind)
        {
            return
                (kind == ALSyntaxNodeKind.TableObject) ||
                (kind == ALSyntaxNodeKind.CodeunitObject) ||
                (kind == ALSyntaxNodeKind.PageObject) ||
                (kind == ALSyntaxNodeKind.PageExtensionObject) ||
                (kind == ALSyntaxNodeKind.PageCustomizationObject) ||
                (kind == ALSyntaxNodeKind.ReportObject) ||
                (kind == ALSyntaxNodeKind.ReportExtensionObject) ||
                (kind == ALSyntaxNodeKind.XmlPortObject) ||
                (kind == ALSyntaxNodeKind.QueryObject) ||
                (kind == ALSyntaxNodeKind.ControlAddInObject) ||
                (kind == ALSyntaxNodeKind.EnumType) ||
                (kind == ALSyntaxNodeKind.DotNetPackage) ||
                (kind == ALSyntaxNodeKind.Interface) ||
                (kind == ALSyntaxNodeKind.PermissionSet) ||
                (kind == ALSyntaxNodeKind.PermissionSetExtension) ||
                (kind == ALSyntaxNodeKind.EnumExtensionType) ||
                (kind == ALSyntaxNodeKind.TableExtensionObject) ||
                (kind == ALSyntaxNodeKind.ProfileObject) ||
                (kind == ALSyntaxNodeKind.ProfileExtensionObject);
        }

    }
}
