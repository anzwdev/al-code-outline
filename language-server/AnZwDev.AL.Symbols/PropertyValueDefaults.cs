using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols
{
    public static class PropertyValueDefaults
    {

        public static readonly bool Enabled = true;
        public static readonly AccessLevel Access = AccessLevel.Public;
        public static readonly ObsoleteState ObsoleteState = ObsoleteState.No;
        public static readonly FieldClass FieldClass = FieldClass.Normal;
        public static readonly Label Label = new Label();

    }
}
