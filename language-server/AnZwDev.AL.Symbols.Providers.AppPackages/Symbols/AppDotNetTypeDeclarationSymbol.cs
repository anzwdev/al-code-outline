using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppDotNetTypeDeclarationSymbol : AppSerializedSymbol<DotNetTypeDeclarationSymbol>
    {

        [JsonPropertyName("TypeName")]
        public string? TypeName { get; set; }

        [JsonPropertyName("AliasName")]
        public string? AliasName { get; set; }

        [JsonPropertyName("ReferenceSourceFileName")]
        public string? ReferenceSourceFileName { get; set; }

        public override DotNetTypeDeclarationSymbol CreateSymbol(string? ns)
        {
            return new DotNetTypeDeclarationSymbol()
            {
                TypeName = TypeName,
                AliasName = AliasName,
                ReferenceSourceFileName = ReferenceSourceFileName
            };
        }

    }
}
