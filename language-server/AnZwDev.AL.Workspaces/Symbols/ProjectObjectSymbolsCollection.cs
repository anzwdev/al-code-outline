using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public partial class ProjectObjectSymbolsCollection<T> : IProjectObjectSymbolCollection where T : ObjectSymbol
    {

        protected ProjectSymbolsProvider ProjectSymbolsProvider { get; }
        private readonly Func<ApplicationSymbol, ObjectSymbolsCollection<T>> _getSymbolsCollection;

        public ProjectObjectSymbolsCollection(ProjectSymbolsProvider symbolsProvider, Func<ApplicationSymbol, ObjectSymbolsCollection<T>> getSymbolsCollection)
        {
            ProjectSymbolsProvider = symbolsProvider;
            _getSymbolsCollection = getSymbolsCollection;
        }

        public IEnumerable<T> Filter(HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var allSymbols = ProjectSymbolsProvider.GetSymbols(appIdFilter);
            foreach (var symbolDescription in allSymbols)
            {
                var objectsCollection = _getSymbolsCollection(symbolDescription.Symbol);
                if (objectsCollection != null)
                {
                    var collectionAccessLevel = (accessLevelFilter == AccessLevelFilter.Accessible) ? symbolDescription.AccessLevelFilter : accessLevelFilter;
                    var filteredObjects = objectsCollection.Filter(collectionAccessLevel);
                    foreach (var obj in filteredObjects)
                        yield return obj;
                }
            }
        }

        public T? FindFirst(int id, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return FindFirstWithSource(id, appIdFilter, accessLevelFilter).Symbol;
        }

        public ProjectObjectSymbolWithSource<T> FindFirstWithSource(int id, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var allSymbols = ProjectSymbolsProvider.GetSymbols(appIdFilter);
            foreach (var symbolDescription in allSymbols)
            {
                var objectsCollection = _getSymbolsCollection(symbolDescription.Symbol);
                if (objectsCollection != null)
                {
                    var collectionAccessLevel = (accessLevelFilter == AccessLevelFilter.Accessible) ? symbolDescription.AccessLevelFilter : accessLevelFilter;
                    var objectSymbol = objectsCollection.FindFirst(id, accessLevelFilter);
                    if (objectSymbol != null)
                        return new ProjectObjectSymbolWithSource<T>()
                        {
                            Symbol = objectSymbol,
                            Source = symbolDescription.Symbol
                        };
                }
            }
            return new ProjectObjectSymbolWithSource<T>()
            {
                Symbol = null,
                Source = null
            };
        }

        public T? FindFirst(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return FindFirstWithSource(reference, appIdFilter, accessLevelFilter).Symbol;
        }

        public ProjectObjectSymbolWithSource<T> FindFirstWithSource(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var allSymbols = ProjectSymbolsProvider.GetSymbols(appIdFilter);
            foreach (var symbolDescription in allSymbols)
            {
                var objectsCollection = _getSymbolsCollection(symbolDescription.Symbol);
                if (objectsCollection != null)
                {
                    var collectionAccessLevel = (accessLevelFilter == AccessLevelFilter.Accessible) ? symbolDescription.AccessLevelFilter : accessLevelFilter;
                    var objectSymbol = objectsCollection.FindFirst(reference, accessLevelFilter);
                    if (objectSymbol != null)
                        return new ProjectObjectSymbolWithSource<T>()
                        {
                            Symbol = objectSymbol,
                            Source = symbolDescription.Symbol
                        };
                }
            }
            return new ProjectObjectSymbolWithSource<T>()
            {
                Symbol = null,
                Source = null
            };
        }

        public T? FindFirst(ObjectIdentifier identifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return FindFirstWithSource(identifier, appIdFilter, accessLevelFilter).Symbol;
        }

        public ProjectObjectSymbolWithSource<T> FindFirstWithSource(ObjectIdentifier identifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var allSymbols = ProjectSymbolsProvider.GetSymbols(appIdFilter);
            foreach (var symbolDescription in allSymbols)
            {
                var objectsCollection = _getSymbolsCollection(symbolDescription.Symbol);
                if (objectsCollection != null)
                {
                    var collectionAccessLevel = (accessLevelFilter == AccessLevelFilter.Accessible) ? symbolDescription.AccessLevelFilter : accessLevelFilter;
                    var objectSymbol = objectsCollection.FindFirst(identifier, accessLevelFilter);
                    if (objectSymbol != null)
                        return new ProjectObjectSymbolWithSource<T>()
                        {
                            Symbol = objectSymbol,
                            Source = symbolDescription.Symbol
                        };
                }
            }
            return new ProjectObjectSymbolWithSource<T>()
            {
                Symbol = null,
                Source = null
            };
        }

        public IEnumerable<T> FindAll(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var allSymbols = ProjectSymbolsProvider.GetSymbols(appIdFilter);
            foreach (var symbolDescription in allSymbols)
            {
                var objectsCollection = _getSymbolsCollection(symbolDescription.Symbol);
                if (objectsCollection != null)
                {
                    var collectionAccessLevel = (accessLevelFilter == AccessLevelFilter.Accessible) ? symbolDescription.AccessLevelFilter : accessLevelFilter;
                    var filteredObjects = objectsCollection.FindAll(reference, accessLevelFilter);
                    foreach (var obj in filteredObjects)
                        yield return obj;
                }
            }
        }

        IEnumerable<ObjectSymbol> IProjectObjectSymbolCollection.Filter(HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            return Filter(appIdFilter, accessLevelFilter);
        }

        ObjectSymbol? IProjectObjectSymbolCollection.FindFirst(ObjectReference reference, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            return FindFirst(reference, appIdFilter, accessLevelFilter);
        }

        ObjectSymbol? IProjectObjectSymbolCollection.FindFirst(ObjectIdentifier identifier, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            return FindFirst(identifier, appIdFilter, accessLevelFilter);
        }

        ObjectSymbol? IProjectObjectSymbolCollection.FindFirst(int id, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            return FindFirst(id, appIdFilter, accessLevelFilter);
        }

        ProjectObjectSymbolWithSource<ObjectSymbol> IProjectObjectSymbolCollection.FindFirstWithSource(ObjectReference reference, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            var symbolWithSource = FindFirstWithSource(reference, appIdFilter, accessLevelFilter);
            return new ProjectObjectSymbolWithSource<ObjectSymbol>()
            {
                Symbol = symbolWithSource.Symbol,
                Source = symbolWithSource.Source
            };
        }

        ProjectObjectSymbolWithSource<ObjectSymbol> IProjectObjectSymbolCollection.FindFirstWithSource(ObjectIdentifier identifier, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            var symbolWithSource = FindFirstWithSource(identifier, appIdFilter, accessLevelFilter);
            return new ProjectObjectSymbolWithSource<ObjectSymbol>()
            {
                Symbol = symbolWithSource.Symbol,
                Source = symbolWithSource.Source
            };
        }

        ProjectObjectSymbolWithSource<ObjectSymbol> IProjectObjectSymbolCollection.FindFirstWithSource(int id, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            var symbolWithSource = FindFirstWithSource(id, appIdFilter, accessLevelFilter);
            return new ProjectObjectSymbolWithSource<ObjectSymbol>()
            {
                Symbol = symbolWithSource.Symbol,
                Source = symbolWithSource.Source
            };
        }

        IEnumerable<ObjectSymbol> IProjectObjectSymbolCollection.FindAll(ObjectReference reference, HashSet<string>? appIdFilter, AccessLevelFilter accessLevelFilter)
        {
            return FindAll(reference, appIdFilter, accessLevelFilter);
        }

    }
}
