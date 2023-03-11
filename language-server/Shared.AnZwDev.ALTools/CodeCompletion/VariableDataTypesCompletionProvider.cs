using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class VariableDataTypesCompletionProvider : CodeCompletionProvider
    {

        private static string[] _commitCharacters = { ";" };

        public VariableDataTypesCompletionProvider(ALDevToolsServer server) : base(server, "VariableDataTypes")
        {
        }

        public override void CollectCompletionItems(ALProject project, SyntaxTree syntaxTree, SyntaxNode syntaxNode, int position, List<CodeCompletionItem> completionItems)
        {
            var declaration = GetDeclaration(syntaxNode, position);
            if (declaration == null)
                return;
            var nameNode = GetDeclarationName(declaration);

            if ((nameNode != null) && (nameNode.Span.End < position))
            {
                var subtypedDataType = syntaxNode.FindParentByKind(ConvertedSyntaxKind.SubtypedDataType);
                if (subtypedDataType == null)
                {
                    string name = nameNode.Identifier.ValueText;
                    if (!String.IsNullOrWhiteSpace(name))
                        CreateCompletionItems(name.ToLower(), project, completionItems);
                }
            }
        }

        private SyntaxNode GetDeclaration(SyntaxNode syntaxNode, int position)
        {
            var parentNode = syntaxNode.FindParentByKind(
                ConvertedSyntaxKind.VariableDeclaration, 
                ConvertedSyntaxKind.Parameter,
                ConvertedSyntaxKind.ReturnValue,
                ConvertedSyntaxKind.ParameterList);
            
            switch (parentNode)
            {
                case VariableDeclarationSyntax variableSyntax:
                    return variableSyntax;
                case ParameterSyntax parameter:
                    if (!this.IsValidParametersOwner(syntaxNode))
                        return null;
                    return parameter;
                case ReturnValueSyntax returnValue:
                    if (!this.IsValidParametersOwner(syntaxNode))
                        return null;
                    return returnValue;
                case ParameterListSyntax parameterList:
                    if (!this.IsValidParametersOwner(syntaxNode))
                        return null;
                    if ((parameterList.CloseParenthesisToken.IsEmptyOrAfter(position)) &&
                        (parameterList.Parameters != null) &&
                        (parameterList.Parameters.Count > 0))
                        for (int i = parameterList.Parameters.Count - 1; i >= 0; i--)
                            if (parameterList.Parameters[i].Span.Start < position)
                                return parameterList.Parameters[i];
                    return null;
            }
            return null;
        }

        private bool IsValidParametersOwner(SyntaxNode syntaxNode)
        {
            var parameterOwner = syntaxNode.FindParentByKind(ConvertedSyntaxKind.MethodDeclaration, ConvertedSyntaxKind.TriggerDeclaration);
            
            //suggest parameters for procedures only                    
            //for triggers it should not be enabled as they have predefined list of parameters
            if (!parameterOwner.IsConvertedSyntaxKind(ConvertedSyntaxKind.MethodDeclaration))
                return false;

            //do not suggest parameters for event subscribers
            var parameterOwnerMethodDeclaration = parameterOwner as MethodDeclarationSyntax;
            if ((parameterOwnerMethodDeclaration != null) && (parameterOwnerMethodDeclaration.IsEventSubscriber()))
                return false;

            return true;
        }

        private IdentifierNameSyntax GetDeclarationName(SyntaxNode node)
        {
            if (node != null)
                switch (node)
                {
                    case VariableDeclarationSyntax variableDeclaration:
                        return variableDeclaration.Name;
                    case ParameterSyntax parameter:
                        return parameter.Name;
                    case ReturnValueSyntax returnValue:
                        if (returnValue.Name != null)
                            return returnValue.Name;
                        var method = returnValue.FindParentByKind(ConvertedSyntaxKind.MethodDeclaration) as MethodDeclarationSyntax;
                        return method?.Name;
                }
            return null;
        }

        private void CreateCompletionItems(string name, ALProject project, List<CodeCompletionItem> completionItems)
        {
            CreateCompletionItems(name, project, project.AllSymbols.Tables.GetObjects(), completionItems, name.StartsWith("Temp", StringComparison.CurrentCultureIgnoreCase));
            CreateCompletionItems(name, project, project.AllSymbols.Codeunits.GetObjects(), completionItems);
            CreateCompletionItems(name, project, project.AllSymbols.Pages.GetObjects(), completionItems);
            CreateCompletionItems(name, project, project.AllSymbols.Reports.GetObjects(), completionItems);
            CreateCompletionItems(name, project, project.AllSymbols.Queries.GetObjects(), completionItems);
            CreateCompletionItems(name, project, project.AllSymbols.XmlPorts.GetObjects(), completionItems);
            CreateCompletionItems(name, project, project.AllSymbols.EnumTypes.GetObjects(), completionItems);
            CreateCompletionItems(name, project, project.AllSymbols.Interfaces.GetObjects(), completionItems);
        }

        private void CreateCompletionItems(string name, ALProject project, IEnumerable<ALAppObject> typesCollection, List<CodeCompletionItem> completionItems, bool temporaryRecordVariable = false)
        {
            foreach (var type in typesCollection)
            {
                var varName = ALSyntaxHelper.ObjectNameToVariableNamePart(type.Name)
                    .ToLower()
                    .RemovePrefixSuffix(project.MandatoryPrefixes, project.MandatorySuffixes, project.MandatoryAffixes, project.AdditionalMandatoryAffixesPatterns);
                if (name.Contains(varName))
                {
                    var varDataType = type.GetALSymbolKind().ToVariableTypeName() + " " + ALSyntaxHelper.EncodeName(type.Name);
                    if ((temporaryRecordVariable) && (!varName.StartsWith("Temp", StringComparison.CurrentCultureIgnoreCase)))
                        varDataType = varDataType + " temporary";
                    var item = new CodeCompletionItem(varDataType, CompletionItemKind.Class);
                    item.commitCharacters = _commitCharacters;
                    completionItems.Add(item);
                }
            }
        }

    }
}
