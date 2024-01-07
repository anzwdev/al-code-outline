using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public static class SymbolReferenceExtensions
    {

        public static ALObjectReference ToALObjectReference(this SymbolReference symbolReference)
        {
            if (symbolReference == null)
                return new ALObjectReference();

            if (!String.IsNullOrWhiteSpace(symbolReference.nameWithNamespaceOrId))
                return new ALObjectReference(symbolReference.usings.ToHashSet(true), symbolReference.nameWithNamespaceOrId);
            return new ALObjectReference(symbolReference.usings.ToHashSet(true), symbolReference.namespaceName, symbolReference.id, symbolReference.name);
        }

        public static ALObjectIdentifier ToALObjectIdentifier(this SymbolReference symbolReference)
        {
            if (symbolReference == null)
                return new ALObjectIdentifier();
            return new ALObjectIdentifier(symbolReference.namespaceName, symbolReference.id, symbolReference.name);
        }


    }
}
