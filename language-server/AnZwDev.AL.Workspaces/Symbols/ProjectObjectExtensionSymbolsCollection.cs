using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public partial class ProjectObjectExtensionSymbolsCollection<T> : ProjectObjectSymbolsCollection<T> where T : ObjectExtensionSymbol
    {

        private readonly Func<ApplicationSymbol, ObjectExtensionSymbolsCollection<T>> _getExtensionSymbolsCollection;

        public ProjectObjectExtensionSymbolsCollection(ProjectSymbolsProvider symbolsProvider, Func<ApplicationSymbol, ObjectExtensionSymbolsCollection<T>> getSymbolsCollection) : base(symbolsProvider, getSymbolsCollection)
        {
            _getExtensionSymbolsCollection = getSymbolsCollection;
        }

        public IEnumerable<T> FindExtensions(ObjectIdentifier objectIdentifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var allSymbols = ProjectSymbolsProvider.GetSymbols(appIdFilter);
            foreach (var symbolDescription in allSymbols)
            {
                var objectsCollection = _getExtensionSymbolsCollection(symbolDescription.Symbol);
                if (objectsCollection != null)
                {
                    var collectionAccessLevel = (accessLevelFilter == AccessLevelFilter.Accessible) ? symbolDescription.AccessLevelFilter : accessLevelFilter;
                    var filteredObjects = objectsCollection.FindExtensions(objectIdentifier, collectionAccessLevel);
                    foreach (var obj in filteredObjects)
                        yield return obj;
                }
            }
        }

    }
}
