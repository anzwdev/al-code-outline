using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal class ReturnParameterDefinitionSymbolSymbolCompiler
    {

        public static MethodReturnParameterDefinitionSymbol? Compile(ReturnValueSyntax? syntax)
        {
            if (syntax == null)
                return null;

            var dataTypeSymbol = TypeDefinitionSymbolCompiler.Compile(syntax.DataType);
            if (dataTypeSymbol == null) 
                return null;

            return new MethodReturnParameterDefinitionSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                IsVar = false,
                Attributes = null,
                TypeDefinition = TypeDefinitionSymbolCompiler.Compile(syntax.DataType)
            };
        }
    }
}
