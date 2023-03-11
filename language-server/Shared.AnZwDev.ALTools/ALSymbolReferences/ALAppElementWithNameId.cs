using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppElementWithNameId : ALAppElementWithName
    {

        public int Id { get; set; }

        public ALAppElementWithNameId()
        {
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            return new ALSymbol(this.GetALSymbolKind(), this.Name, this.Id);
        }

    }
}
