using AnZwDev.System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{

    public partial class ObjectExtensionSymbolsCollection<T>: ObjectSymbolsCollection<T>, IObjectExtensionSymbolsCollection<T> where T : ObjectExtensionSymbol
    {

        public IEnumerable<T> FindExtensions(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            for (int i = 0; i < this.Count; i++)
            {
                var extension = this[i];
                if ((extension.ExtendedObjectReference.References(objectIdentifier)) && 
                    (accessLevelFilter.Valid(extension.AccessLevel)))
                    yield return extension;
            }
        }

    }

}
