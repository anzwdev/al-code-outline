using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SymbolsTreeViews
{
    public class SymbolsTreeNode
    {

        [JsonProperty("uid")]
        public int Uid { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("namespaceName", NullValueHandling = NullValueHandling.Ignore)]
        public string? NamespaceName { get; set; }

        [JsonProperty("usings", NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<string>? Usings { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = String.Empty;

        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string? FullName { get; set; }

        [JsonProperty("kind")]
        public ALSyntaxNodeKind Kind { get; set; }

        [JsonProperty("subtype", NullValueHandling = NullValueHandling.Ignore)]
        public string? Subtype { get; set; }

        [JsonProperty("access")]
        public ALSyntaxNodeAccessModifier Access { get; set; } = ALSyntaxNodeAccessModifier.Public;

        [JsonProperty("extends", NullValueHandling = NullValueHandling.Ignore)]
        public string? Extends { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string? Source { get; set; }

        [JsonProperty("childSymbols", NullValueHandling = NullValueHandling.Ignore)]
        public List<SymbolsTreeNode>? ChildSymbols { get; set; }

        [JsonIgnore]
        public SymbolsTreeNode? ParentSymbol { get; set; }

        [JsonIgnore]
        public required Symbol? TreeNodeSource { get; set; }

        public void AddChildSymbol(SymbolsTreeNode? child)
        {
            if (child != null)
            {
                if (ChildSymbols == null)
                    ChildSymbols = new List<SymbolsTreeNode>();
                ChildSymbols.Add(child);
                child.ParentSymbol = this;
            }
        }

        public void UpdateUid()
        {
            int lastUid = 0;
            UpdateUid(ref lastUid);
        }

        internal void UpdateUid(ref int lastUid)
        {
            lastUid++;
            this.Uid = lastUid;

            if (ChildSymbols != null)
                for (int i = 0; i < ChildSymbols.Count; i++)
                    ChildSymbols[i].UpdateUid(ref lastUid);
        }

    }
}
