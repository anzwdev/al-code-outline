using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppXmlPortSymbol : AppObjectWithCodeSymbol<XmlPortSymbol>
    {

        public override XmlPortSymbol CreateSymbol(string? ns)
        {
            return new XmlPortSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                Usings = null,
                RequestPage = null,
                Schema = null
            };
        }


    }
}
