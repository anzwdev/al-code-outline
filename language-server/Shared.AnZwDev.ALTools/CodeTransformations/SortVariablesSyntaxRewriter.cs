using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortVariablesSyntaxRewriter: ALSyntaxRewriter
    {

        public VariablesSortMode SortMode { get; set; }

        #region Variable comparer

#if BC
        protected class VariableDeclarationNameComparer : IComparer<VariableDeclarationNameSyntax>
        {
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

            public int Compare(VariableDeclarationNameSyntax x, VariableDeclarationNameSyntax y)
            {
                string xName = x.Name?.Unquoted();
                string yName = y.Name?.Unquoted();
                return _stringComparer.Compare(xName, yName);
            }
        }

        protected class VariableComparer : IComparer<VariableDeclarationBaseSyntax>
        {
            protected static string[] _typePriority = {"record ", "report", "codeunit", "xmlport", "page", "query", "notification",
                    "bigtext", "dateformula", "recordid", "recordref", "fieldref", "filterpagebuilder" };
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();
            protected Dictionary<string, int> OriginalOrder { get; private set; }
            public VariablesSortMode SortMode { get; set; }

            public VariableComparer(VariablesSortMode sortMode, SyntaxList<VariableDeclarationBaseSyntax> originalList)
            {
                this.SortMode = sortMode;
                this.OriginalOrder = GetVariablesOrder(originalList);
            }

            protected int GetDataTypePriority(string dataTypeName)
            {
                for (int i=0; i<_typePriority.Length; i++)
                {
                    if (dataTypeName.StartsWith(_typePriority[i]))
                        return i;
                }
                return _typePriority.Length;
            }

            protected string GetDataTypeName(VariableDeclarationBaseSyntax node)
            {
                if (node.Type != null)
                {
                    string typeName = (node.Type.DataType != null) ? node.Type.DataType.ToString() : node.Type.ToString();

                    if (this.SortMode.SortByMainTypeNameOnly())
                    {
                        if ((node.Type.DataType != null) && (node.Type.DataType is SubtypedDataTypeSyntax subtypedType) && (subtypedType.TypeName.ValueText != null))
                            typeName = subtypedType.TypeName.ValueText + " ";
                    }

                    if (typeName != null)
                    {
                        typeName = typeName.Replace("\"", "").ToLower().TrimStart();
                        //ignore text value for labels and text constants
                        if (typeName.StartsWith("label", StringComparison.CurrentCultureIgnoreCase))
                            typeName = "label";
                        else if (typeName.StartsWith("textconst", StringComparison.CurrentCultureIgnoreCase))
                            typeName = "textconst";
                    }
                    return typeName;
                }
                return "";
            }

            public int Compare(VariableDeclarationBaseSyntax x, VariableDeclarationBaseSyntax y)
            {
                string xTypeName = this.GetDataTypeName(x);
                string yTypeName = this.GetDataTypeName(y);
                
                //check type
                int xTypePriority = this.GetDataTypePriority(xTypeName);
                int yTypePriority = this.GetDataTypePriority(yTypeName);
                if (xTypePriority != yTypePriority)
                    return xTypePriority - yTypePriority;

                int value = _stringComparer.Compare(xTypeName, yTypeName);
                if (value != 0)
                    return value;

                string xName = this.GetVariableName(x);
                string yName = this.GetVariableName(y);

                if (this.SortMode.SortByVariableName())
                    return _stringComparer.Compare(xName, yName);
                return CompareOriginalOrder(xName, yName);
            }

            protected int CompareOriginalOrder(string xName, string yName)
            {
                if (this.OriginalOrder == null)                    
                    return 0;

                bool xExists = this.OriginalOrder.ContainsKey(xName);
                bool yExists = this.OriginalOrder.ContainsKey(yName);

                if (xExists && yExists)
                    return this.OriginalOrder[xName].CompareTo(this.OriginalOrder[yName]);
                if (xExists)
                    return 1;
                if (yExists)
                    return -1;
                return 0;
            }

            protected string GetVariableName(VariableDeclarationBaseSyntax variableDeclarationBaseSyntax)
            {
                if (variableDeclarationBaseSyntax is VariableListDeclarationSyntax variableListDeclaration)
                {
                    if ((variableListDeclaration.VariableNames != null) && (variableListDeclaration.VariableNames.Count > 0))
                        return variableListDeclaration.VariableNames[0]?.Name?.Unquoted();
                }
                return variableDeclarationBaseSyntax.GetNameStringValue()?.ToLower();
            }

            private Dictionary<string, int> GetVariablesOrder(SyntaxList<VariableDeclarationBaseSyntax> variables)
            {
                Dictionary<string, int> variablesOrder = new Dictionary<string, int>();
                for (int i = 0; i < variables.Count; i++)
                {
                    string name = this.GetVariableName(variables[i]);
                    if (!variablesOrder.ContainsKey(name))
                        variablesOrder.Add(name, i);
                }
                return variablesOrder;
            }

        }
#else
        protected class VariableComparer : IComparer<VariableDeclarationSyntax>
        {
            protected static string[] _typePriority = {"record ", "report", "codeunit", "xmlport", "page", "query", "notification",
                    "bigtext", "dateformula", "recordid", "recordref", "fieldref", "filterpagebuilder" };
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();
            protected Dictionary<string, int> OriginalOrder { get; private set; }
            public VariablesSortMode SortMode { get; set; }

            public VariableComparer(SyntaxList<VariableDeclarationSyntax> originalList)
            {
                this.OriginalOrder = GetVariablesOrder(originalList);
            }

            protected int GetDataTypePriority(string dataTypeName)
            {
                for (int i = 0; i < _typePriority.Length; i++)
                {
                    if (dataTypeName.StartsWith(_typePriority[i]))
                        return i;
                }
                return _typePriority.Length;
            }

            protected string GetDataTypeName(VariableDeclarationSyntax node)
            {
                if (node.Type != null)
                {
                    if (node.Type.DataType != null)
                        return node.Type.DataType.ToString().Replace("\"", "").ToLower().Trim();
                    return node.Type.ToString().ToLower().Replace("\"", "").Trim();
                }
                return "";
            }

            public int Compare(VariableDeclarationSyntax x, VariableDeclarationSyntax y)
            {
                string xTypeName = this.GetDataTypeName(x);
                string yTypeName = this.GetDataTypeName(y);

                //check type
                int xTypePriority = this.GetDataTypePriority(xTypeName);
                int yTypePriority = this.GetDataTypePriority(yTypeName);
                if (xTypePriority != yTypePriority)
                    return xTypePriority - yTypePriority;

                int value = _stringComparer.Compare(xTypeName, yTypeName);
                if (value != 0)
                    return value;

                string xName = GetVariableName(x);
                string yName = GetVariableName(y);

                if (this.SortMode.SortByVariableName())
                    return _stringComparer.Compare(xName, yName);
                return CompareOriginalOrder(xName, yName);
            }

            private string GetVariableName(VariableDeclarationSyntax variable)
            {
                return variable.GetNameStringValue().ToLower();
            }

            private Dictionary<string, int> GetVariablesOrder(SyntaxList<VariableDeclarationSyntax> variables)
            {
                Dictionary<string, int> variablesOrder = new Dictionary<string, int>();
                for (int i = 0; i < variables.Count; i++)
                {
                    string name = GetVariableName(variables[i]);
                    if (!variablesOrder.ContainsKey(name))
                        variablesOrder.Add(name, i);
                }
                return variablesOrder;
            }

            protected int CompareOriginalOrder(string xName, string yName)
            {
                if (this.OriginalOrder == null)                    
                    return 0;

                bool xExists = this.OriginalOrder.ContainsKey(xName);
                bool yExists = this.OriginalOrder.ContainsKey(yName);

                if (xExists && yExists)
                    return this.OriginalOrder[xName].CompareTo(this.OriginalOrder[yName]);
                if (xExists)
                    return 1;
                if (yExists)
                    return -1;
                return 0;
            }

        }
#endif

        #endregion

        public SortVariablesSyntaxRewriter()
        {
        }

#if BC
        public override SyntaxNode VisitGlobalVarSection(GlobalVarSectionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithVariables(this.SortVariables(node.Variables));
            return base.VisitGlobalVarSection(node);
        }
#endif

        public override SyntaxNode VisitVarSection(VarSectionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithVariables(this.SortVariables(node.Variables));
            return base.VisitVarSection(node);
        }

#if BC
        protected SyntaxList<VariableDeclarationBaseSyntax> SortVariables(SyntaxList<VariableDeclarationBaseSyntax> variables)
        {
            //sort variable names in variable list declarations
            bool anyNamesSorted = false;
            if ((this.SortMode.SortByVariableName()) && (variables != null) && (variables.Count > 0))
            {
                VariableDeclarationNameComparer variableNameComparer = new VariableDeclarationNameComparer();
                for (int i=0; i<variables.Count; i++)
                {
                    if (variables[i] is VariableListDeclarationSyntax variableListDeclaration)
                    {
                        (VariableListDeclarationSyntax newVariableListDeclaration, bool namesSorted) = this.SortVariableNames(variableListDeclaration, variableNameComparer);
                        if (namesSorted)
                        {
                            variables = variables.Replace(variableListDeclaration, newVariableListDeclaration);
                            anyNamesSorted = true;
                        }
                    }
                }
            }

            //sort variables
            var newVariables = SyntaxNodesGroupsTree<VariableDeclarationBaseSyntax>.SortSyntaxList(
                variables, new VariableComparer(this.SortMode, variables), out bool sorted);
            if (sorted || anyNamesSorted)
                this.NoOfChanges++;
            return newVariables;
        }

        protected (VariableListDeclarationSyntax, bool) SortVariableNames(VariableListDeclarationSyntax variables, VariableDeclarationNameComparer comparer)
        {
            if ((variables.VariableNames != null) && (variables.VariableNames.Count > 1))
            {
                var newNames = SyntaxNodesGroupsTree<VariableDeclarationNameSyntax>.SortSeparatedSyntaxList(variables.VariableNames, comparer, out bool sorted);
                if (sorted)
                    return (variables.WithVariableNames(newNames), true);
            }
            return (variables, false);
        }


#else
        protected SyntaxList<VariableDeclarationSyntax> SortVariables(SyntaxList<VariableDeclarationSyntax> variables)
        {
            var newVariables = SyntaxNodesGroupsTree<VariableDeclarationSyntax>.SortSyntaxList(
                variables, new VariableComparer(variables), out bool sorted);
            if (sorted)
                this.NoOfChanges++;
            return newVariables;
        }
#endif

    }
}
