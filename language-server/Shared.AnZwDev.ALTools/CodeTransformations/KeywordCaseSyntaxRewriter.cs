using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class KeywordCaseSyntaxRewriter : ALSyntaxRewriter
    {

        public KeywordCaseSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPropertyName(PropertyNameSyntax node)
        {
            string prevName = node.Identifier.ValueText;
            if ((!String.IsNullOrWhiteSpace(prevName)) && (Enum.TryParse(prevName, true, out PropertyKind propertyKind)))
            {
                string newName = propertyKind.ToString();
                if (prevName != newName)
                {
                    this.NoOfChanges++;
                    SyntaxToken prevIdentifier = node.Identifier;
                    node = node.WithIdentifier(SyntaxFactory.Identifier(newName).WithTriviaFrom(prevIdentifier));
                }
            }
            return base.VisitPropertyName(node);
        }

        public override SyntaxNode VisitSimpleNamedDataType(SimpleNamedDataTypeSyntax node)
        {
            SyntaxNode syntaxNode = base.VisitSimpleNamedDataType(node);
            node = syntaxNode as SimpleNamedDataTypeSyntax;
            if (node == null)
                return syntaxNode;

            if (!node.ContainsDiagnostics)
            {
                SyntaxToken typeName = node.TypeName;
                string prevName = typeName.ValueText;
                string newName = this.FixTypeNameCase(prevName);
                if (prevName != newName)
                {
                    this.NoOfChanges++;
                    return node.WithTypeName(SyntaxFactory.Identifier(newName).WithTriviaFrom(typeName));
                }
            }

            return node;
        }

        public override SyntaxNode VisitLengthDataType(LengthDataTypeSyntax node)
        {
            SyntaxNode syntaxNode = base.VisitLengthDataType(node);
            node = syntaxNode as LengthDataTypeSyntax;
            if (node == null)
                return syntaxNode;

            if (!node.ContainsDiagnostics)
            {
                SyntaxToken typeName = node.TypeName;
                string prevName = typeName.ValueText;
                string newName = this.FixTypeNameCase(prevName);
                if (prevName != newName)
                {
                    this.NoOfChanges++;
                    return node.WithTypeName(SyntaxFactory.Identifier(newName).WithTriviaFrom(typeName));
                }
            }

            return node;
        }

        public override SyntaxNode VisitGenericNamedDataType(GenericNamedDataTypeSyntax node)
        {
            SyntaxNode syntaxNode = base.VisitGenericNamedDataType(node);
            node = syntaxNode as GenericNamedDataTypeSyntax;
            if (node == null)
                return syntaxNode;

            if (!node.ContainsDiagnostics)
            {
                SyntaxToken typeName = node.TypeName;
                string prevName = typeName.ValueText;
                string newName = this.FixTypeNameCase(prevName);
                if (prevName != newName)
                {
                    this.NoOfChanges++;
                    return node.WithTypeName(SyntaxFactory.Identifier(newName).WithTriviaFrom(typeName));
                }
            }

            return node;
        }

        public override SyntaxNode VisitSubtypedDataType(SubtypedDataTypeSyntax node)
        {
            SyntaxNode syntaxNode = base.VisitSubtypedDataType(node);
            node = syntaxNode as SubtypedDataTypeSyntax;
            if (node == null)
                return syntaxNode;
            
            if (!node.ContainsDiagnostics)
            {
                SyntaxToken typeName = node.TypeName;
                string prevName = typeName.ValueText;
                string newName = this.FixTypeNameCase(prevName);
                if (prevName != newName)
                {
                    this.NoOfChanges++;
                    return node.WithTypeName(SyntaxFactory.Identifier(newName).WithTriviaFrom(typeName));
                }
            }

            return node;
        }

        protected string FixTypeNameCase(string name)
        {
            if (Enum.TryParse(name, true, out NavTypeKind typeKind))
                return typeKind.ToString();
            return name;
        }

        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            if (token.Kind.IsKeyword())
            {
                string prevSource = token.ToString().Trim();

                SyntaxTriviaList leadingTrivia = token.LeadingTrivia;
                SyntaxTriviaList trailingTrivia = token.TrailingTrivia;
                bool parsed = false;

#if BC
                ConvertedSyntaxKind kind = token.Kind.ConvertToLocalType();
                SyntaxNode parentToken = token.Parent;
                ConvertedSyntaxKind parentKind = parentToken.Kind.ConvertToLocalType();
                if ((parentKind == ConvertedSyntaxKind.SubtypedDataType) || (parentKind == ConvertedSyntaxKind.EnumDataType) || (parentKind == ConvertedSyntaxKind.LabelDataType))
                {
                    switch (kind)
                    {
                        case ConvertedSyntaxKind.CodeunitKeyword:
                            token = SyntaxFactory.ParseKeyword("Codeunit");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.PageKeyword:
                            token = SyntaxFactory.ParseKeyword("Page");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.XmlPortKeyword:
                            token = SyntaxFactory.ParseKeyword("XmlPort");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.QueryKeyword:
                            token = SyntaxFactory.ParseKeyword("Query");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.ReportKeyword:
                            token = SyntaxFactory.ParseKeyword("Report");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.InterfaceKeyword:
                            token = SyntaxFactory.ParseKeyword("Interface");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.DotNetKeyword:
                            token = SyntaxFactory.ParseKeyword("DotNet");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.ControlAddInKeyword:
                            token = SyntaxFactory.ParseKeyword("ControlAddIn");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.LabelKeyword:
                            token = SyntaxFactory.ParseKeyword("Label");
                            parsed = true;
                            break;
                        case ConvertedSyntaxKind.EnumKeyword:
                            token = SyntaxFactory.ParseKeyword("Enum");
                            parsed = true;
                            break;
                    }
                }
#endif

                if (!parsed)
                    token = SyntaxFactory.Token(token.Kind);
                token = token.WithLeadingTrivia(leadingTrivia).WithTrailingTrivia(trailingTrivia);

                string newSource = token.ToString().Trim();
                if (prevSource != newSource)
                    this.NoOfChanges++;
            }
            return base.VisitToken(token);
        }

    }
}
