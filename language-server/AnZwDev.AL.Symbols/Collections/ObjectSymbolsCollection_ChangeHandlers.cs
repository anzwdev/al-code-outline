using AnZwDev.System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class ObjectSymbolsCollection<T> : ExtendableList<T>, IObjectSymbolsCollection<T>, IObjectSymbolsCollection where T : ObjectSymbol
    {

        protected override void OnItemAdded(T item)
        {
            base.OnItemAdded(item);
            if (item.Identifier.Id != 0)
                _objectsById.Add(item.Identifier.Id, item);
            _objectsByName.Add(item.Identifier.FullyQualifiedName.Name, item);
            if (!String.IsNullOrEmpty(item.ReferenceSourceFileName))
                _objectsBySourceFile.Add(item.ReferenceSourceFileName, item);
        }

        protected override void OnItemRemoved(T item)
        {
            base.OnItemRemoved(item);
            if (item.Identifier.Id != 0)
                _objectsById.Remove(item.Identifier.Id, item);
            _objectsByName.Remove(item.Identifier.FullyQualifiedName.Name, item);
            if (!String.IsNullOrEmpty(item.ReferenceSourceFileName))
                _objectsBySourceFile.Remove(item.ReferenceSourceFileName, item);
        }

        protected override void OnClear()
        {
            base.OnClear();
            _objectsById.Clear();
            _objectsByName.Clear();
            _objectsBySourceFile.Clear();
        }

    }
}
