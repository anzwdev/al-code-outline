using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeViewerTreeViews
{
    public class SyntaxTreeViewerTreeNode
    {


        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string? Uid { get; private set; } = null;

        [JsonProperty("idx")]
        public int Idx { get; private set; } = 0;

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name { get; set; }

        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string? FullName { get; set; }

        [JsonProperty("containsDiagnostics")]
        public bool ContainsDiagnostics { get; set; }

        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? Range { get; set; }

        [JsonProperty("selectionRange", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? SelectionRange { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string? Type { get; set; }

        [JsonProperty("childSymbols", NullValueHandling = NullValueHandling.Ignore)]
        public List<SyntaxTreeViewerTreeNode>? ChildSymbols { get; set; } = null;

        [JsonIgnore]
        public SyntaxTreeViewerTreeNode? ParentSymbol { get; set; } = null;

        [JsonIgnore]
        public SyntaxNode? SyntaxNode { get; set; }

        [JsonIgnore]
        public List<SyntaxTreeViewerTreeNodeProperty>? Properties { get; set; }

        public SyntaxTreeViewerTreeNode()
        {
        }

        public void AddChildSymbol(SyntaxTreeViewerTreeNode? child)
        {
            if (child != null)
            {
                if (ChildSymbols == null)
                    ChildSymbols = new List<SyntaxTreeViewerTreeNode>();
                ChildSymbols.Add(child);
                child.ParentSymbol = this;
            }
        }

        public SyntaxTreeViewerTreeNode? Find(string uid)
        {
            if (uid == Uid)
                return this;

            if (ChildSymbols != null)
                for (int i = 0; i < ChildSymbols.Count; i++)
                {
                    var found = ChildSymbols[i].Find(uid);
                    if (found != null)
                        return found;
                }

            return null;
        }

        public void CalculateUid(int childIndex = 0, SyntaxTreeViewerTreeNode? parent = null)
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
