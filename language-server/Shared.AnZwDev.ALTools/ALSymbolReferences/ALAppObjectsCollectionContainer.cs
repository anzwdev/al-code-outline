using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectsCollectionContainer<T> : IALAppObjectsCollectionContainer where T : ALAppObject
    {

        public ALObjectType ObjectType { get; }
        public ALAppObjectsCollection<T> Collection { get; set; }

        public ALAppObjectsCollectionContainer(ALObjectType objectType)
        {
            ObjectType = objectType;
        }

        public ALAppObjectsCollection<T> GetOrCreateObjectsCollection()
        {
            if (Collection == null)
                Collection = new ALAppObjectsCollection<T>();
            return Collection;
        }

        #region IALAppObjectsCollectionContainer interface implementation

        IALAppObjectsCollection IALAppObjectsCollectionContainer.Objects => Collection;

        IALAppObjectsCollection IALAppObjectsCollectionContainer.GetOrCreateObjectsCollection()
        {
            return GetOrCreateObjectsCollection();
        }

        #endregion

    }

}
