using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public static class VariableSortModeExtensions
    {

        public static bool SortByMainTypeNameOnly(this VariablesSortMode sortMode)
        {
            return (sortMode == VariablesSortMode.MainTypeNameOnly) || (sortMode == VariablesSortMode.MainTypeNameOnlyKeepVariableNameOrder);
        }

        public static bool SortByVariableName(this VariablesSortMode sortMode)
        {
            return (sortMode == VariablesSortMode.MainTypeNameOnly) || (sortMode == VariablesSortMode.FullTypeName);
        }

        public static VariablesSortMode FromString(string value)
        {
            if (Enum.TryParse(value, true, out VariablesSortMode mode))
                return mode;
            return VariablesSortMode.FullTypeName;
        }

    }
}
