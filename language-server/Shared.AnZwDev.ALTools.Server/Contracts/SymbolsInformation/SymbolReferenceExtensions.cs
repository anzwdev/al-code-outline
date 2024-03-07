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
            {
                //!!! TO-DO
                //!!! Decode name with namespace correctly
                //    return new ALObjectReference(symbolReference.usings.ToHashSet(true), symbolReference.nameWithNamespaceOrId);

                if (Int32.TryParse(symbolReference.nameWithNamespaceOrId, out int id))
                    return new ALObjectReference(symbolReference.usings.ToHashSet(true), null, id, null);
                return new ALObjectReference(symbolReference.usings.ToHashSet(true), null, symbolReference.nameWithNamespaceOrId);  //temporary use nameWithNamespaceOrId as name
            }


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
