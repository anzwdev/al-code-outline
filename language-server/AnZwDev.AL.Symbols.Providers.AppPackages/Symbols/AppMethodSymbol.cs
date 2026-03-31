using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppMethodSymbol : AppBaseMethodSymbol<MethodSymbol>
    {

        public override MethodSymbol CreateSymbol(string? ns)
        {
            var symbol = new MethodSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                MemberKind = GetMemberKind(),
                IsInternal = IsInternal,
                IsLocal = IsLocal,
                IsProtected = IsProtected,
                Parameters = Parameters.CreateSymbolsListOrNull<MethodParameterSymbol, AppMethodParameterSymbol>(ns),
                Attributes = Attributes.CreateSymbolsListOrNull<AttributeSymbol, AppAttributeSymbol>(ns),
                ReturnParameterDefinition = ReturnTypeDefinition?.CreateSymbol(ns)
            };

            return symbol;
        }

        private MemberKind GetMemberKind()
        {
            if (Attributes != null)
                for (int i=0; i< Attributes.Length; i++)
                {
                    var kind = ALSymbolExpressionParser.ParseMemberKind(Attributes[i].Name);
                    if (kind != MemberKind.Undefined)
                        return kind;
                }
            return MemberKind.Undefined;
        }


    }
}
