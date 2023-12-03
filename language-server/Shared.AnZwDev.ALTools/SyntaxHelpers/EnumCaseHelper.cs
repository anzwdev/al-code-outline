using Microsoft.Dynamics.Nav.CodeAnalysis.Semantics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.SyntaxHelpers
{

#if BC

    internal static class EnumCaseHelper
    {

        public static List<string> GetMissingEnumCaseValues(CaseStatementSyntax caseStatementSyntax, SemanticModel semanticModel, bool ignoreElse)
        {
            var operation = semanticModel.GetOperation(caseStatementSyntax) as ICaseStatement;
            if (operation != null)
                return GetMissingEnumCaseValues(operation, ignoreElse);
            return null;
        }

        public static List<string> GetMissingEnumCaseValues(ICaseStatement caseStatement, bool ignoreElse)
        {
            if ((!ignoreElse) && (caseStatement.ElseStatement != null))
                return null;

            switch (caseStatement.Value?.Type)
            {
                case IEnumTypeSymbol enumTypeSymbol:
                    (var missingEnums, _) = GetMissingEnumCaseValues(caseStatement, enumTypeSymbol);
                    return missingEnums;
                case IOptionTypeSymbol optionTypeSymbol:
                    (var missingOptions, _) = GetMissingOptionCaseValues(caseStatement, optionTypeSymbol);
                    return missingOptions;
            }

            return null;
        }

        public static (List<string>, CodeExpressionSyntax) GetMissingEnumCaseValues(ICaseStatement caseStatement, IEnumTypeSymbol caseExpressionType)
        {
            CodeExpressionSyntax enumInstanceSyntax = null;

            //collect all enum values
            HashSet<string> missingEnumValues = new HashSet<string>();
            foreach (var enumValue in caseExpressionType.Values)
                missingEnumValues.Add(enumValue.Name);

            //remove existing enum values
            foreach (var line in caseStatement.CaseLines)
            {
                if (line.Expressions != null)
                    foreach (var expression in line.Expressions)
                    {
                        var optionAccessExpression = expression as IOptionAccess;

                        if (enumInstanceSyntax == null)
                        {
                            var lineInstanceSyntax = optionAccessExpression.Instance.Syntax as CodeExpressionSyntax;
                            if (lineInstanceSyntax != null)
                                enumInstanceSyntax = lineInstanceSyntax;
                        }
                                
                        if (optionAccessExpression != null)
                        {
                            var enumValueName = (optionAccessExpression.OptionSymbol as IEnumValueSymbol)?.Name;
                            if ((!String.IsNullOrEmpty(enumValueName)) && (missingEnumValues.Contains(enumValueName)))
                                missingEnumValues.Remove(enumValueName);
                        }
                    }
            }

            //return null if case statement expression can be used as enum source in the case lines
            return (missingEnumValues.ToList(), enumInstanceSyntax);
        }

        public static (List<string>, CodeExpressionSyntax) GetMissingOptionCaseValues(ICaseStatement caseStatement, IOptionTypeSymbol caseExpressionType)
        {
            CodeExpressionSyntax optionInstanceSyntax = null;

            //collect all enum values
            HashSet<string> missingEnumValues = new HashSet<string>();
            foreach (var enumValue in caseExpressionType.Values)
                missingEnumValues.Add(enumValue.Name);

            //remove existing enum values
            foreach (var line in caseStatement.CaseLines)
            {
                if (line.Expressions != null)
                    foreach (var expression in line.Expressions)
                    {
                        var optionAccess = (expression as IConversionExpression).Operand as IOptionAccess;
                        if (optionAccess != null)
                        {
                            if (optionInstanceSyntax == null)
                            {
                                var lineInstanceSyntax = optionAccess.Instance.Syntax as CodeExpressionSyntax;
                                if (lineInstanceSyntax != null)
                                    optionInstanceSyntax = lineInstanceSyntax;
                            }

                            var valueName = optionAccess.OptionSymbol.Name;
                            if ((!String.IsNullOrEmpty(valueName)) && (missingEnumValues.Contains(valueName)))
                                missingEnumValues.Remove(valueName);
                        }
                    }
            }

            return (missingEnumValues.ToList(), optionInstanceSyntax);
        }

    }

#endif
}
