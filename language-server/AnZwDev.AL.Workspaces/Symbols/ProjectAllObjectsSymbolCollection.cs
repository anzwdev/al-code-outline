using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces.Symbols
{
    public class ProjectAllObjectsSymbolCollection
    {

        private static Dictionary<ObjectKind, Func<ProjectSymbolsView, IProjectObjectSymbolCollection>> _objectCollections = new()
        {
            { ObjectKind.Table, (app) => app.Tables },
            { ObjectKind.Page, (app) => app.Pages },
            { ObjectKind.Report, (app) => app.Reports },
            { ObjectKind.XmlPort, (app) => app.XmlPorts },
            { ObjectKind.Query, (app) => app.Queries },
            { ObjectKind.Codeunit, (app) => app.Codeunits },
            { ObjectKind.ControlAddIn, (app) => app.ControlAddIns },
            { ObjectKind.PageExtension, (app) => app.PageExtensions },
            { ObjectKind.TableExtension, (app) => app.TableExtensions },
            { ObjectKind.Profile, (app) => app.Profiles },
            { ObjectKind.ProfileExtension, (app) => app.ProfileExtensions },
            { ObjectKind.PageCustomization, (app) => app.PageCustomizations },
            { ObjectKind.DotNetPackage, (app) => app.DotNetPackages },
            { ObjectKind.EnumType, (app) => app.EnumTypes },
            { ObjectKind.EnumExtensionType, (app) => app.EnumExtensionTypes },
            { ObjectKind.Interface, (app) => app.Interfaces },
            { ObjectKind.ReportExtension, (app) => app.ReportExtensions },
            { ObjectKind.PermissionSet, (app) => app.PermissionSets },
            { ObjectKind.PermissionSetExtension, (app) => app.PermissionSetExtensions }
        };

        protected ProjectSymbolsView ProjectSymbols { get; }

        public ProjectAllObjectsSymbolCollection(ProjectSymbolsView projectSymbols)
        {
            ProjectSymbols = projectSymbols;
        }

        public IEnumerable<ObjectSymbol> Filter(HashSet<ObjectKind>? objectKindFilter, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            foreach (var objectKind in _objectCollections.Keys)
                if ((objectKindFilter == null) || (objectKindFilter.Count == 0) || (objectKindFilter.Contains(objectKind)))
                {
                    var collection = _objectCollections[objectKind](ProjectSymbols);
                    foreach (var symbol in collection.Filter(appIdFilter, accessLevelFilter))
                        yield return symbol;
                }
        }

        public IEnumerable<ObjectSymbol> Filter(ObjectKind objectKindFilter, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(objectKindFilter))
            {
                var collection = _objectCollections[objectKindFilter](ProjectSymbols);
                foreach (var symbol in collection.Filter(appIdFilter, accessLevelFilter))
                    yield return symbol;
            }
        }

        public ObjectSymbol? FindFirst(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var objectKind = reference.ObjectKind;
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindFirst(reference, appIdFilter, accessLevelFilter);
            }
            return null;
        }

        public ProjectObjectSymbolWithSource<ObjectSymbol> FindFirstWithSource(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var objectKind = reference.ObjectKind;
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindFirstWithSource(reference, appIdFilter, accessLevelFilter);
            }
            return new ProjectObjectSymbolWithSource<ObjectSymbol>()
            {
                Symbol = null,
                Source = null
            };
        }

        public ObjectSymbol? FindFirst(ObjectIdentifier identifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var objectKind = identifier.ObjectKind;
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindFirst(identifier, appIdFilter, accessLevelFilter);
            }
            return null;
        }

        public ProjectObjectSymbolWithSource<ObjectSymbol> FindFirstWithSource(ObjectIdentifier identifier, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var objectKind = identifier.ObjectKind;
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindFirstWithSource(identifier, appIdFilter, accessLevelFilter);
            }
            return new ProjectObjectSymbolWithSource<ObjectSymbol>()
            {
                Symbol = null,
                Source = null
            };
        }

        public ObjectSymbol? FindFirst(ObjectKind objectKind, int id, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindFirst(id, appIdFilter, accessLevelFilter);
            }
            return null;
        }

        public ProjectObjectSymbolWithSource<ObjectSymbol> FindFirstWithSource(ObjectKind objectKind, int id, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindFirstWithSource(id, appIdFilter, accessLevelFilter);
            }
            return new ProjectObjectSymbolWithSource<ObjectSymbol>()
            {
                Symbol = null,
                Source = null
            };
        }

        public IEnumerable<ObjectSymbol> FindAll(ObjectReference reference, HashSet<string>? appIdFilter = null, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var objectKind = reference.ObjectKind;
            if (_objectCollections.ContainsKey(objectKind))
            {
                var collection = _objectCollections[objectKind](ProjectSymbols);
                return collection.FindAll(reference, appIdFilter, accessLevelFilter);
            }
            return Enumerable.Empty<ObjectSymbol>();
        }

    }
}
