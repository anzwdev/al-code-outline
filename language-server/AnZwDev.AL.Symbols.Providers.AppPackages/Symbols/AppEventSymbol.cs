using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppEventSymbol : AppBaseMethodSymbol<EventSymbol>
    {

        public override EventSymbol CreateSymbol(string? ns)
        {
            var symbol = new EventSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                MemberKind = MemberKind.EventDeclaration,
                IsInternal = IsInternal,
                IsLocal = IsLocal,
                IsProtected = IsProtected,
                Parameters = Parameters.CreateSymbolsListOrNull<MethodParameterSymbol, AppMethodParameterSymbol>(ns),
                Attributes = Attributes.CreateSymbolsListOrNull<AttributeSymbol, AppAttributeSymbol>(ns),
                ReturnParameterDefinition = ReturnTypeDefinition?.CreateSymbol(ns)
            };

            return symbol;
        }


    }
}
