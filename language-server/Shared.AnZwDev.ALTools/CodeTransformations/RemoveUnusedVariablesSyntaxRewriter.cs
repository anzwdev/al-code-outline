using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.ALSymbols.Internal;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.CodeAnalysis;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC
    public class RemoveUnusedVariablesSyntaxRewriter : ALSemanticModelSyntaxRewriter
    {
        public bool RemoveGlobalVariables { get; set; }
        public bool RemoveLocalVariables { get; set; }
        public bool RemoveLocalMethodParameters { get; set; }
        public bool IgnoreCodeAnalysisRules { get; set; }

        private SyntaxTreeDirectivesParser _directivesParser = new SyntaxTreeDirectivesParser();
        private const string _unusedVariableErrorCode = "AA0137";

        public RemoveUnusedVariablesSyntaxRewriter()
        {
            this.RemoveGlobalVariables = true;
            this.RemoveLocalVariables = true;
            this.RemoveLocalMethodParameters = true;
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node is CompilationUnitSyntax)
            {
                _directivesParser.DisabledErrors = this.Project?.Properties.SuppressWarnings;
                _directivesParser.Parse(node);
            }

            return base.Visit(node);
        }

        public override SyntaxNode VisitGlobalVarSection(GlobalVarSectionSyntax node)
        {
            if (this.RemoveGlobalVariables)
            {
                var appObject = node.FindParentApplicationObject();
                if (appObject != null)
                {
                    (GlobalVarSectionSyntax newGlobalVariables, bool globalsUpdated) = this.RemoveUnusedGlobalVariablesFromNode(appObject, node);
                    if (globalsUpdated)
                        return newGlobalVariables;
                }
            }
            return base.VisitGlobalVarSection(node);
        }

        public override SyntaxNode VisitTriggerDeclaration(TriggerDeclarationSyntax node)
        {
            if ((this.RemoveLocalVariables) || (this.RemoveLocalMethodParameters))
            {
                (TriggerDeclarationSyntax newTriggerDeclaration, bool triggerUpdated) = this.RemoveUnusedLocalVariablesFromTrigger(node);
                if (triggerUpdated)
                    return newTriggerDeclaration;
            }
            return base.VisitTriggerDeclaration(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if ((this.RemoveLocalVariables) || (this.RemoveLocalMethodParameters))
            {
                (MethodDeclarationSyntax newMethodDeclaration, bool methodUpdated) = this.RemoveUnusedLocalVariablesFromMethod(node);
                if (methodUpdated)
                    return newMethodDeclaration;
            }
            return base.VisitMethodDeclaration(node);
        }

        #region Remove usused variables from object members

        protected (GlobalVarSectionSyntax, bool) RemoveUnusedGlobalVariablesFromNode(SyntaxNode node, GlobalVarSectionSyntax globalVariables)
        {
            //collect global variables names
            HashSet<string> deleteVariables = this.CollectVariableNames(globalVariables);

            //remove used global variables from the list
            this.RemoveReferencedGlobalVariablesFromNamesSet(deleteVariables, node);

            //remove unused global variables from the global variables section
            if (deleteVariables.Count > 0)
            {
                this.NoOfChanges += deleteVariables.Count;
                GlobalVarSectionSyntax newGlobalVarSection = this.RemoveGlobalVarSectionVariables(globalVariables, deleteVariables);
                return (newGlobalVarSection, true);
            }

            return (globalVariables, false);
        }

        protected (TriggerDeclarationSyntax, bool) RemoveUnusedLocalVariablesFromTrigger(TriggerDeclarationSyntax triggerDeclaration)
        {
            if (this.RemoveLocalVariables)
            {

                //collect local variables
                SymbolInfo info = this.SemanticModel.GetSymbolInfo(triggerDeclaration);
                IMethodSymbol symbol = SemanticModel.GetDeclaredSymbol(triggerDeclaration) as IMethodSymbol;

                if ((symbol != null) && (symbol.LocalVariables != null))
                {
                    //collect variables                
                    Dictionary<string, ISymbol> deleteVariables = new Dictionary<string, ISymbol>();
                    this.CollectLocalVariables(deleteVariables, symbol);

                    //analyze variables
                    if (triggerDeclaration.Body != null)
                        this.RemoveReferencedVariables(deleteVariables, triggerDeclaration.Body);

                    if (deleteVariables.Count > 0)
                    {
                        HashSet<string> deleteVariableNames = new HashSet<string>();
                        foreach (string name in deleteVariables.Keys)
                        {
                            deleteVariableNames.Add(name);
                        }

                        if ((triggerDeclaration.Variables != null) && (triggerDeclaration.Variables.Variables != null))
                        {
                            this.NoOfChanges += deleteVariables.Count;
                            (var varSectionSyntax, var collectedDirectives) = this.RemoveVarSectionVariables(triggerDeclaration.Variables, deleteVariableNames);
                            triggerDeclaration = triggerDeclaration.WithVariables(varSectionSyntax);

                            if ((collectedDirectives.Count > 0) && (triggerDeclaration.Body != null))
                                triggerDeclaration = triggerDeclaration.WithBody(
                                    triggerDeclaration.Body.WithLeadingLeadingTrivia(collectedDirectives));

                            return (triggerDeclaration, true);
                        }
                    }
                }
            }

            return (triggerDeclaration, false);
        }

        protected (MethodDeclarationSyntax, bool) RemoveUnusedLocalVariablesFromMethod(MethodDeclarationSyntax methodDeclaration)
        {
            var origParameterList = methodDeclaration.ParameterList;
            var origVariablesList = methodDeclaration.Variables;

            string returnValueName = null;
            SymbolInfo info = this.SemanticModel.GetSymbolInfo(methodDeclaration);
            IMethodSymbol symbol = SemanticModel.GetDeclaredSymbol(methodDeclaration) as IMethodSymbol;
            if ((symbol != null) && (symbol.LocalVariables != null))
            {
                bool removeMethodParameters = (this.RemoveLocalMethodParameters) && 
                    (symbol.IsLocal) && 
                    (symbol.MethodKind.ConvertToLocalType() != ConvertedMethodKind.Trigger) && 
                    (!symbol.IsEventSubscriber() &&
                    (!symbol.IsEvent) &&
                    (symbol.Parameters != null) &&
                    (origParameterList != null) &&
                    (origParameterList.Parameters != null));

                //collect local variables                
                Dictionary<string, ISymbol> deleteVariables = new Dictionary<string, ISymbol>();
                this.CollectLocalVariables(deleteVariables, symbol);

                //add return variable
                if (symbol.IsLocal)
                {
                    if ((symbol.ReturnValueSymbol != null) && (!symbol.ReturnValueSymbol.IsSynthesized) && (symbol.ReturnValueSymbol.IsNamed))
                    {
                        returnValueName = symbol.ReturnValueSymbol.Name.ToLower();
                        if ((!String.IsNullOrWhiteSpace(returnValueName)) && (!deleteVariables.ContainsKey(returnValueName)))
                            deleteVariables.Add(returnValueName, symbol.ReturnValueSymbol);
                    }

                    //remove parameters
                    if (removeMethodParameters)
                    {
                        foreach (ISymbol parameterSymbol in symbol.Parameters)
                        {
                            if (!parameterSymbol.IsSynthesized)
                            {
                                string parameterName = parameterSymbol.Name.ToLower();
                                if ((!deleteVariables.ContainsKey(parameterName)) && (!deleteVariables.ContainsKey(parameterName)))
                                    deleteVariables.Add(parameterName, parameterSymbol);
                            }
                        }
                    }
                }

                //analyze variables
                if (methodDeclaration.Body != null)
                    this.RemoveReferencedVariables(deleteVariables, methodDeclaration.Body);

                //remove unused variables
                bool updated = false;
                if (deleteVariables.Count > 0)
                {
                    this.NoOfChanges += deleteVariables.Count;

                    //remove variables
                    HashSet<string> deleteVariableNames = new HashSet<string>();
                    foreach (string name in deleteVariables.Keys)
                    {
                        deleteVariableNames.Add(name);
                    }

                    if (this.RemoveLocalVariables)
                    {
                        if ((origVariablesList != null) && (origVariablesList.Variables != null))
                        {
                            (var varSectionSyntax, var collectedDirectives) = this.RemoveVarSectionVariables(origVariablesList, deleteVariableNames);
                            methodDeclaration = methodDeclaration.WithVariables(varSectionSyntax);

                            if ((collectedDirectives.Count > 0) && (methodDeclaration.Body != null))
                                methodDeclaration = methodDeclaration.WithBody(
                                    methodDeclaration.Body.WithLeadingLeadingTrivia(collectedDirectives));

                            updated = true;
                        }

                        //remove return value name
                        if ((!String.IsNullOrWhiteSpace(returnValueName)) && (deleteVariables.ContainsKey(returnValueName)))
                        {
                            methodDeclaration = methodDeclaration.WithReturnValue(methodDeclaration.ReturnValue.WithName(null));
                            updated = true;
                        }
                    }

                    //remove parameters
                    if (removeMethodParameters)
                    {
                        (var newParametersSyntax, var collectedDirectives, var parametersUpdated) = this.RemoveParametersVariables(origParameterList.Parameters, deleteVariableNames);
                        if (parametersUpdated)
                        {
                            var newParameterList = methodDeclaration.ParameterList.WithParameters(newParametersSyntax);
                            if ((collectedDirectives.Count > 0) && (newParameterList.CloseParenthesisToken != null) && (newParameterList.CloseParenthesisToken.Kind.ConvertToLocalType() != ConvertedSyntaxKind.None))
                                newParameterList = newParameterList.WithCloseParenthesisToken(
                                    newParameterList.CloseParenthesisToken.WithLeadingLeadingTrivia(collectedDirectives));

                            methodDeclaration = methodDeclaration.WithParameterList(newParameterList);

                            updated = true;
                        }
                    }

                    return (methodDeclaration, updated);
                }
            }

            return (methodDeclaration, false);
        }

        protected void CollectLocalVariables(Dictionary<string, ISymbol> deleteVariables, IMethodSymbol symbol)
        {
            foreach (IVariableSymbol variableSymbol in symbol.LocalVariables)
            {
                string localVariableName = variableSymbol.Name.ToLower();
                if ((!variableSymbol.IsSynthesized) && (!deleteVariables.ContainsKey(localVariableName)))
                    deleteVariables.Add(localVariableName, variableSymbol);
            }
        }

        #endregion

        #region Collect variable names from var section

        protected HashSet<string> CollectVariableNames(VarSectionBaseSyntax varSection)
        {
            string name;
            HashSet<string> namesCollection = new HashSet<string>();

            if (varSection.Variables != null)
            {
                foreach (VariableDeclarationBaseSyntax baseVariableDeclaration in varSection.Variables)
                {
                    switch (baseVariableDeclaration)
                    {
                        case VariableDeclarationSyntax variableDeclaration:
                            name = variableDeclaration.Name.Unquoted()?.ToLower();
                            if ((!String.IsNullOrWhiteSpace(name)) && (!namesCollection.Contains(name)))
                                namesCollection.Add(name);
                            break;
                        case VariableListDeclarationSyntax variableListDeclaration:
                            if (variableListDeclaration.VariableNames != null)
                            {
                                foreach (VariableDeclarationNameSyntax variableNameSyntax in variableListDeclaration.VariableNames)
                                {
                                    name = variableNameSyntax.Name.Unquoted()?.ToLower();
                                    if ((!String.IsNullOrWhiteSpace(name)) && (!namesCollection.Contains(name)))
                                        namesCollection.Add(name);
                                }
                            }
                            break;
                    }
                }
            }
            return namesCollection;
        }

        #endregion

        #region Remove variables from method parameters list

        protected (SeparatedSyntaxList<ParameterSyntax>, List<SyntaxTrivia>, bool) RemoveParametersVariables(SeparatedSyntaxList<ParameterSyntax> parametersListSyntax, HashSet<string> deleteVariables)
        {
            bool updated = false;
            List<SyntaxTrivia> collectedDirectives = new List<SyntaxTrivia>();
            SeparatedSyntaxList<ParameterSyntax> newParametersListSyntax = parametersListSyntax;
            int idx = 0;

            while (idx < newParametersListSyntax.Count)
            {
                ParameterSyntax parameterSyntax = newParametersListSyntax[idx];
                string name = parameterSyntax.Name.Unquoted()?.ToLower();
                var errorEnabled = (IgnoreCodeAnalysisRules || _directivesParser.GetErrorCodeStateAtPosition(parameterSyntax.Span.Start, _unusedVariableErrorCode));

                if ((!String.IsNullOrWhiteSpace(name)) && (deleteVariables.Contains(name)) && (errorEnabled))
                {
                    updated = true;
                    parameterSyntax.CollectDirectiveTrivias(collectedDirectives);
                    newParametersListSyntax = newParametersListSyntax.RemoveAt(idx);
                }
                else
                {
                    if (collectedDirectives.Count > 0)
                    {
                        var updatedParameterSyntax = parameterSyntax.WithLeadingLeadingTrivia(collectedDirectives);
                        newParametersListSyntax = newParametersListSyntax.Replace(parameterSyntax, updatedParameterSyntax);
                        collectedDirectives.Clear();
                    }
                    idx++;
                }
            }

            if ((collectedDirectives.Count > 0) && (newParametersListSyntax.Count > 0))
            {
                var parameterSyntax = newParametersListSyntax[newParametersListSyntax.Count - 1];
                var updatedParameterSyntax = parameterSyntax.WithTrailingTrailingTrivia(collectedDirectives);
                newParametersListSyntax = newParametersListSyntax.Replace(parameterSyntax, updatedParameterSyntax);
                collectedDirectives.Clear();
            }

            return (newParametersListSyntax, collectedDirectives, updated);
        }

        #endregion

        #region Remove variables from var sections

        protected GlobalVarSectionSyntax RemoveGlobalVarSectionVariables(GlobalVarSectionSyntax node, HashSet<string> deleteVariables)
        {
            (var newVariablesSyntax, var collectedDirectives)  = this.RemoveVarSectionVariables(node.Variables, deleteVariables);

            if (newVariablesSyntax == null)
            {
                //include leading directives trivia from node
                var varSectionDirectives = new List<SyntaxTrivia>();
                node.CollectLeadingDirectiveTrivias(varSectionDirectives);
                if (varSectionDirectives.Count > 0)
                    collectedDirectives.InsertRange(0, varSectionDirectives);

                if (collectedDirectives.Count == 0)
                    return null;
                newVariablesSyntax = SyntaxFactory.List(new List<VariableDeclarationBaseSyntax>());
            }

            node = node.WithVariables(newVariablesSyntax.Value);

            if (collectedDirectives.Count > 0)
                node = node.WithLeadingTrailingTrivia(collectedDirectives);

            return node.WithVariables(newVariablesSyntax.Value);
        }

        protected (VarSectionSyntax, List<SyntaxTrivia>) RemoveVarSectionVariables(VarSectionSyntax node, HashSet<string> deleteVariables)
        {
            (var newVariablesSyntax, var collectedDirectives) = this.RemoveVarSectionVariables(node.Variables, deleteVariables);
            if (newVariablesSyntax == null)
            {
                //include leading directives trivia from node
                var varSectionDirectives = new List<SyntaxTrivia>();
                node.CollectLeadingDirectiveTrivias(varSectionDirectives);
                if (varSectionDirectives.Count > 0)
                    collectedDirectives.InsertRange(0, varSectionDirectives);

                return (null, collectedDirectives);
            }
            return (node.WithVariables(newVariablesSyntax.Value), collectedDirectives);
        }

        protected (SyntaxList<VariableDeclarationBaseSyntax>?, List<SyntaxTrivia>) RemoveVarSectionVariables(SyntaxList<VariableDeclarationBaseSyntax> variablesSyntax, HashSet<string> deleteVariables)
        {
            List<SyntaxTrivia> collectedDirectives = new List<SyntaxTrivia>();
            List<VariableDeclarationBaseSyntax> keepVariables = new List<VariableDeclarationBaseSyntax>();
            foreach (VariableDeclarationBaseSyntax variableSyntax in variablesSyntax)
            {
                switch (variableSyntax)
                {
                    case VariableListDeclarationSyntax variableListSyntax:
                        variableListSyntax = this.RemoveVariableListVariables(variableListSyntax, deleteVariables);
                        if (variableListSyntax != null)
                            keepVariables.Add(variableListSyntax);
                        break;
                    case VariableDeclarationSyntax variableDeclarationSyntax:
                        string name = variableDeclarationSyntax.Name.Unquoted()?.ToLower();
                        var errorEnabled = (IgnoreCodeAnalysisRules) || (_directivesParser.GetErrorCodeStateAtPosition(variableSyntax.Span.Start, _unusedVariableErrorCode));

                        if ((deleteVariables.Contains(name)) && (errorEnabled))
                            variableSyntax.CollectDirectiveTrivias(collectedDirectives);
                        else
                        {
                            var newVariableSyntax = variableSyntax.WithLeadingLeadingTrivia(collectedDirectives);
                            collectedDirectives.Clear();
                            keepVariables.Add(newVariableSyntax);
                        }
                        break;
                }
            }

            if (keepVariables.Count == 0)
                return (null, collectedDirectives);

            if (collectedDirectives.Count > 0)
            {
                keepVariables[keepVariables.Count - 1] = keepVariables[keepVariables.Count - 1].WithTrailingTrailingTrivia(collectedDirectives);
                collectedDirectives.Clear();
            }

            return (SyntaxFactory.List(keepVariables), collectedDirectives);
        }

        protected VariableListDeclarationSyntax RemoveVariableListVariables(VariableListDeclarationSyntax variableList, HashSet<string> deleteVariables)
        {
            List<VariableDeclarationNameSyntax> keepNames = new List<VariableDeclarationNameSyntax>();
            bool listChanged = false;
            foreach (VariableDeclarationNameSyntax variableNameSyntax in variableList.VariableNames)
            {
                string name = variableNameSyntax.Name.Unquoted()?.ToLower();
                if (deleteVariables.Contains(name))
                    listChanged = true;
                else
                    keepNames.Add(variableNameSyntax);
            }

            if (listChanged)
            {
                if (keepNames.Count == 0)
                    return null;

                var newNamesList = new SeparatedSyntaxList<VariableDeclarationNameSyntax>();
                newNamesList = newNamesList.AddRange(keepNames);
                return variableList.WithVariableNames(newNamesList);
            }

            return variableList;
        }

        #endregion

        #region Process syntax nodes and remove referenced variables from the list of variables

        protected void RemoveReferencedVariables(Dictionary<string, ISymbol> deleteVariables, SyntaxNode node)
        {
            if (node is IdentifierNameSyntax identifierNameSyntax)
            {
                string name = identifierNameSyntax.Unquoted()?.ToLower();
                if (deleteVariables.ContainsKey(name))
                {
                    SymbolInfo symbolInfo = this.SemanticModel.GetSymbolInfo(identifierNameSyntax);
                    if ((symbolInfo != null) && (symbolInfo.Symbol != null) && (deleteVariables[name] == symbolInfo.Symbol))
                        deleteVariables.Remove(name);
                }
            }

            IEnumerable<SyntaxNode> syntaxNodes = node.ChildNodes();
            foreach (SyntaxNode childNode in syntaxNodes)
            {
                this.RemoveReferencedVariables(deleteVariables, childNode);
            }
        }

        protected void RemoveReferencedGlobalVariablesFromNamesSet(HashSet<string> namesCollection, SyntaxNode node)
        {
            ConvertedSyntaxKind nodeKind = node.Kind.ConvertToLocalType();
            switch (nodeKind)
            {
                case ConvertedSyntaxKind.GlobalVarSection:
                case ConvertedSyntaxKind.VarSection:
                case ConvertedSyntaxKind.VariableDeclaration:
                case ConvertedSyntaxKind.ParameterList:
                case ConvertedSyntaxKind.ReturnValue:
                    return;
            }

            if (node is IdentifierNameSyntax identifierNameSyntax)
            {
                string name = identifierNameSyntax.Unquoted()?.ToLower();
                if (namesCollection.Contains(name))
                {
                    SymbolInfo symbolInfo = this.SemanticModel.GetSymbolInfo(identifierNameSyntax);
                    if ((symbolInfo != null) && (symbolInfo.Symbol != null) && (symbolInfo.Symbol.Kind.ConvertToLocalType() == ConvertedSymbolKind.GlobalVariable))
                        namesCollection.Remove(name);
                }
            }

            IEnumerable<SyntaxNode> syntaxNodes = node.ChildNodes();
            foreach (SyntaxNode childNode in syntaxNodes)
            {
                this.RemoveReferencedGlobalVariablesFromNamesSet(namesCollection, childNode);
            }
        }

        #endregion

    }

#endif
}
