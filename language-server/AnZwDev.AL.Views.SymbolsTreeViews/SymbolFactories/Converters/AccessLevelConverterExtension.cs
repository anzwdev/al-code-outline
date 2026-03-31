using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class AccessLevelConverterExtension
    {

        public static ALSyntaxNodeAccessModifier ToALSyntaxNodeAccessModifier(this AccessLevel accessLevel)
        {
            return accessLevel switch
            {
                AccessLevel.Public => ALSyntaxNodeAccessModifier.Public,
                AccessLevel.Protected => ALSyntaxNodeAccessModifier.Protected,
                AccessLevel.Local => ALSyntaxNodeAccessModifier.Local,
                AccessLevel.Internal => ALSyntaxNodeAccessModifier.Internal,
                _ => ALSyntaxNodeAccessModifier.Public
            };
        }

    }
}
