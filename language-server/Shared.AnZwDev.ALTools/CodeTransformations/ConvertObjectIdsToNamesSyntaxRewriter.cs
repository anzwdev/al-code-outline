﻿using AnZwDev.ALTools.ALLanguageInformation;
using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{


#if BC
    public class ConvertObjectIdsToNamesSyntaxRewriter : ALSemanticModelSyntaxRewriter
#else
    public class ConvertObjectIdsToNamesSyntaxRewriter : ALSyntaxRewriter
#endif
    {

        public ConvertObjectIdsToNamesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitMemberAttribute(MemberAttributeSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                string name = node.Name?.Identifier.ValueText;
                if ((!String.IsNullOrWhiteSpace(name)) && (name.Equals("EventSubscriber", StringComparison.CurrentCultureIgnoreCase)) && (node.ArgumentList != null))
                {
                    SeparatedSyntaxList<AttributeArgumentSyntax> arguments = node.ArgumentList.Arguments;
                    if ((arguments != null) && (arguments.Count >= 2))
                    {
                        OptionAccessAttributeArgumentSyntax objectTypeArgument = arguments[0] as OptionAccessAttributeArgumentSyntax;
                        LiteralAttributeArgumentSyntax objectNameOrIdArgument = arguments[1] as LiteralAttributeArgumentSyntax;
                        if ((objectTypeArgument != null) && (objectNameOrIdArgument != null))
                        {
                            string objectType = ALSyntaxHelper.DecodeName(objectTypeArgument?.OptionAccess?.Name?.ToString());
                            string prevValue = objectNameOrIdArgument.ToString();
                            string newValue = prevValue;
                            if (Int32.TryParse(prevValue, out int intValue))
                            {
                                ALAppObject alAppObject = this.FindObjectById(objectType, intValue);
                                if (alAppObject != null)
                                    newValue = alAppObject.Name;
                            }

                            if ((prevValue != newValue) && (!String.IsNullOrWhiteSpace(newValue)))
                            {
                                this.NoOfChanges++;
                                OptionAccessAttributeArgumentSyntax newObjectNameOrIdArgument = SyntaxFactory.OptionAccessAttributeArgument(
                                    SyntaxFactory.OptionAccessExpression(
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(this.ObjectTypeNameToEnumName(objectType))),
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(newValue)))).WithTriviaFrom(objectNameOrIdArgument);
                                AttributeArgumentListSyntax argumentsList = node.ArgumentList.WithArguments(arguments.Replace(objectNameOrIdArgument, newObjectNameOrIdArgument));
                                node = node.WithArgumentList(argumentsList);
                            }

                        }
                    }
                }
            }

            return base.VisitMemberAttribute(node);
        }

        public override SyntaxNode VisitObjectNameOrId(ObjectNameOrIdSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                if (node.Identifier is ObjectIdSyntax objectIdSyntax)
                {
                    string idText = objectIdSyntax.Value.ValueText;
                    string newName = idText;
                    if (Int32.TryParse(idText, out int idValue))
                    {
                        string objectType = this.FindObjectTypeForObjectNameOrId(node);
                        if (!String.IsNullOrWhiteSpace(objectType))
                        {
                            ALAppObject alAppObject = this.FindObjectById(objectType, idValue);
                            if (alAppObject != null)
                                newName = alAppObject.Name;
                        }
                    }

                    if ((newName != idText) && (!String.IsNullOrWhiteSpace(newName)))
                    {
                        this.NoOfChanges++;
                        SyntaxToken objectNameValue = SyntaxFactory.Identifier(newName).WithTriviaFrom(objectIdSyntax.Value);
                        IdentifierNameSyntax objectName = SyntaxFactory.IdentifierName(objectNameValue).WithTriviaFrom(objectIdSyntax);
                        ObjectNameOrIdSyntax newNode = SyntaxFactory.ObjectNameOrId(objectName).WithTriviaFrom(node);
                        return newNode;
                    }
                }
            }

            return base.VisitObjectNameOrId(node);
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                var objectIdParameters = GetObjectIdMethodParameter(node);
                if (objectIdParameters != null)
                    node = ReplaceIdParametersWithNames(node, objectIdParameters);
            }

            return base.VisitInvocationExpression(node);
        }

#if BC
        public override SyntaxNode VisitAssignmentStatement(AssignmentStatementSyntax node)
        {
            if (!node.ContainsDiagnostics)
            {
                var operation = SemanticModel.GetOperation(node);

                if (operation is IInvocationExpression invocationExpression)
                {
                    var objectIdParameters = GetObjectIdInvocationExpressionParameter(invocationExpression);
                    if ((objectIdParameters != null) && (objectIdParameters.Length > 0) && (objectIdParameters[0].Index == 0))
                        node = ReplaceIdAssignmentWithName(node, objectIdParameters[0].ObjectType);
                }
            }

            return base.VisitAssignmentStatement(node);
        }
#endif

        private AssignmentStatementSyntax ReplaceIdAssignmentWithName(AssignmentStatementSyntax node, string objectType)
        {
            if (node.Source is LiteralExpressionSyntax sourceSyntax)
            {
                if (sourceSyntax.Literal is Int32SignedLiteralValueSyntax intLiteralSyntax)
                {
                    int objectId;
                    if (Int32.TryParse(intLiteralSyntax.Number.ValueText, out objectId))
                    {
                        if (objectId != 0)
                        {
                            ALAppObject alAppObject = this.FindObjectById(objectType, objectId);
                            string objectName = alAppObject?.Name;
                            if (!String.IsNullOrWhiteSpace(objectName))
                            {
                                this.NoOfChanges++;
                                CodeExpressionSyntax newSourceSyntax = SyntaxFactory.OptionAccessExpression(
                                    SyntaxFactory.IdentifierName(this.ObjectTypeNameToEnumName(objectType)),
                                    SyntaxFactory.IdentifierName(objectName)).WithTriviaFrom(sourceSyntax);
                                node = node.WithSource(newSourceSyntax);
                            }
                        }
                    }
                }
            }

            return node;
        }

        private InvocationExpressionSyntax ReplaceIdParametersWithNames(InvocationExpressionSyntax node, ObjectIdParameterInformation[] parameter)
        {
            for (int i=0; i<parameter.Length; i++)
                node = ReplaceIdParameterWithName(node, parameter[i]);
            return node;
        }

        private InvocationExpressionSyntax ReplaceIdParameterWithName(InvocationExpressionSyntax node, ObjectIdParameterInformation parameter)
        {
            if ((node.ArgumentList.Arguments.Count > parameter.Index) && (node.ArgumentList.Arguments[parameter.Index] is LiteralExpressionSyntax argumentSyntax))
            {
                if (argumentSyntax.Literal is Int32SignedLiteralValueSyntax intLiteralSyntax)
                {
                    int objectId;
                    if (Int32.TryParse(intLiteralSyntax.Number.ValueText, out objectId))
                    {
                        if (objectId != 0)
                        {
                            ALAppObject alAppObject = this.FindObjectById(parameter.ObjectType, objectId);
                            string objectName = alAppObject?.Name;
                            if (!String.IsNullOrWhiteSpace(objectName))
                            {
                                this.NoOfChanges++;
                                CodeExpressionSyntax newArgumentSyntax = SyntaxFactory.OptionAccessExpression(
                                    SyntaxFactory.IdentifierName(this.ObjectTypeNameToEnumName(parameter.ObjectType)),
                                    SyntaxFactory.IdentifierName(objectName)).WithTriviaFrom(argumentSyntax);
                                SeparatedSyntaxList<CodeExpressionSyntax> newArguments = node.ArgumentList.Arguments.Replace(argumentSyntax, newArgumentSyntax);
                                ArgumentListSyntax newArgumentList = node.ArgumentList.WithArguments(newArguments);
                                node = node.WithArgumentList(newArgumentList);
                            }
                        }
                    }
                }
            }

            return node;
        }

#if BC

        private ObjectIdParameterInformation[] GetObjectIdMethodParameter(InvocationExpressionSyntax node)
        {
            if ((node.ArgumentList?.Arguments != null) && (node.ArgumentList?.Arguments.Count > 0))
            {
                var operation = SemanticModel.GetOperation(node);

                if (operation is IInvocationExpression invocationExpression)
                    return GetObjectIdInvocationExpressionParameter(invocationExpression);
            }

            return null;
        }

        private ObjectIdParameterInformation[] GetObjectIdInvocationExpressionParameter(IInvocationExpression invocationExpression)
        {
            if ((invocationExpression.Instance == null) && (invocationExpression.TargetMethod?.ContainingSymbol != null))
            {
                //system methods
                if ((invocationExpression.TargetMethod.MethodKind.ConvertToLocalType() == ConvertedMethodKind.BuiltInMethod) &&
                    (invocationExpression.TargetMethod.ContainingSymbol.Kind.ConvertToLocalType() == ConvertedSymbolKind.Class))
                {
                    var typeName = invocationExpression.TargetMethod.ContainingSymbol.Name;
                    var methodName = invocationExpression.TargetMethod.Name;
                    var objectType = GetObjectTypeForSystemFunction(typeName, methodName);
                    if (objectType == null)
                        return null;
                    return new ObjectIdParameterInformation[] { new ObjectIdParameterInformation(0, objectType) };
                }

            }
            else
            {
                var invocationInstanceType = invocationExpression.Instance.Type;
                var navTypeKind = invocationInstanceType.NavTypeKind.ConvertToLocalType();
                var systemMethod = SystemTypesObjectIdParametersInformationDictionary.GetSystemTypeMethod(navTypeKind, invocationExpression.TargetMethod?.Name);
                if (systemMethod != null)
                    return systemMethod.Parameters;
            }

            return null;
        }

#else

        private ObjectIdParameterInformation[] GetObjectIdMethodParameter(InvocationExpressionSyntax node)
        {
            if ((node.ArgumentList != null) && (node.ArgumentList.Arguments != null) && (node.ArgumentList.Arguments.Count > 0) && (node.ArgumentList.Arguments[0] is LiteralExpressionSyntax argumentSyntax))
            {
                if (argumentSyntax.Literal is Int32SignedLiteralValueSyntax intLiteralSyntax)
                {
                    int objectId;
                    if (Int32.TryParse(intLiteralSyntax.Number.ValueText, out objectId))
                    {
                        if ((objectId != 0) && (node.Expression is MemberAccessExpressionSyntax expressionSyntax))
                        {
                            if (expressionSyntax.Expression is IdentifierNameSyntax expressionNameSyntax)
                            {
                                string expressionName = expressionNameSyntax.Identifier.ValueText;
                                string expressionMemberName = expressionSyntax.Name?.Identifier.ValueText;
                                string objectType = this.GetObjectTypeForSystemFunction(expressionName, expressionMemberName);
                                if (objectType != null)
                                    return new ObjectIdParameterInformation[] { new ObjectIdParameterInformation(0, objectType) };
                            }
                        }
                    }
                }
            }

            return null;
        }

#endif


        protected string GetObjectTypeForSystemFunction(string expressionName, string expressionMemberName)
        {
            if ((String.IsNullOrWhiteSpace(expressionName)) || (String.IsNullOrWhiteSpace(expressionMemberName)))
                return null;
            string expression = expressionName.ToLower() + "." + expressionMemberName.ToLower();

            switch (expression)
            {
                case "codeunit.run":
                    return "codeunit";
                case "report.run":
                case "report.execute":
                case "report.print":
                case "report.rdlclayout":
                case "report.runmodal":
                case "report.runrequestpage":
                case "report.saveas":
                case "report.saveasexcel":
                case "report.saveashtml":
                case "report.saveaspdf":
                case "report.saveasword":
                case "report.saveasxml":
                    return "report";
                case "xmlport.run":
                case "xmlport.export":
                case "xmlport.import":
                    return "xmlport";
                case "page.run":
                case "page.runmodal":
                    return "page";
                case "recordref.open":
                    return "table";
                case "query.saveascsv":
                case "query.saveasxml":
                    return "query";
            }

            return null;
        }

        protected string FindObjectTypeForObjectNameOrId(ObjectNameOrIdSyntax node)
        {
            if (node.Parent.ContainsDiagnostics)
                return null;

            switch (node.Parent)
            {
                case SubtypedDataTypeSyntax subtypedDataTypeSyntax:
                    return subtypedDataTypeSyntax.TypeName.ValueText;
                case ObjectReferencePropertyValueSyntax objectReferencePropertyValueSyntax:
                    if (objectReferencePropertyValueSyntax.Parent is PropertySyntax propertySyntax)
                        return this.PropertyNameToObjectTypeName(propertySyntax.Name?.Identifier.ValueText);
                    break;
                case PermissionSyntax permissionSyntax:
                    return permissionSyntax.ObjectType.ValueText;
                case PagePartSyntax pagePartSyntax:
                    return "page";
                case QualifiedObjectReferencePropertyValueSyntax qualifiedObjectReferenceSyntax:
                    return ALSyntaxHelper.DecodeName(qualifiedObjectReferenceSyntax.ObjectType.ToString());
            }

            return null;
        }

        protected ALAppObject FindObjectById(string objectType, int objectId)
        {
            ALSymbolKind alSymbolKind = this.TypeNameToSymbolKind(objectType);
            if (alSymbolKind == ALSymbolKind.Undefined)
                return null;
            ALAppObject alAppObject = this.Project.Symbols.FindObjectById(alSymbolKind, objectId, false);
            if (alAppObject != null)
                return alAppObject;
            if (this.Project.Dependencies != null)
            {
                for (int i=0; i<this.Project.Dependencies.Count; i++)
                {
                    if (this.Project.Dependencies[i].Symbols != null)
                    {
                        alAppObject = this.Project.Dependencies[i].Symbols.FindObjectById(alSymbolKind, objectId, false);
                        if (alAppObject != null)
                            return alAppObject;
                    }
                }
            }
            return null;
        }


        protected string PropertyNameToObjectTypeName(string name)
        {
            if (name != null)
            {
                name = name.ToLower();
                switch (name)
                {
                    case "tableid":
                    case "tableno":
                    case "sourcetable":
                        return "table";
                    case "rolecenter":
                    case "cardpageid":
                    case "drilldownpageid":
                    case "lookuppageid":
                    case "navigationpageid":
                        return "page";
                }
            }
            return null;
        }

        protected ALSymbolKind TypeNameToSymbolKind(string typeName)
        {
            typeName = typeName.ToLower();
            switch (typeName)
            {
                case "tabledata": return ALSymbolKind.TableObject;
                case "table": return ALSymbolKind.TableObject;
                case "record": return ALSymbolKind.TableObject;
                case "page": return ALSymbolKind.PageObject;
                case "report": return ALSymbolKind.ReportObject;
                case "xmlport": return ALSymbolKind.XmlPortObject;
                case "query": return ALSymbolKind.QueryObject;
                case "codeunit": return ALSymbolKind.CodeunitObject;
                case "controladdin": return ALSymbolKind.ControlAddInObject;
                case "enum": return ALSymbolKind.EnumType;
                case "permissionset": return ALSymbolKind.PermissionSet;
                    //case "profile": return ALSymbolKind.ProfileObject;
                    //case "dotnet": return ALSymbolKind.DotNetPackage;
                    //case "interface": return ALSymbolKind.Interface;
            }
            return ALSymbolKind.Undefined;
        }

        protected string ObjectTypeNameToEnumName(string typeName)
        {
            typeName = typeName.ToLower();
            switch (typeName)
            {
                case "tabledata": return "Database";
                case "table": return "Database";
                case "record": return "Database";
                case "page": return "Page";
                case "report": return "Report";
                case "xmlport": return "XmlPort";
                case "query": return "Query";
                case "codeunit": return "Codeunit";
                case "enum": return "Enum";
            }
            return typeName;
        }

    }

}

