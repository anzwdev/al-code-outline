using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class VariableComparer : IComparer<VariableDeclarationBaseSyntax>
    {

        protected static Dictionary<string, int> _typePriority = new Dictionary<string, int>()
            {
                { "record", 0 },
                { "report", 1 },
                { "codeunit", 2 },
                { "xmlport", 3 },
                { "page", 4 },
                { "query", 5 },
                { "notification", 6 },
                { "bigtext", 7 },
                { "dateformula", 8 },
                { "recordid", 9 },
                { "recordref", 10 },
                { "fieldref", 11 },
                { "filterpagebuilder", 12 }
            };

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
            dataTypeName = dataTypeName.FirstWord();
            if (_typePriority.ContainsKey(dataTypeName))
                return _typePriority[dataTypeName];
            return _typePriority.Count;
        }

        protected string GetDataTypeName(VariableDeclarationBaseSyntax? node)
        {
            if (node?.Type != null)
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
                    if (typeName.StartsWith("label", StringComparison.OrdinalIgnoreCase))
                        typeName = "label";
                    else if (typeName.StartsWith("textconst", StringComparison.OrdinalIgnoreCase))
                        typeName = "textconst";
                    return typeName;
                }
            }
            return "";
        }

        public int Compare(VariableDeclarationBaseSyntax? x, VariableDeclarationBaseSyntax? y)
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

            var xName = this.GetVariableName(x);
            var yName = this.GetVariableName(y);

            if (this.SortMode.SortByVariableName())
                return _stringComparer.Compare(xName, yName);
            return CompareOriginalOrder(xName, yName);
        }

        protected int CompareOriginalOrder(string? xName, string? yName)
        {
            if (this.OriginalOrder == null)
                return 0;

            bool xExists = (!String.IsNullOrWhiteSpace(xName)) && (this.OriginalOrder.ContainsKey(xName));
            bool yExists = (!String.IsNullOrWhiteSpace(yName)) && (this.OriginalOrder.ContainsKey(yName));

            if (xExists && yExists)
                return this.OriginalOrder[xName!].CompareTo(this.OriginalOrder[yName!]);

            if (xExists)
                return 1;

            if (yExists)
                return -1;

            return 0;
        }

        protected string? GetVariableName(VariableDeclarationBaseSyntax? variableDeclarationBaseSyntax)
        {
            if (variableDeclarationBaseSyntax is VariableListDeclarationSyntax variableListDeclaration)
            {
                if (variableListDeclaration.VariableNames.Count > 0)
                    return variableListDeclaration.VariableNames[0]?.Name?.Unquoted();
            }
            return variableDeclarationBaseSyntax?.GetNameStringValue()?.ToLower();
        }

        private Dictionary<string, int> GetVariablesOrder(SyntaxList<VariableDeclarationBaseSyntax> variables)
        {
            Dictionary<string, int> variablesOrder = new Dictionary<string, int>();
            for (int i = 0; i < variables.Count; i++)
            {
                var name = this.GetVariableName(variables[i]);
                if ((!String.IsNullOrWhiteSpace(name)) && (!variablesOrder.ContainsKey(name)))
                    variablesOrder.Add(name, i);
            }
            return variablesOrder;
        }

    }

}
