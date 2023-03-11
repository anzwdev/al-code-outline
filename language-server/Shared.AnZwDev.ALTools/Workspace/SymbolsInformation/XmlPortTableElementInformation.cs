using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class XmlPortTableElementInformation : TableBasedSymbolInformation
    {

        public XmlPortTableElementInformation()
        {
        }

        public XmlPortTableElementInformation(ALAppXmlPortNode xmlPortTableElement) : base(xmlPortTableElement, xmlPortTableElement.Expression)
        {
        }


    }
}
