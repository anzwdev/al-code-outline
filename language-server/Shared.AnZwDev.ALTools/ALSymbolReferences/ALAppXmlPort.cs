using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppXmlPort : ALAppObject
    {

        public ALAppRequestPage RequestPage { get; set; }
        public ALAppElementsCollection<ALAppXmlPortNode> Schema { get; set; }

        public ALAppXmlPort()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.XmlPortObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Schema?.AddToALSymbol(symbol, ALSymbolKind.QueryElements, "schema");
            base.AddChildALSymbols(symbol);
            if (this.RequestPage != null)
                symbol.AddChildSymbol(this.RequestPage.ToALSymbol());
            base.AddChildALSymbols(symbol);
        }

        public ALAppXmlPortNode FindNode(string name, ALAppXmlPortNodeKind nodeKind)
        {
            if ((this.Schema != null) && (!String.IsNullOrWhiteSpace(name)))
                return this.FindNode(this.Schema, name, nodeKind);
            return null;
        }

        protected ALAppXmlPortNode FindNode(List<ALAppXmlPortNode> nodes, string name, ALAppXmlPortNodeKind nodeKind)
        {
            for (int i=0; i<nodes.Count; i++)
            {
                if ((nodes[i].Kind == nodeKind) && (name.Equals(nodes[i].Name, StringComparison.CurrentCultureIgnoreCase)))
                    return nodes[i];
            }
            
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Schema != null)
                {
                    ALAppXmlPortNode foundNode = this.FindNode(nodes[i].Schema, name, nodeKind);
                    if (foundNode != null)
                        return foundNode;
                }
            }

            return null;
        }


    }
}
