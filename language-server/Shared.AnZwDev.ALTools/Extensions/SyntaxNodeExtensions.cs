using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxNodeExtensions
    {

        public static string GetSyntaxNodeName(this SyntaxNode node)
        {
            return node.GetType().TryGetPropertyValueAsString(node, "Name");
        }

        public static T TryGetValueOf<T>(this SyntaxNode node, string propertyName)
        {
            return node.GetType().TryGetPropertyValue<T>(node, propertyName);
        }

        public static bool HasProperty(this SyntaxNode node, string propertyName)
        {
            return (node.GetProperty(propertyName) != null);
        }

        public static bool HasNonEmptyProperty(this SyntaxNode node, string propertyName, string emptyValue = null)
        {
            PropertySyntax propertySyntax = node.GetProperty(propertyName);
            return ((propertySyntax != null) &&
                (propertySyntax.Value != null) &&
                (!String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())) &&
                (
                    (emptyValue == null) ||
                    (!emptyValue.Equals(propertySyntax.Value.ToString(), StringComparison.CurrentCultureIgnoreCase))));
        }

        public static bool CheckIfPropertyValueEquals(this SyntaxNode node, string propertyName, bool value)
        {
            if (value)
                return node.CheckIfPropertyValueEquals(propertyName, "true");
            return node.CheckIfPropertyValueEquals(propertyName, "false");
        }

        public static bool CheckIfPropertyValueEquals(this SyntaxNode node, string propertyName, string value)
        {
            string propertyValue = node.GetPropertyValue(propertyName)?.ToString()?.Trim();
            return ((propertyValue != null) && (propertyValue.Equals(value, StringComparison.CurrentCultureIgnoreCase)));
        }

        public static SyntaxTriviaList CreateChildNodeIdentTrivia(this SyntaxNode node)
        {
            //calculate indent
            int indentLength = 4;
            string indent;
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                indent = leadingTrivia.ToString();
                int newLinePos = indent.LastIndexOf("/n");
                if (newLinePos >= 0)
                    indent = indent.Substring(newLinePos + 1);
                indentLength += indent.Length;
            }
            indent = "".PadLeft(indentLength);

            return SyntaxFactory.ParseLeadingTrivia(indent, 0);
        }

        public static LabelInformation GetCaptionPropertyInformation(this SyntaxNode node)
        {
            return node.GetLabelPropertyInformation("Caption");
        }

        public static LabelInformation GetLabelPropertyInformation(this SyntaxNode node, string name)
        {
            PropertySyntax propertySyntax = node.GetProperty(name);
            if ((propertySyntax != null) && (propertySyntax.Value != null))
            {
                LabelPropertyValueSyntax labelPropertyValue = propertySyntax.Value as LabelPropertyValueSyntax;
                if ((labelPropertyValue != null) && (labelPropertyValue.Value != null))
                {
                    LabelSyntax labelSyntax = labelPropertyValue.Value;
                    LabelInformation labelInformation = new LabelInformation(name);

                    //get label text
                    if (labelSyntax.LabelText != null)
                        labelInformation.Value = ALSyntaxHelper.DecodeString(labelSyntax.LabelText.ToString());

                    //add property arguments
                    if ((labelSyntax.Properties != null) && (labelSyntax.Properties.Values != null))
                    {
                        foreach (IdentifierEqualsLiteralSyntax labelPropertySyntax in labelSyntax.Properties.Values)
                        {
                            if ((labelPropertySyntax.Identifier != null) && (labelPropertySyntax.Literal != null))
                            {
                                labelInformation.SetProperty(
                                    labelPropertySyntax.Identifier.ToString().Trim(),
                                    ALSyntaxHelper.DecodeStringOrName(labelPropertySyntax.Literal.ToString()));
                            }
                        }
                    }

                    return labelInformation;
                }
            }

            return null;
        }

        public static bool IsInsideCodeBlock(this SyntaxNode node)
        {
            while (node != null)
            {
                var kind = node.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.Block:
                    case ConvertedSyntaxKind.IfStatement:
                    case ConvertedSyntaxKind.ForStatement:
                    case ConvertedSyntaxKind.ForEachStatement:
                    case ConvertedSyntaxKind.RepeatStatement:
                    case ConvertedSyntaxKind.WhileStatement:
                    case ConvertedSyntaxKind.CaseStatement:
                        return true;
                    case ConvertedSyntaxKind.CodeunitObject:
                    case ConvertedSyntaxKind.ControlAddInObject:
                    case ConvertedSyntaxKind.PageObject:
                    case ConvertedSyntaxKind.TableObject:
                    case ConvertedSyntaxKind.ReportObject:
                    case ConvertedSyntaxKind.QueryObject:
                    case ConvertedSyntaxKind.XmlPortObject:
                    case ConvertedSyntaxKind.TableExtensionObject:
                    case ConvertedSyntaxKind.PageExtensionObject:
                    case ConvertedSyntaxKind.PermissionSet:
                    case ConvertedSyntaxKind.PermissionSetExtension:
                    case ConvertedSyntaxKind.EnumType:
                    case ConvertedSyntaxKind.EnumExtensionType:
                    case ConvertedSyntaxKind.ProfileExtensionObject:
                    case ConvertedSyntaxKind.ProfileObject:
                    case ConvertedSyntaxKind.PageCustomizationObject:
                    case ConvertedSyntaxKind.Interface:
                    case ConvertedSyntaxKind.CompilationUnit:
                        return false;
                }
                node = node.Parent;
            }
            return false;
        }

        public static bool HasNonEmptyTrivia(this SyntaxNode node)
        {
            if ((!node.GetLeadingTrivia().IsNullOrWhiteSpace()) || (!node.GetTrailingTrivia().IsNullOrWhiteSpace()))
                return true;

            foreach (var token in node.DescendantTokens())
                if ((!token.LeadingTrivia.IsNullOrWhiteSpace()) || (!token.TrailingTrivia.IsNullOrWhiteSpace()))
                    return true;

            return false;
        }

        public static bool HasNonEmptyTriviaInside(this SyntaxNode node)
        {
            foreach (var token in node.DescendantTokens())
                if ((!token.LeadingTrivia.IsNullOrWhiteSpace()) || (!token.TrailingTrivia.IsNullOrWhiteSpace()))
                    return true;

            return false;
        }

        internal static SyntaxNode FindParentByKind(this SyntaxNode node, params ConvertedSyntaxKind[] parentNodeKind)
        {
            while (node != null)
            {
                ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
                for (int i = 0; i < parentNodeKind.Length; i++)
                    if (parentNodeKind[i] == kind)
                        return node;
                node = node.Parent;
            }
            return null;
        }

        internal static SyntaxNode FindParentApplicationObject(this SyntaxNode node)
        {
            while (node != null)
            {
                if (node.Kind.IsApplicationObject())
                    return node;
                node = node.Parent;
            }
            return null;
        }

        public static SyntaxNode FindNodeOnLeftOfPosition(this SyntaxNode node, int position)
        {
            if (node != null)
            {
                var token = node.FindTokenOnLeftOfPosition(position);
                return token.Parent;
            }
            return null;
        }

#if BC
        public static ConvertedObsoleteState GetObsoleteState(this SyntaxNode node)
        {
            switch (node)
            {
                case MethodDeclarationSyntax methodDeclaration:
                    if (methodDeclaration.Attributes.Any(p => (ALSyntaxHelper.DecodeName(p.Name?.ToString()?.ToLower()) == "obsolete")))
                        return ConvertedObsoleteState.Pending;
                    break;
                default:
                    var propertySyntax = node.GetProperty("ObsoleteState");
                    if (propertySyntax != null)
                    {
                        var value = ALSyntaxHelper.DecodeName(propertySyntax.Value?.ToString());
                        if ((!String.IsNullOrWhiteSpace(value)) && (Enum.TryParse<ConvertedObsoleteState>(value, true, out var state)))
                            return state;
                    }
                    break;
            }

            return ConvertedObsoleteState.None;     
        }

        public static bool IsInsideObsoleteSyntaxTreeBranch(this SyntaxNode node, ConvertedObsoleteState level)
        {
            if (level == ConvertedObsoleteState.None)
                return false;
            while (node != null)
            {
                var nodeLevel = node.GetObsoleteState();
                if (nodeLevel >= level)
                    return true;
                node = node.Parent;
            }
            return false;
        }

#else

        public static bool IsInsideObsoleteSyntaxTreeBranch(this SyntaxNode node, ConvertedObsoleteState level)
        {
            return false;
        }

#endif

        public static IEnumerable<MemberSyntax> GetObjectMembersEnumerable(this SyntaxNode node)
        {
            if (node is ObjectSyntax objectSyntax)
                return objectSyntax.Members;
            return null;
        }

        public static SyntaxNode ReplaceObjectMembers(this SyntaxNode node, SyntaxList<MemberSyntax> members)
        {
            switch (node)
            {
                case TableSyntax tableSyntax:
                    return tableSyntax.WithMembers(members);
                case TableExtensionSyntax tableExtensionSyntax:
                    return tableExtensionSyntax.WithMembers(members);
                case PageSyntax pageSyntax:
                    return pageSyntax.WithMembers(members);
                case PageExtensionSyntax pageExtensionSyntax:
                    return pageExtensionSyntax.WithMembers(members);
                case CodeunitSyntax codeunitSyntax:
                    return codeunitSyntax.WithMembers(members);
                case ReportSyntax reportSyntax:
                    return reportSyntax.WithMembers(members);
#if BC
                case ReportExtensionSyntax reportExtensionSyntax:
                    return reportExtensionSyntax.WithMembers(members);
#endif
                case XmlPortSyntax xmlPortSyntax:
                    return xmlPortSyntax.WithMembers(members);
                case ControlAddInSyntax controlAddInSyntax:
                    return controlAddInSyntax.WithMembers(members);
                case QuerySyntax querySyntax:
                    return querySyntax.WithMembers(members);
            }
            return node;            
        }

        public static IEnumerable<TriggerDeclarationSyntax> GetNodeTriggersEnumerable(this SyntaxNode node)
        {
            var triggersProperty = node.GetType().GetProperty("Triggers");
            if (triggersProperty != null)
                return triggersProperty.GetValue(node) as IEnumerable<TriggerDeclarationSyntax>;
            return null;
        }

        internal static bool IsConvertedSyntaxKind(this SyntaxNode node, params ConvertedSyntaxKind[] kind)
        {
            if (node == null)
                return false;
            var nodeKind = node.Kind.ConvertToLocalType();
            return kind.Contains(nodeKind);
        }

        public static bool GetBoolPropertyValue(this SyntaxNode node, string propertyName, bool defaultValue)
        {
            var stringValue = node.GetPropertyValue(propertyName)?.ToString();
            if (stringValue != null)
                return (stringValue.Equals("true", StringComparison.CurrentCultureIgnoreCase)) || (stringValue == "1");
            return defaultValue;
        }

        public static string GetIdentifierPropertyValue(this SyntaxNode node, string propertyName)
        {
            var stringValue = node.GetPropertyValue(propertyName)?.ToString();
            if (stringValue != null)
                return ALSyntaxHelper.DecodeName(stringValue);
            return null;
        }

        #region Nav2018 helpers

#if NAV2018

        public static PropertySyntax GetProperty(this SyntaxNode node, string name)
        {
            PropertyListSyntax propertyList = node.TryGetValueOf<PropertyListSyntax>(name);
            if (propertyList != null)
            {
                foreach (PropertySyntax property in propertyList.Properties)
                {
                    if (property.Name.Identifier.ValueText == name)
                        return property;
                }
            }
            return null;
        }

        public static PropertyValueSyntax GetPropertyValue(this SyntaxNode node, string name)
        {
            PropertySyntax property = node.GetProperty(name);
            if (property != null)
                return property.Value;
            return null;
        }

        public static string GetNameStringValue(this SyntaxNode node)
        {
            string name = node.GetSyntaxNodeName();
            if (!String.IsNullOrWhiteSpace(name))
                name = ALSyntaxHelper.DecodeName(name);
            return name;
        }

#endif

        #endregion


    }
}
