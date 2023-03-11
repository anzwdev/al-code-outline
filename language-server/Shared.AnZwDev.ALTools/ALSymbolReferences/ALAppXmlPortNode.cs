using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppXmlPortNode: ALAppElementWithName
    {

        public ALAppXmlPortNodeKind Kind { get; set; }
        public string Expression { get; set; }
        public ALAppElementsCollection<ALAppXmlPortNode> Schema { get; set; }

        public ALAppXmlPortNode()
        {
            this.Schema = null;
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            switch (Kind)
            {
                case ALAppXmlPortNodeKind.FieldAttribute:
                    return ALSymbolKind.XmlPortFieldAttribute;
                case ALAppXmlPortNodeKind.FieldElement:
                    return ALSymbolKind.XmlPortFieldElement;
                case ALAppXmlPortNodeKind.TableElement:
                    return ALSymbolKind.XmlPortTableElement;
                case ALAppXmlPortNodeKind.TextElement:
                    return ALSymbolKind.XmlPortTextElement;
                case ALAppXmlPortNodeKind.TextAttribute:
                    return ALSymbolKind.XmlPortTextAttribute;
            }
            return ALSymbolKind.QueryObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Schema?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

        public void AddChildNode(ALAppXmlPortNode node)
        {
            if (this.Schema == null)
                this.Schema = new ALAppElementsCollection<ALAppXmlPortNode>();
            this.Schema.Add(node);
        }

    }
}
