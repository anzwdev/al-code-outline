using AnZwDev.ALTools.SyntaxHelpers;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Semantics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class AddMissingCaseLinesSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {

        public bool IgnoreElseStatement { get; set; }

        public AddMissingCaseLinesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitCaseStatement(CaseStatementSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
            {
                var caseStatementOperation = SemanticModel.GetOperation(node) as ICaseStatement;

                if ((caseStatementOperation != null) && ((!IgnoreElseStatement) || (caseStatementOperation.ElseStatement == null)))
                {

                    switch (caseStatementOperation.Value?.Type)
                    {
                        case IEnumTypeSymbol enumTypeSymbol:
                            node = AddMissingEnumValues(node, caseStatementOperation, enumTypeSymbol);
                            break;
                        case IOptionTypeSymbol optionTypeSymbol:
                            node = AddMissingOptionValues(node, caseStatementOperation, optionTypeSymbol);
                            break;
                    }
                }

            }

            return base.VisitCaseStatement(node);
        }

        private CaseStatementSyntax AddMissingEnumValues(CaseStatementSyntax node, ICaseStatement caseStatement, IEnumTypeSymbol expressionType)
        {
            (var missingValuesCollection, var enumInstance) = EnumCaseHelper.GetMissingEnumCaseValues(caseStatement, expressionType);
            return AddEnumOrOptionValues(node, missingValuesCollection, enumInstance);
        }

        private CaseStatementSyntax AddMissingOptionValues(CaseStatementSyntax node, ICaseStatement caseStatement, IOptionTypeSymbol expressionType)
        {
            (var missingValuesCollection, var optionInstance) = EnumCaseHelper.GetMissingOptionCaseValues(caseStatement, expressionType);
            return AddEnumOrOptionValues(node, missingValuesCollection, optionInstance);
        }

        private CaseStatementSyntax AddEnumOrOptionValues(CaseStatementSyntax node, List<string> missingValuesCollection, CodeExpressionSyntax optionOrEnumInstance)
        {
            if ((missingValuesCollection != null) && (missingValuesCollection.Count > 0))
            {
                if (optionOrEnumInstance == null)
                    optionOrEnumInstance = node.Expression;

                var caseLines = node.CaseLines;
                foreach (var missingValue in missingValuesCollection)
                {
                    var enumOrOptionValue = SyntaxFactory.OptionAccessExpression(
                        optionOrEnumInstance,
                        SyntaxFactory.IdentifierName(missingValue));
                    var caseLineStatement = ExpressionFactory.NotSupportedExceptionStatement("Case condition %1 not implemented.", enumOrOptionValue);

                    var missingCaseLine = SyntaxFactory.CaseLine(caseLineStatement);
                    missingCaseLine = missingCaseLine.AddExpressions(enumOrOptionValue);

                    missingCaseLine = missingCaseLine
                        .WithLeadingTrivia(SyntaxFactory.EndOfLine(""));

                    caseLines = caseLines.Add(missingCaseLine);
                    NoOfChanges++;
                }

                node = node.WithCaseLines(caseLines);
            }

            return node;
        }


    }

#endif

}
