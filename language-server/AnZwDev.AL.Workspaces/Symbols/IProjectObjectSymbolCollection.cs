using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public interface IProjectObjectSymbolCollection
    {

        public IEnumerable<ObjectSymbol> Filter(HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(ObjectIdentifier identifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(int id, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public IEnumerable<ObjectSymbol> FindAll(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);

        public ProjectObjectSymbolWithSource<ObjectSymbol> FindFirstWithSource(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ProjectObjectSymbolWithSource<ObjectSymbol> FindFirstWithSource(ObjectIdentifier identifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ProjectObjectSymbolWithSource<ObjectSymbol> FindFirstWithSource(int id, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);

    }
}
