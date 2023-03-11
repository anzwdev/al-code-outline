using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AddDropDownFieldGroupsRewriter : ALSyntaxRewriter
    {

        public List<string> FieldNamesPatterns { get; set; } = null;

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            var fieldGroup = GetDropDownFieldGroup(node);
            if (fieldGroup == null)
            {
                var tableName = ALSyntaxHelper.DecodeName(node.Name?.ToString());

                if (tableName != null)
                {
                    var matcher = new TableFieldsInformationPatternMatcher();
                    var collectedFields = matcher.Match(Project, tableName, true, FieldNamesPatterns, true, true, false);

                    if (collectedFields.Count > 0)
                    {
                        var fieldNamesSyntaxList = new List<IdentifierNameSyntax>();
                        for (int i = 0; i < collectedFields.Count; i++)
                            fieldNamesSyntaxList.Add(SyntaxFactory.IdentifierName(collectedFields[i].Name));

                        var fieldNamesSyntaxSeparatedList = new SeparatedSyntaxList<IdentifierNameSyntax>();
                        fieldNamesSyntaxSeparatedList = fieldNamesSyntaxSeparatedList.AddRange(fieldNamesSyntaxList);
                        var dropDownGroup = SyntaxFactory.FieldGroup("DropDown").WithFields(fieldNamesSyntaxSeparatedList);

                        var newFieldGroups = (node.FieldGroups == null)?
                            SyntaxFactory.FieldGroupList(SyntaxFactory.List(dropDownGroup)):
                            node.FieldGroups.AddFieldGroups(dropDownGroup);

                        node = node.WithFieldGroups(newFieldGroups);

                        NoOfChanges++;
                    }
                }
            }

            return base.VisitTable(node);
        }

        private FieldGroupSyntax GetDropDownFieldGroup(TableSyntax tableSyntax)
        {
            return tableSyntax.FieldGroups?.FieldGroups
                .Where(p => (p.Name?.ToString() == "DropDown"))
                .FirstOrDefault();
        }

    }
}
