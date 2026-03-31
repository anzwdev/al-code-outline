using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Semantics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AnZwDev.System.Extensions;

namespace AnZwDev.AL.Views.SyntaxTreeViewerTreeViews
{
    public class SyntaxTreeViewerPropertiesBuilder
    {

        public List<SyntaxTreeViewerTreeNodeProperty> CreateProperties(SyntaxNode syntaxNode)
        {
            var propertiesList = new List<SyntaxTreeViewerTreeNodeProperty>();
            var type = syntaxNode.GetType();
            var propList = type.GetProperties();

            if (propList != null)
                for (int i = 0; i < propList.Length; i++)
                {
                    if (!propList[i].Name.EqualsAny(StringComparison.OrdinalIgnoreCase, "Parent", "ParentTrivia", "SyntaxTree"))
                    {
                        var val = propList[i].GetValue(syntaxNode);
                        if (val != null)
                            propertiesList.Add(new SyntaxTreeViewerTreeNodeProperty(propList[i].Name, val.ToString() ?? String.Empty));
                    }
                }

            return propertiesList;
        }

    }
}
