using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Extensions.FileSystemGlobbing;
using AnZwDev.ALTools.ALLanguageInformation;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AddTableDataCaptionFieldsRewriter : ALSyntaxRewriter
    {

        public List<string> FieldNamesPatterns { get; set; } = null;
        public bool SortProperties { get; set; }

        private SortPropertiesSyntaxRewriter _sortPropertiesSyntaxRewriter;

        public AddTableDataCaptionFieldsRewriter()
        {
            _sortPropertiesSyntaxRewriter = new SortPropertiesSyntaxRewriter();
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("DataCaptionFields");
            if (propertySyntax == null)
            {
                var newProperty = CreateDataCaptionFieldsProperty(node);
                if (newProperty != null)
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(newProperty);

                    if (SortProperties)
                        node = node.WithPropertyList(_sortPropertiesSyntaxRewriter.SortPropertyList(node.PropertyList, out _));
                }
            }
            else
            {
                string valueText = propertySyntax.Value.ToString();
                if (String.IsNullOrWhiteSpace(valueText))
                {
                    var newProperty = CreateDataCaptionFieldsProperty(node);
                    if (newProperty != null)
                    {
                        NoOfChanges++;
                        node = node.ReplaceNode(propertySyntax, newProperty);
                    }
                }
            }

            return base.VisitTable(node);
        }


        protected PropertySyntax CreateDataCaptionFieldsProperty(TableSyntax node)
        {
            var propertyValueSyntax = CreateDataCaptionFieldsPropertyValue(node);
            if (propertyValueSyntax != null)
                return CreateDataCaptionFieldsProperty(node, propertyValueSyntax);
            return null;
        }

        protected PropertySyntax CreateDataCaptionFieldsProperty(SyntaxNode node, PropertyValueSyntax propertyValue)
        {
            var propertySyntax = SyntaxFactory.Property("DataCaptionFields", propertyValue);

            /*
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
            propertySyntax = propertySyntax
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
            */

            return propertySyntax;
        }

        protected PropertyValueSyntax CreateDataCaptionFieldsPropertyValue(TableSyntax node)
        {
            var tableName = ALSyntaxHelper.DecodeName(node.Name?.ToString());

            if (tableName == null)
                return null;


            var matcher = new TableFieldsInformationPatternMatcher();
            var collectedFields = matcher.Match(Project, tableName, true, FieldNamesPatterns, true, true, false);

            if (collectedFields.Count > 0)
            {
                var fieldNamesSyntaxList = new List<IdentifierNameSyntax>();
                for (int i = 0; i < collectedFields.Count; i++)
                {
                    var fieldName = collectedFields[i].Name;
                    if (KeywordInformation.IsAnyKeyword(fieldName))
                        fieldName = ALSyntaxHelper.EncodeName(fieldName, true);

                    fieldNamesSyntaxList.Add(SyntaxFactory.IdentifierName(fieldName));
                }

                var fieldNamesSyntaxSeparatedList = new SeparatedSyntaxList<IdentifierNameSyntax>();
                fieldNamesSyntaxSeparatedList = fieldNamesSyntaxSeparatedList.AddRange(fieldNamesSyntaxList);
                return SyntaxFactory.CommaSeparatedPropertyValue(fieldNamesSyntaxSeparatedList);
            }

            return null;
        }

    }
}
