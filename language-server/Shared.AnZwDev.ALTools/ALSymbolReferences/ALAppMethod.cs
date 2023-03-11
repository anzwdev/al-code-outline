using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppMethod : ALAppElementWithName
    {

        public bool IsInternal { get; set; }
        public bool IsProtected { get; set; }
        public bool IsLocal { get; set; }
        public string AccessModifier { get; set; }
        public int MethodKind { get; set; }
        public ALAppElementsCollection<ALAppMethodParameter> Parameters { get; set; }
        public ALAppElementsCollection<ALAppAttribute> Attributes { get; set; }
        public ALAppTypeDefinition ReturnTypeDefinition { get; set; }

        public ALAppMethod()
        {
        }

        public override ALSymbolAccessModifier? GetAccessModifier()
        {
            if (this.IsInternal)
                return ALSymbolAccessModifier.Internal;
            if (this.IsProtected)
                return ALSymbolAccessModifier.Protected;
            if (this.IsLocal)
                return ALSymbolAccessModifier.Local;

            return base.GetAccessModifier();
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            if (this.Attributes != null)
            {
                for (int i=0; i<this.Attributes.Count; i++)
                {
                    ALSymbolKind kind = ALSyntaxHelper.MemberAttributeToMethodKind(this.Attributes[i].Name);
                    if (kind != ALSymbolKind.Undefined)
                        return kind;
                }
            }
            if (this.IsInternal)
                return ALSymbolKind.InternalMethodDeclaration;
            if (this.IsProtected)
                return ALSymbolKind.ProtectedMethodDeclaration;
            if (this.IsLocal)
                return ALSymbolKind.LocalMethodDeclaration;
            return ALSymbolKind.MethodDeclaration;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = this.GetShortHeaderSourceCode();
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Parameters?.AddToALSymbol(symbol, ALSymbolKind.ParameterList, "parameters");
            base.AddChildALSymbols(symbol);
        }

        protected string GetShortHeaderSourceCode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            this.AppendShortHeaderSourceCode(stringBuilder);
            return stringBuilder.ToString();
        }

        protected void AppendShortHeaderSourceCode(StringBuilder builder)
        {
            builder.Append(ALSyntaxHelper.EncodeName(this.Name));
            builder.Append("(");
            if (this.Parameters != null)
            {
                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    if (i > 0)
                        builder.Append(", ");
                    this.Parameters[i].AppendDefinitionSourceCode(builder);
                }
            }
            builder.Append(")");
            if ((this.ReturnTypeDefinition != null) && (!this.ReturnTypeDefinition.IsEmpty()))
            {
                builder.Append(": ");
                this.ReturnTypeDefinition.AppendDefinitionSourceCode(builder);
            }
        }

        public string GetHeaderSourceCode()
        {
            StringBuilder builder = new StringBuilder();
            this.AppendHeaderSourceCode(builder);
            return builder.ToString();
        }

        public string GetAccessModifierName()
        {
            if (this.IsInternal)
                return "internal";
            if (this.IsProtected)
                return "protected";
            if (this.IsLocal)
                return "local";
            return null;
        }

        public void AppendHeaderSourceCode(StringBuilder builder)
        {
            var accessModifier = GetAccessModifierName();
            if (!String.IsNullOrWhiteSpace(accessModifier))
            {
                builder.Append(accessModifier);
                builder.Append(" ");
            }

            builder.Append("procedure ");
            builder.Append(this.Name);
            builder.Append("(");

            if (this.Parameters != null)
            {
                for (int i=0; i<this.Parameters.Count; i++)
                {
                    if (i != 0)
                        builder.Append("; ");
                    this.Parameters[i].AppendDefinitionSourceCode(builder);
                }
            }

            builder.Append(")");
            if ((this.ReturnTypeDefinition != null) && (!this.ReturnTypeDefinition.IsEmpty()))
            {
                builder.Append(": ");
                this.ReturnTypeDefinition.AppendDefinitionSourceCode(builder);
            }

        }

    }
}
