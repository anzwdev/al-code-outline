using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class ALAppObjectWithExtensions<OT, ET> where OT: ALAppObject where ET : ALAppObject, IALAppObjectExtension
    {

        public OT ALAppObject { get; set; }
        public List<ET> ALAppObjectExtensions { get; set; }

        public ALAppObjectWithExtensions() : this(null)
        {
        }

        public ALAppObjectWithExtensions(OT alAppObject)
        {
            this.ALAppObject = alAppObject;
            this.ALAppObjectExtensions = null;
        }

        public void AddExtension(ET alAppObjectExtension)
        {
            if (this.ALAppObjectExtensions == null)
                this.ALAppObjectExtensions= new List<ET>();
            this.ALAppObjectExtensions.Add(alAppObjectExtension);
        }

    }
}
