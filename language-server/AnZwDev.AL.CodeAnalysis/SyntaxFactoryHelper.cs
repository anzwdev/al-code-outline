using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis
{
    public static class SyntaxFactoryHelper
    {

        public static IdentifierEqualsLiteralSyntax IdentifierEqualsLiteral(string identifier, string value)
        {
            return SyntaxFactory.IdentifierEqualsLiteral(identifier, SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(value)));
        }

        public static IdentifierEqualsLiteralSyntax IdentifierEqualsLiteral(string identifier, bool value)
        {
            SyntaxKind tokenKind = value ? SyntaxKind.TrueKeyword : SyntaxKind.FalseKeyword;
            return SyntaxFactory.IdentifierEqualsLiteral(identifier, SyntaxFactory.BooleanLiteralValue(SyntaxFactory.Token(tokenKind)));
        }

        public static LabelSyntax Label(string labelText, string comment, bool locked)
        {
            StringLiteralValueSyntax labelTextSyntax = SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(labelText));

            List<IdentifierEqualsLiteralSyntax> propertiesList = new List<IdentifierEqualsLiteralSyntax>();
            if (!String.IsNullOrWhiteSpace(comment))
                propertiesList.Add(SyntaxFactoryHelper.IdentifierEqualsLiteral("Comment", comment));
            if (locked)
                propertiesList.Add(SyntaxFactoryHelper.IdentifierEqualsLiteral("Locked", true));

            if (propertiesList.Count > 0)
            {
                SeparatedSyntaxList<IdentifierEqualsLiteralSyntax> separatedSyntaxList = new SeparatedSyntaxList<IdentifierEqualsLiteralSyntax>();
                separatedSyntaxList = separatedSyntaxList.AddRange(propertiesList);
                CommaSeparatedIdentifierEqualsLiteralListSyntax propertiesListSyntax = SyntaxFactory.CommaSeparatedIdentifierEqualsLiteralList(separatedSyntaxList);
                return SyntaxFactory.Label(
                    labelTextSyntax,
                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                    propertiesListSyntax);
            }
            return SyntaxFactory.Label(labelTextSyntax);
        }

        public static PropertySyntax LabelProperty(PropertyKind propertyKind, string labelText, string? comment, bool locked)
        {
            var propertyValue = SyntaxFactory.LabelPropertyValue(SyntaxFactoryHelper.Label(labelText, comment ?? String.Empty, locked));
            return SyntaxFactory.Property(propertyKind, propertyValue);
        }

        public static PropertySyntax CaptionProperty(string labelText, string? comment, bool locked)
        {
            return SyntaxFactoryHelper.LabelProperty(PropertyKind.Caption, labelText, comment, locked);
        }

        public static PropertySyntax ToolTipProperty(string labelText, string? comment, bool locked)
        {
            return SyntaxFactoryHelper.LabelProperty(PropertyKind.ToolTip, labelText, comment, locked);
        }

    }
}
