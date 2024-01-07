using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageExtension : ALAppObject, IALAppObjectExtension
    {

        public string TargetObject { get; set; }

        public ALAppSymbolsCollection<ALAppPageControlChange> ControlChanges { get; set; }
        public ALAppSymbolsCollection<ALAppPageActionChange> ActionChanges { get; set; }

        public ALAppPageExtension()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PageExtensionObject;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.PageExtension;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.extends = this.TargetObject;
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.ControlChanges?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionLayout, "layout");
            this.ActionChanges?.AddToALSymbol(symbol, ALSymbolKind.PageExtensionActionList, "actions");
            base.AddChildALSymbols(symbol);
        }

        public string GetTargetObjectName()
        {
            return this.TargetObject;
        }

        public ALObjectReference GetTargetObjectReference()
        {
            return new ALObjectReference(Usings, this.TargetObject);
        }
    }
}
