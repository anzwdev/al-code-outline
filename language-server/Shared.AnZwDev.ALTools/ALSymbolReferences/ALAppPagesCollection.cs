using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPagesCollection : ALAppObjectsCollection<ALAppPage>
    {

        private readonly DictionaryWithDuplicates<string, ALAppPage> _objectsBySourceObjectName = new DictionaryWithDuplicates<string, ALAppPage>(StringComparer.OrdinalIgnoreCase);
        private readonly DictionaryWithDuplicates<int, ALAppPage> _objectsBySourceObjectId = new DictionaryWithDuplicates<int, ALAppPage>();

        public ALAppPagesCollection()
        {
        }

        public ALAppPagesCollection(ALBaseSymbolReference symbolReference) : base(symbolReference)
        {
        }

        protected override void OnItemAdded(ALAppPage item)
        {
            base.OnItemAdded(item);

            var objectReference = item.GetSourceTable();
            if (!String.IsNullOrEmpty(objectReference.Name))
                _objectsBySourceObjectName.Add(objectReference.Name, item);
            if (objectReference.ObjectId != 0)
                _objectsBySourceObjectId.Add(objectReference.ObjectId, item);
        }

        protected override void OnItemRemoved(ALAppPage item)
        {
            base.OnItemRemoved(item);

            var objectReference = item.GetSourceTable();
            if (!String.IsNullOrEmpty(objectReference.Name))
                _objectsBySourceObjectName.Remove(objectReference.Name, item);
            if (objectReference.ObjectId != 0)
                _objectsBySourceObjectId.Remove(objectReference.ObjectId, item);
        }

        protected override void OnClear()
        {
            base.OnClear();
            _objectsBySourceObjectName.Clear();
            _objectsBySourceObjectId.Clear();
        }

        public IEnumerable<ALAppPage> FindPagesForTable(ALObjectIdentifier tableObjectIdentifier, bool includeInternal = true)
        {
            //find by name
            var targetReferenceName = tableObjectIdentifier.Name;
            if (!String.IsNullOrEmpty(targetReferenceName))
            {
                if (_objectsBySourceObjectName.ContainsSingleElementKey(targetReferenceName))
                {
                    var pageObject = _objectsBySourceObjectName.GetSingleValue(targetReferenceName);
                    if (tableObjectIdentifier.IsReferencedBy(pageObject.GetSourceTable()))
                        yield return pageObject;
                }
                else
                {
                    var pageObjectsCollection = _objectsBySourceObjectName.GetMultipleValues(targetReferenceName);
                    if (pageObjectsCollection != null)
                        foreach (var pageObject in pageObjectsCollection)
                            if (tableObjectIdentifier.IsReferencedBy(pageObject.GetSourceTable()))
                                yield return pageObject;
                }
            }

            //find by id
            if (tableObjectIdentifier.Id != 0)
            {
                if (_objectsBySourceObjectId.ContainsSingleElementKey(tableObjectIdentifier.Id))
                {
                    yield return _objectsBySourceObjectId.GetSingleValue(tableObjectIdentifier.Id);
                }
                else
                {
                    var pageObjectsCollection = _objectsBySourceObjectId.GetMultipleValues(tableObjectIdentifier.Id);
                    if (pageObjectsCollection != null)
                        foreach (var pageObject in pageObjectsCollection)
                            yield return pageObject;
                }
            }
        }


    }
}
