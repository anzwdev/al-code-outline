using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.TreeViewModel
{
    public static class SymbolHierarchyNodeKindExtensions
    {

        private static Dictionary<SymbolHierarchyNodeKind, string> _nameCache = new Dictionary<SymbolHierarchyNodeKind, string>();

        public static string ToDescriptionString(this SymbolHierarchyNodeKind value)
        {
            if (_nameCache.TryGetValue(value, out var name))
                return name;

            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            var description = attr?.Description ?? value.ToString();
            _nameCache[value] = description;

            return description;
        }


    }
}
