using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageControl : ALAppElementWithName
    {


        //public string Expression { get; set; }
        public ALAppPageControlKind Kind { get; set; }
        public ALAppTypeDefinition TypeDefinition { get; set; }
        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }

        public ALAppPageControl()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return this.Kind.ToALSymbolKind();
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Controls?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

        public string GetSourceExpression()
        {
            if (this.Properties != null)
                return this.Properties.GetProperty("SourceExpression")?.Value;
            return null;
        }
       
        public void SetSourceExpression(string value)
        {
            if (this.Properties == null)
                this.Properties = new ALAppPropertiesCollection();
            this.Properties.GetOrCreateProperty("SourceExpression").Value = value;
        }

        public string GetSourceExpressionWithoutRec()
        {
            string expression = this.GetSourceExpression();
            ALMemberAccessExpression memberAccessExpression = ALSyntaxHelper.DecodeMemberAccessExpression(expression);
            if (String.IsNullOrWhiteSpace(memberAccessExpression.Name))
                return memberAccessExpression.Expression;
            else if (memberAccessExpression.Name.Equals("rec", StringComparison.CurrentCultureIgnoreCase))
                return memberAccessExpression.Expression;
            return null;
        }


    }
}
