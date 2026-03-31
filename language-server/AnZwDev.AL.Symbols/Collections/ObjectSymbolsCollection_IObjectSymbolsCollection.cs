using AnZwDev.System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class ObjectSymbolsCollection<T> : ExtendableList<T>, IObjectSymbolsCollection<T>, IObjectSymbolsCollection where T : ObjectSymbol
    {

        void IObjectSymbolsCollection.Add(ObjectSymbol symbol)
        {
            this.Add((T)symbol);
        }

        IEnumerable<ObjectSymbol> IObjectSymbolsCollection.Filter(AccessLevelFilter accessLevelFilter)
        {
            return this.Filter(accessLevelFilter);
        }

        ObjectSymbol? IObjectSymbolsCollection.FindFirst(int id, AccessLevelFilter accessLevelFilter)
        {
            return this.FindFirst(id, accessLevelFilter);
        }

        ObjectSymbol? IObjectSymbolsCollection.FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter)
        {
            return this.FindFirst(reference, accessLevelFilter);
        }

        ObjectSymbol? IObjectSymbolsCollection.FindFirst(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter)
        {
            return this.FindFirst(objectIdentifier, accessLevelFilter);
        }

        IEnumerable<ObjectSymbol> IObjectSymbolsCollection.FindAll(ObjectReference objectReference, AccessLevelFilter accessLevelFilter)
        { 
            return this.FindAll(objectReference, accessLevelFilter);
        }

    }
}
