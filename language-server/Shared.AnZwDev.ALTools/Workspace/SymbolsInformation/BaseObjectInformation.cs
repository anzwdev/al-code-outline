using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseObjectInformation : SymbolWithIdInformation
    {

        public BaseObjectInformation()
        {
        }

        public BaseObjectInformation(ALAppObject alAppObject) : base(alAppObject)
        {
            if (alAppObject.Properties != null)
                this.Caption = alAppObject.Properties.GetValue("Caption");
        }
    }
}
