using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class MergedAllALAppObjectsCollection
    {
        
        protected MergedALAppSymbolReference AllSymbolsReference { get; }
        protected List<IReadOnlyALAppObjectsCollection> ALObjectsCollectionsList { get; }
        protected Dictionary<ALSymbolKind, IReadOnlyALAppObjectsCollection> ALObjectsCollectionsDictionary { get; }

        public MergedAllALAppObjectsCollection(MergedALAppSymbolReference allSymbolsReference)
        {
            this.AllSymbolsReference = allSymbolsReference;
            this.ALObjectsCollectionsList = new List<IReadOnlyALAppObjectsCollection>();
            this.ALObjectsCollectionsDictionary = new Dictionary<ALSymbolKind, IReadOnlyALAppObjectsCollection>();
            
            this.AddObjectCollections(
                allSymbolsReference.Tables, 
                allSymbolsReference.TableExtensions,
                allSymbolsReference.Codeunits, 
                allSymbolsReference.ControlAddIns, 
                allSymbolsReference.DotNetPackages,
                allSymbolsReference.EnumTypes, 
                allSymbolsReference.EnumExtensionTypes, 
                allSymbolsReference.Interfaces,
                allSymbolsReference.Pages, 
                allSymbolsReference.PageCustomizations, 
                allSymbolsReference.PageExtensions,
                allSymbolsReference.PermissionSets, 
                allSymbolsReference.PermissionSetExtensions, 
                allSymbolsReference.Profiles,
                allSymbolsReference.Queries,
                allSymbolsReference.Reports,
                allSymbolsReference.ReportExtensions,
                allSymbolsReference.XmlPorts);
        }

        protected void AddObjectCollections(params IReadOnlyALAppObjectsCollection[] alObjectsCollections)
        {
            for (int i=0; i<alObjectsCollections.Length; i++)
            {
                IReadOnlyALAppObjectsCollection collection = alObjectsCollections[i];
                this.ALObjectsCollectionsList.Add(collection);
                this.ALObjectsCollectionsDictionary.Add(collection.ALSymbolKind, collection);
            }
        }

        public IEnumerable<ALAppObject> GetObjects(HashSet<ALSymbolKind> includeObjects)
        {
            for (int i=0; i<this.ALObjectsCollectionsList.Count; i++)
            {
                IReadOnlyALAppObjectsCollection collection = this.ALObjectsCollectionsList[i];
                if ((includeObjects == null) || (includeObjects.Contains(collection.ALSymbolKind)))
                {
                    IEnumerable<ALAppObject> allObjects = collection.GetObjects();
                    foreach (ALAppObject alAppObject in allObjects)
                        yield return alAppObject;
                }
            }
        }

        public IEnumerable<ALAppObject> GetObjects()
        {
            return this.GetObjects(null);
        }

        public IEnumerable<long> GetIds(HashSet<ALSymbolKind> includeObjects)
        {
            for (int i = 0; i < this.ALObjectsCollectionsList.Count; i++)
            {
                IReadOnlyALAppObjectsCollection collection = this.ALObjectsCollectionsList[i];
                if ((includeObjects == null) || (includeObjects.Contains(collection.ALSymbolKind)))
                {
                    IEnumerable<ALAppObject> allObjects = collection.GetObjects();
                    foreach (ALAppObject alAppObject in allObjects)
                        if ((alAppObject != null) && (alAppObject.Id != 0))
                            yield return alAppObject.Id;
                }
            }
        }

        public IEnumerable<long> GetIds()
        {
            return this.GetIds(null);
        }

    }
}
