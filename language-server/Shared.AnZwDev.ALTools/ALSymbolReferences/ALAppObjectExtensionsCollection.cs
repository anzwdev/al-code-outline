using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectExtensionsCollection<T> : ALAppObjectsCollection<T> where T : ALAppObject, IALAppObjectExtension
    {

        private readonly DictionaryWithDuplicates<string, T> _objectsByBaseObjectName = new DictionaryWithDuplicates<string, T>(StringComparer.OrdinalIgnoreCase);
        private readonly DictionaryWithDuplicates<int, T> _objectsByBaseObjectId = new DictionaryWithDuplicates<int, T>();

        public ALAppObjectExtensionsCollection()
        {
        }

        public ALAppObjectExtensionsCollection(ALBaseSymbolReference symbolReference) : base(symbolReference)
        {
        }

        protected override void OnItemAdded(T item)
        {
            base.OnItemAdded(item);

            var objectReference = item.GetTargetObjectReference();
            if (!String.IsNullOrEmpty(objectReference.Name))
                _objectsByBaseObjectName.Add(objectReference.Name, item);
            if (objectReference.ObjectId != 0)
                _objectsByBaseObjectId.Add(objectReference.ObjectId, item);
        }

        protected override void OnItemRemoved(T item)
        {
            base.OnItemRemoved(item);

            var objectReference = item.GetTargetObjectReference();
            if (!String.IsNullOrEmpty(objectReference.Name))
                _objectsByBaseObjectName.Remove(objectReference.Name, item);
            if (objectReference.ObjectId != 0)
                _objectsByBaseObjectId.Remove(objectReference.ObjectId, item);
        }

        protected override void OnClear()
        {
            base.OnClear();
            _objectsByBaseObjectName.Clear();
            _objectsByBaseObjectId.Clear();
        }

        public IEnumerable<T> FindObjectExtensions(ALObjectIdentifier extendedObjectIdentifier, bool includeInternal = true)
        {
            //find by name
            var targetReferenceName = extendedObjectIdentifier.Name;
            if (!String.IsNullOrEmpty(targetReferenceName))
            {
                if (_objectsByBaseObjectName.ContainsSingleElementKey(targetReferenceName))
                {
                    var extensionObject = _objectsByBaseObjectName.GetSingleValue(targetReferenceName);
                    if (extendedObjectIdentifier.IsReferencedBy(extensionObject.GetTargetObjectReference()))
                        yield return extensionObject;
                }
                else
                {
                    var extensionObjectsCollection = _objectsByBaseObjectName.GetMultipleValues(targetReferenceName);
                    if (extensionObjectsCollection != null)
                        foreach (var extensionObject in extensionObjectsCollection)
                            if (extendedObjectIdentifier.IsReferencedBy(extensionObject.GetTargetObjectReference()))
                                yield return extensionObject;
                }
            }

            //find by id
            if (extendedObjectIdentifier.Id != 0)
            {
                if (_objectsByBaseObjectId.ContainsSingleElementKey(extendedObjectIdentifier.Id))
                {
                    yield return _objectsByBaseObjectId.GetSingleValue(extendedObjectIdentifier.Id);
                }
                else
                {
                    var extensionObjectsCollection = _objectsByBaseObjectId.GetMultipleValues(extendedObjectIdentifier.Id);
                    if (extensionObjectsCollection != null)
                        foreach (var extensionObject in extensionObjectsCollection)
                            yield return extensionObject;
                }
            }
        }

    }
}
