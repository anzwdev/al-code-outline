using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{

    public class ALSymbol
    {
#pragma warning disable IDE1006
        public int? id { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ALSymbolAccessModifier? access { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string subtype { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string format { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string elementsubtype { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string fullName { get; set; }
        
        public ALSymbolKind kind { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string source { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extends { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ALSymbol> childSymbols { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range range { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range selectionRange { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range contentRange { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? containsDiagnostics { get; set; }

        [JsonIgnore]
        public ALSymbol ParentSymbol { get; set; }

#pragma warning restore IDE1006

        public ALSymbol()
        {
            this.id = 0;
            this.childSymbols = null;
            this.fullName = null;
            this.subtype = null;
            this.elementsubtype = null;
            this.source = null;
            this.extends = null;
            this.contentRange = null;
            this.containsDiagnostics = null;
            this.access = null;
        }

        public ALSymbol(ALSymbolKind kindValue, string nameValue) : this()
        {
            this.kind = kindValue;
            this.name = nameValue;
            this.fullName = ALSyntaxHelper.EncodeName(nameValue);
        }

        public ALSymbol(string kindTextValue, string nameValue) : this(ALSymbolKind.Undefined, nameValue)
        {
            if (Enum.TryParse<ALSymbolKind>(kindTextValue, out ALSymbolKind kindValue))
                this.kind = kindValue;
        }

        public ALSymbol(ALSymbolKind kindValue, string nameValue, int? idValue) : this(kindValue, nameValue)
        {
            if (idValue.HasValue)
                this.id = idValue.Value;
        }

        public void AddChildSymbol(ALSymbol symbolInfo)
        {
            if (symbolInfo == null)
                return;
            if (this.childSymbols == null)
                this.childSymbols = new List<ALSymbol>();
            symbolInfo.ParentSymbol = this;
            this.childSymbols.Add(symbolInfo);
        }

        public void UpdateFields()
        {
            if (String.IsNullOrWhiteSpace(this.fullName))
                this.fullName = this.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.name);

            if (this.childSymbols != null)
            {
                for (int i=0; i<this.childSymbols.Count;i++)
                {
                    this.childSymbols[i].UpdateFields();
                }
            }
        }

        public ALSymbol CreateCopy(bool withChildSymbols)
        {
            ALSymbol symbol = new ALSymbol
            {
                id = this.id,
                name = this.name,
                subtype = this.subtype,
                fullName = this.fullName,
                kind = this.kind,
                range = this.range,
                selectionRange = this.selectionRange
            };

            if ((withChildSymbols) && (this.childSymbols != null))
            {
                for (int i=0; i<this.childSymbols.Count; i++)
                {
                    symbol.AddChildSymbol(this.childSymbols[i].CreateCopy(withChildSymbols));
                }
            }

            return symbol;
        }

        public ALSymbol GetObjectsTree()
        {
            ALSymbol symbol = this.CreateCopy(false);
            if ((!this.kind.IsObjectDefinition()) && (this.childSymbols != null))
            {
                for (int i = 0; i < this.childSymbols.Count; i++)
                {
                    symbol.AddChildSymbol(this.childSymbols[i].GetObjectsTree());
                }
            }
            return symbol;
        }

        public void RemoveEmptyChildNodes()
        {
            if (this.childSymbols != null)
            {
                for (int i = this.childSymbols.Count - 1; i >= 0; i--)
                {
                    this.childSymbols[i].RemoveEmptyChildNodes();
                    if (this.childSymbols[i].IsEmptyAndCanBeRemoved())
                        this.childSymbols.RemoveAt(i);
                }
                if (this.childSymbols.Count == 0)
                    this.childSymbols = null;
            }
        }

        public bool IsEmptyAndCanBeRemoved()
        {
            switch (kind)
            {
                case ALSymbolKind.ParameterList:
                case ALSymbolKind.PropertyList:
                    return (childSymbols == null) || (childSymbols.Count == 0);
            }
            return false;
        }

    }
}
