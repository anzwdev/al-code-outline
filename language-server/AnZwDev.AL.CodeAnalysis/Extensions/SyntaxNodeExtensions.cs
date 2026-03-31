using AnZwDev.AL.Syntax;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class SyntaxNodeExtensions
    {

        public static T? GetParentOfType<T>(this SyntaxNode node) where T : SyntaxNode
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (parent is T)
                    return (T)parent;
                parent = parent.Parent;
            }
            return null;
        }

        public static (bool, TextSpan) GetChildNodesFullSpan(this SyntaxNode node)
        {
            int start = 0;
            int end = 0;
            bool empty = true;

            var childNodes = node.ChildNodes();
            foreach (var child in childNodes)
            {
                var span = child.FullSpan;
                if (empty)
                {
                    start = span.Start;
                    end = span.End;
                    empty = false;
                } 
                else
                {
                    start = Math.Min(start, span.Start);
                    end = Math.Max(end, span.End);
                }
            }

            return (!empty, new TextSpan(start, end - start));
        }

        public static bool HasNonEmptyProperty(this SyntaxNode node, string propertyName, string? emptyValue = null)
        {
            var propertySyntax = node.GetProperty(propertyName);
            return ((propertySyntax != null) &&
                (propertySyntax.Value != null) &&
                (!String.IsNullOrWhiteSpace(propertySyntax.Value.ToString())) &&
                (
                    (emptyValue == null) ||
                    (!emptyValue.Equals(propertySyntax.Value.ToString(), StringComparison.OrdinalIgnoreCase))));
        }

        public static bool GetBoolPropertyValue(this SyntaxNode node, string propertyName, bool defaultValue)
        {
            var stringValue = node.GetPropertyValue(propertyName)?.ToString();
            if (stringValue != null)
                return stringValue.Equals("true", StringComparison.OrdinalIgnoreCase) || stringValue == "1";
            return defaultValue;
        }

        public static string? GetStringPropertyValue(this SyntaxNode node, string propertyName)
        {
            return node.GetPropertyValue(propertyName)?.ToString();
        }

        public static string? GetDecodedNamePropertyValue(this SyntaxNode node, string propertyName)
        {
            var value = node.GetStringPropertyValue(propertyName);
            if (value != null)
                return ALLiteralParser.ParseName(value);
            return value;
        }

        public static string? GetSyntaxNodeName(this SyntaxNode node)
        {
            return node.GetType().TryGetPropertyValueAsString(node, "Name");
        }


        public static SyntaxNode? FindParentByKind(this SyntaxNode node, params SyntaxKind[] parentNodeKind)
        {
            while (node != null)
            {
                var kind = node.Kind;
                for (int i = 0; i < parentNodeKind.Length; i++)
                    if (parentNodeKind[i] == kind)
                        return node;
                node = node.Parent;
            }
            return null;
        }

        public static SyntaxNode? FindParentApplicationObject(this SyntaxNode node)
        {
            while (node != null)
            {
                if (node.Kind.IsApplicationObject())
                    return node;
                node = node.Parent;
            }
            return null;
        }

        public static bool HasProperty(this SyntaxNode node, string propertyName)
        {
            return (node.GetProperty(propertyName) != null);
        }


        public static SyntaxTriviaList CreateChildNodeIdentTrivia(this SyntaxNode node)
        {
            //calculate indent
            int indentLength = 4;
            string indent;
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();

            if (leadingTrivia.Count > 0)
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

        public static T WithTrailingNewLine<T>(this T node) where T : SyntaxNode
        {
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
            return node.WithTrailingTrivia(trailingTriviaList);
        }

        public static bool HasParents(this SyntaxNode node, params SyntaxKind[] parentNodeKind)
        {
            int nodeKindIndex = 0;
            while (node != null)
            {
                if (node.Kind == parentNodeKind[nodeKindIndex])
                {
                    nodeKindIndex++;
                    if (nodeKindIndex >= parentNodeKind.Length)
                        return true;
                }
                node = node.Parent;
            }
            return false;
        }

        public static bool CheckIfPropertyValueEquals(this SyntaxNode node, string propertyName, bool value)
        {
            if (value)
                return node.CheckIfPropertyValueEquals(propertyName, "true");
            return node.CheckIfPropertyValueEquals(propertyName, "false");
        }

        public static bool CheckIfPropertyValueEquals(this SyntaxNode node, string propertyName, string value)
        {
            var propertyValue = node.GetPropertyValue(propertyName)?.ToString()?.Trim();
            return ((propertyValue != null) && (propertyValue.Equals(value, StringComparison.OrdinalIgnoreCase)));
        }

        public static string? GetIdentifierPropertyValue(this SyntaxNode node, string propertyName)
        {
            var stringValue = node.GetPropertyValue(propertyName)?.ToString();
            if (stringValue != null)
                return ALLiteralParser.ParseName(stringValue);
            return null;
        }

        public static bool IsObsoletePendingOrRemoved(this SyntaxNode node)
        {
            var property = node.GetIdentifierPropertyValue("ObsoleteState");
            return (!String.IsNullOrWhiteSpace(property)) &&
                ((property.Equals("Pending", StringComparison.OrdinalIgnoreCase)) || (property.Equals("Removed", StringComparison.OrdinalIgnoreCase)));
        }

        public static IEnumerable<MemberSyntax>? GetObjectMembersEnumerable(this SyntaxNode node)
        {
            if (node is ObjectSyntax objectSyntax)
                return objectSyntax.Members;
            return null;
        }

        public static IEnumerable<TriggerDeclarationSyntax>? GetNodeTriggersEnumerable(this SyntaxNode node)
        {
            var triggersProperty = node.GetType().GetProperty("Triggers");
            if (triggersProperty != null)
                return triggersProperty.GetValue(node) as IEnumerable<TriggerDeclarationSyntax>;
            return null;
        }

    }
}
