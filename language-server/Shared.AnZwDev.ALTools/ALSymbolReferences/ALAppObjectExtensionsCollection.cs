using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectExtensionsCollection<T> : ALAppObjectsCollection<T> where T : ALAppObject, IALAppObjectExtension
    {

        // !!! TO-DO !!!
        // !!! Implement faster search for object extensions !!!
        private readonly DictionaryWithDuplicates<string, T> _objectsByBaseObjectName = new DictionaryWithDuplicates<string, T>(StringComparer.OrdinalIgnoreCase);

        protected override void OnItemAdded(T item)
        {
            base.OnItemAdded(item);
            //var sourceName = item.GetTargetObjectName();
            //_objectsByBaseObjectName.Add(item.Name, item);
        }

        protected override void OnItemRemoved(T item)
        {
            base.OnItemRemoved(item);
            //_objectsByBaseObjectName.Remove(item.Name, item);
        }

        protected override void OnClear()
        {
            base.OnClear();
            //_objectsByBaseObjectName.Clear();
        }


        public T FindFirstObjectExtension(ALObjectIdentifier extendedObjectIdentifier, bool includeInternal = true)
        {
            foreach (var item in this)
                if (extendedObjectIdentifier.IsReferencedBy(item.GetTargetObjectReference()))
                    return item;

            return null;
        }


    }
}
