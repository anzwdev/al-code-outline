using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;

namespace AnZwDev.AL.Symbols.CodeAnalysis
{
    public static class ApplicationSymbolExtensions
    {

        public static ParseOptions GetParseOptions(this ApplicationSymbol? applicationSymbol)
        {
            if (applicationSymbol == null)
                return ParseOptions.Default;

            return new ParseOptions(applicationSymbol.Metadata.BCRuntimeVersion, applicationSymbol.Metadata.PreprocessorSymbols);
        }

    }
}
