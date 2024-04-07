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
    public class AddTableDataCaptionFieldsRewriter : ALSyntaxRewriterWithNamespaces
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
                var newProperty = CreateDataCaptionFieldsProperty(node, propertySyntax);
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
                    var newProperty = CreateDataCaptionFieldsProperty(node, propertySyntax);
                    if (newProperty != null)
                    {
                        NoOfChanges++;
                        node = node.ReplaceNode(propertySyntax, newProperty);
                    }
                }
            }

            return base.VisitTable(node);
        }


        protected PropertySyntax CreateDataCaptionFieldsProperty(TableSyntax node, PropertySyntax existingProperty)
        {
            var propertyValueSyntax = CreateDataCaptionFieldsPropertyValue(node);
            if (propertyValueSyntax != null)
                return CreateDataCaptionFieldsProperty(node, propertyValueSyntax, existingProperty);
            return null;
        }

        protected PropertySyntax CreateDataCaptionFieldsProperty(SyntaxNode node, PropertyValueSyntax propertyValue, PropertySyntax existingProperty)
        {
            var propertySyntax = SyntaxFactory.Property("DataCaptionFields", propertyValue);

            if (existingProperty != null)
                propertySyntax = propertySyntax.WithTriviaFrom(existingProperty);
            else
                propertySyntax = propertySyntax
                    .WithLeadingTrivia(node.CreateChildNodeIdentTrivia())
                    .WithTrailingNewLine();

            return propertySyntax;
        }

        protected PropertyValueSyntax CreateDataCaptionFieldsPropertyValue(TableSyntax node)
        {
            //!!! TO-DO !!!
            //!!! Check if it works !!!
            var tableIdentifier = new ALObjectIdentifier(NamespaceName, node.ObjectId?.Value.ValueText, node.Name?.Identifier.ValueText);

            if (!tableIdentifier.IsEmpty())
            {
                var matcher = new TableFieldsInformationPatternMatcher();
                var collectedFields = matcher.Match(Project, tableIdentifier.ToObjectReference(), true, FieldNamesPatterns, true, true, false);

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
            }

            return null;
        }

    }
}
