using AnZwDev.AL.CodeAnalysis.Sorting.Comparers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Extensions
{
    public static class PropertySyntaxExtensions
    {

        public static PropertySyntax SortCommaSeparatedPropertyValue(this PropertySyntax property, bool sortSingleNodeRegions, out bool sorted)
        {
            sorted = false;
            var value = property.Value as CommaSeparatedPropertyValueSyntax;
            if (value != null)
            {
                value = value.WithValues(
                    SyntaxNodesGroupsTree<IdentifierNameSyntax>.SortSeparatedSyntaxList(value.Values, new IdentifierNameComparer(), sortSingleNodeRegions, out sorted));
                property = property.WithValue(value);
            }
            return property;
        }


    }
}
