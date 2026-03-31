using AnZwDev.AL.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews
{
    public class SyntaxTreeSymbolsTreeViewNode
    {

        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string? Uid { get; private set; } = null;

        [JsonProperty("idx")]
        public int Idx { get; private set; } = 0;

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

        [JsonProperty("access")]
        public ALSyntaxNodeAccessModifier Access { get; set; } = ALSyntaxNodeAccessModifier.Public;

        [JsonProperty("subtype", NullValueHandling = NullValueHandling.Ignore)]
        public string? Subtype { get; set; }

        [JsonProperty("elementsubtype", NullValueHandling = NullValueHandling.Ignore)]
        public string? ElementSubtype { get; set; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string? Format { get; set; }

        [JsonProperty("extends", NullValueHandling = NullValueHandling.Ignore)]
        public string? Extends { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string? Source { get; set; }

        [JsonProperty("childSymbols", NullValueHandling = NullValueHandling.Ignore)]
        public List<SyntaxTreeSymbolsTreeViewNode>? ChildSymbols { get; set; }

        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? Range { get; set; }

        [JsonProperty("selectionRange", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? SelectionRange { get; set; }

        [JsonProperty("contentRange", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? ContentRange { get; set; }

        [JsonProperty("tokensRange", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? TokensRange { get; set; }

        [JsonProperty("containsDiagnostics", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ContainsDiagnostics { get; set; }

        [JsonIgnore]
        public SyntaxTreeSymbolsTreeViewNode? ParentSymbol { get; set; }

        public void AddChildSymbol(SyntaxTreeSymbolsTreeViewNode? child)
        {
            if (child != null)
            {
                if (ChildSymbols == null)
                    ChildSymbols = new List<SyntaxTreeSymbolsTreeViewNode>();
                ChildSymbols.Add(child);
                child.ParentSymbol = this;
            }
        }

        public SyntaxTreeSymbolsTreeViewNode? FindParent(ALSyntaxNodeKind kind)
        {
            return this.ParentSymbol?.FindThisOrParent(kind);
        }

        public SyntaxTreeSymbolsTreeViewNode? FindThisOrParent(ALSyntaxNodeKind kind)
        {
            var current = this;

            while ((current != null) && (current.Kind != kind))
                current = current.ParentSymbol;

            return current;
        }

        public void CalculateUid(int childIndex = 0, SyntaxTreeSymbolsTreeViewNode? parent = null)
        {
            string calculatedUid = childIndex.ToString();
            if (!String.IsNullOrEmpty(parent?.Uid))
                calculatedUid = parent.Uid + "." + calculatedUid;
            
            this.Uid = calculatedUid;
            this.Idx = childIndex;

            if (ChildSymbols != null)
                for (var i = 0; i < ChildSymbols.Count; i++)
                    ChildSymbols[i].CalculateUid(i, this);
        }

    }
}
