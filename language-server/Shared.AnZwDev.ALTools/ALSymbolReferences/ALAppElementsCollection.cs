using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppElementsCollection<T> : List<T>, IALAppElementsCollection where T : ALAppBaseElement
    {

        public ALAppElementsCollection()
        {
        }

        public void AddBaseElement(ALAppBaseElement element)
        {
            this.Add((T)element);
        }

        public void RemoveBaseElement(ALAppBaseElement element)
        {
            this.Remove((T)element);
        }

        public void ReplaceBaseElement(ALAppBaseElement element)
        {
            T existingItem = this.FindElement((T)element);
            if (existingItem != null)
                this.Remove(existingItem);
            this.Add((T)element);
        }

        protected virtual T FindElement(T element)
        {
            return null;
        }

        #region ALSymbolInformation conversion

        public void AddToALSymbol(ALSymbol symbol)
        {
            this.AddToALSymbol(symbol, ALSymbolKind.Undefined, null, ALSymbolKind.Undefined);
        }

        public void AddCollectionToALSymbol(ALSymbol symbol, ALSymbolKind collectionKind)
        {
            this.AddToALSymbol(symbol, collectionKind, collectionKind.ToName(), ALSymbolKind.Undefined);
        }

        public void AddToALSymbol(ALSymbol symbol, ALSymbolKind collectionKind, string collectionName)
        {
            this.AddToALSymbol(symbol, collectionKind, collectionName, ALSymbolKind.Undefined);
        }

        public void AddToALSymbol(ALSymbol symbol, ALSymbolKind collectionKind, string collectionName, ALSymbolKind firstItemSymbolKind)
        {
            if (this.Count > 0)
            {
                ALSymbol collectionSymbol = symbol;
                if (!String.IsNullOrWhiteSpace(collectionName))
                {
                    collectionSymbol = new ALSymbol(collectionKind, collectionName);
                    symbol.AddChildSymbol(collectionSymbol);
                }

                for (int i = 0; i < this.Count; i++)
                {
                    if ((i == 0) && (firstItemSymbolKind != ALSymbolKind.Undefined))
                    {
                        ALSymbol itemSymbol = this[i].ToALSymbol();
                        itemSymbol.kind = firstItemSymbolKind;
                        collectionSymbol.AddChildSymbol(itemSymbol);
                    }
                    else
                        collectionSymbol.AddChildSymbol(this[i].ToALSymbol());
                }
            }
        }

        #endregion

    }
}
