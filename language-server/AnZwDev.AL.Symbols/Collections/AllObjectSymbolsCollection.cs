using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class AllObjectSymbolsCollection : IAllObjectSymbolsCollection
    {

        private static Dictionary<ObjectKind, Func<ApplicationSymbol, IObjectSymbolsCollection>> _objectCollections = new() 
        {
            { ObjectKind.TableData, (app) => app.Tables.TableData },
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

        public ApplicationSymbol ApplicationSymbol { get; }

        public AllObjectSymbolsCollection(ApplicationSymbol applicationSymbol)
        {
            ApplicationSymbol = applicationSymbol;
        }

        public void Add(ObjectSymbol symbol)
        {            
            if (_objectCollections.ContainsKey(symbol.Identifier.ObjectKind))
            {
                var collection = _objectCollections[symbol.Identifier.ObjectKind](ApplicationSymbol);
                collection.Add(symbol);
            }
        }

        public void RemoveReferenceSourceFileName(string referenceSourceFileName)
        {
            foreach (var collectionFunc in _objectCollections.Values)
            {
                var collection = collectionFunc(ApplicationSymbol);
                collection.RemoveReferenceSourceFileName(referenceSourceFileName);
            }
        }

        public void RenameReferenceSourceFileName(string oldReferenceSourceFileName, string newReferenceSourceFileName)
        {
            foreach (var collectionFunc in _objectCollections.Values)
            {
                var collection = collectionFunc(ApplicationSymbol);
                collection.RenameReferenceSourceFileName(oldReferenceSourceFileName, newReferenceSourceFileName);
            }
        }

        public bool UsesNamespaces()
        {
            foreach (var collectionFunc in _objectCollections.Values)
            {
                var collection = collectionFunc(ApplicationSymbol);
                if (collection.UsesNamespaces())
                    return true;
            }
            return false;
        }

    }
}
