using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ArrayDimensionsCompiler
    {

        public static List<int>? Compile(ArraySyntax? syntax)
        {
            if ((syntax?.DimensionList?.Dimensions == null) || (syntax.DimensionList.Dimensions.Count == 0))
                return null;

            var list = new List<int>(syntax.DimensionList.Dimensions.Count);
            for (int i=0; i < syntax.DimensionList.Dimensions.Count; i++)
            {
                list.Add(Compile(syntax.DimensionList.Dimensions[i]));
            }

            return list;
        }

        private static int Compile(DimensionSyntax syntax)
        {
            var text = syntax?.Value.Text;
            if (String.IsNullOrEmpty(text))
                return 0;
            return ALLiteralParser.ParseInt(text);
        }

    }
}
