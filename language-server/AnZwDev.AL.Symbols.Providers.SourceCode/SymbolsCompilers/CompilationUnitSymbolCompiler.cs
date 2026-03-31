using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.System.Collections.Extensions;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class CompilationUnitSymbolCompiler
    {

        public static void Compile(CompilationUnitSyntax compilationUnitSyntax, AllObjectSymbolsCollection objectsCollection, string sourceFileName)
        {
            if (compilationUnitSyntax.Objects.Count > 0)
            {
                var usings = UsingSymbolCompiler.Compile(compilationUnitSyntax.Usings);
                var namespaceName = NameCompiler.Compile(compilationUnitSyntax.NamespaceDeclaration?.Name);
                if (!String.IsNullOrWhiteSpace(namespaceName))
                    usings = usings.AddOrCreate(namespaceName);

                for (int i=0; i< compilationUnitSyntax.Objects.Count; i++)
                {
                    var objectSyntax = compilationUnitSyntax.Objects[i];
                    var objectSymbol = ObjectSymbolCompiler.Compile(objectSyntax, namespaceName, usings, sourceFileName);
                    if (objectSymbol != null)
                        objectsCollection.Add(objectSymbol);
                }   
            }
        }

    }
}
