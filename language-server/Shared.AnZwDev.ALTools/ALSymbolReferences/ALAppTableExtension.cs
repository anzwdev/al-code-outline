using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTableExtension : ALAppTable, IALAppObjectExtension
    {

        public string TargetObject { get; set; }
        public ALAppSymbolsCollection<ALAppTableField> FieldModifications { get; set; }

        public ALAppTableExtension()
        {
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.extends = this.TargetObject;
            return symbol;
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.TableExtensionObject;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.TableExtension ;
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
