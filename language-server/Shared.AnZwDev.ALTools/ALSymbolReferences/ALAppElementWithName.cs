using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppElementWithName : ALAppBaseElement
    {

        public string Name { get; set; }

        public ALAppElementWithName()
        {
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            return new ALSymbol(this.GetALSymbolKind(), this.Name);
        }

        public override int CompareTo(object obj)
        {
            ALAppElementWithName appElement = obj as ALAppElementWithName;
            if ((this.Name == null) || (appElement == null) || (appElement.Name == null))
                return 0;
            return String.Compare(this.Name, appElement.Name, true);
        }


    }
}
