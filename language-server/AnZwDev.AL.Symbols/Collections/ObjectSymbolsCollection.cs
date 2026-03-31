using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AnZwDev.System.Collections;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class ObjectSymbolsCollection<T> : ExtendableList<T>, IObjectSymbolsCollection<T>, IObjectSymbolsCollection where T : ObjectSymbol
    {

        private readonly ListDictionary<int, T> _objectsById = new ListDictionary<int, T>();
        private readonly ListDictionary<string, T> _objectsByName = new ListDictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        private readonly ListDictionary<string, T> _objectsBySourceFile = new ListDictionary<string, T>();

        public void RemoveReferenceSourceFileName(string referenceSourceFileName)
        {
            if ((!String.IsNullOrEmpty(referenceSourceFileName)) && (_objectsBySourceFile.ContainsKey(referenceSourceFileName)))
            {
                var items = _objectsBySourceFile.GetAll(referenceSourceFileName);
                _objectsBySourceFile.Remove(referenceSourceFileName);
                if (items != null)
                    for (int i=0; i < items.Count; i++)
                        Remove(items[i]);
            }
        }

        public void RenameReferenceSourceFileName(string oldReferenceSourceFileName, string newReferenceSourceFileName)
        {
            if (
                (!String.IsNullOrEmpty(oldReferenceSourceFileName)) &&
                (!String.IsNullOrEmpty(newReferenceSourceFileName)) &&
                (!oldReferenceSourceFileName.Equals(newReferenceSourceFileName)) &&
                (_objectsBySourceFile.ContainsKey(oldReferenceSourceFileName))
                )
            {
                RemoveReferenceSourceFileName(newReferenceSourceFileName);

                var items = _objectsBySourceFile.GetAll(oldReferenceSourceFileName);
                if ((items != null) && (items.Count > 0))
                {
                    for (int i = 0; i < items!.Count; i++)
                        items[i].ReferenceSourceFileName = newReferenceSourceFileName;
                    _objectsBySourceFile.Add(newReferenceSourceFileName, items);
                }
            }
        }

        public bool UsesNamespaces()
        {
            for (int i=0; i < Count; i++)
                if (!String.IsNullOrEmpty(this[i].Identifier.FullyQualifiedName.Namespace))
                    return true;
            return false;
        }

    }
}
