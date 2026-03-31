using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class ProfileExtensionSymbolCompiler
    {

        public static ProfileExtensionSymbol Compile(ProfileExtensionSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = 0;
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);

            return new ProfileExtensionSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.Profile, usings, syntax.BaseObject)
            };
        }


    }
}
