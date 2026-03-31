using System.Text.Json.Serialization;
using System.Xml;
using AnZwDev.AL.Syntax;

namespace AnZwDev.AL.TreeViewModel
{

    public sealed class SymbolHierarchyNode : SymbolHierarchyNode<SymbolHierarchyNode>
    {
    }

    public abstract class SymbolHierarchyNode<T> where T : SymbolHierarchyNode<T>, new()
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("namespaceName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NamespaceName { get; set; }

        [JsonPropertyName("usings")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HashSet<string>? Usings { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; } = String.Empty;

        [JsonPropertyName("access")]
        public SymbolHierarchyNodeAccessModifier Access { get; set; } = SymbolHierarchyNodeAccessModifier.Public;

        [JsonPropertyName("subtype")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Subtype { get; set; }

        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Format { get; set; }

        [JsonPropertyName("elementsubtype")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ElementSubtype { get; set; }

        [JsonPropertyName("fullName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FullName { get; set; }

        [JsonPropertyName("kind")]
        public SymbolHierarchyNodeKind Kind { get; set; }

        [JsonPropertyName("source")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Source { get; set; }

        [JsonPropertyName("extends")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Extends { get; set; }

        [JsonPropertyName("childSymbols")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<T>? ChildSymbols { get; set; }

        [JsonPropertyName("range")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextRange? Range { get; set; }

        [JsonPropertyName("selectionRange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextRange? SelectionRange { get; set; }

        [JsonPropertyName("contentRange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextRange? ContentRange { get; set; }

        [JsonPropertyName("tokensRange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextRange? TokensRange { get; set; }

        [JsonPropertyName("containsDiagnostics")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ContainsDiagnostics { get; set; }

        [JsonIgnore]
        public T? ParentSymbol { get; set; }

        public void AddChildSymbol(T? child)
        {
            if (child != null)
            {
                if (ChildSymbols == null)
                    ChildSymbols = new List<T>();
                ChildSymbols.Add(child);
                child.ParentSymbol = (T)this;
            }
        }

        public T Clone(bool includeChildSymbols)
        {
            var item = CloneItemOnly();
            if (includeChildSymbols)
                CloneChildSymbols(item);
            return item;
        }

        protected virtual T CloneItemOnly()
        {
            return new T()
            {
                Id = this.Id,
                NamespaceName = this.NamespaceName,
                Usings = (this.Usings != null) ? new HashSet<string>(this.Usings) : null,
                Name = this.Name,
                Access = this.Access,
                Subtype = this.Subtype,
                Format = this.Format,
                ElementSubtype = this.ElementSubtype,
                FullName = this.FullName,
                Kind = this.Kind,
                Source = this.Source,
                Extends = this.Extends,
                Range = (this.Range != null) ? this.Range.Clone() : null,
                SelectionRange = (this.SelectionRange != null) ? this.SelectionRange.Clone() : null,
                ContentRange = (this.ContentRange != null) ? this.ContentRange.Clone() : null,
                TokensRange = (this.TokensRange != null) ? this.TokensRange.Clone() : null,
                ContainsDiagnostics = this.ContainsDiagnostics
            };
        }

        private void CloneChildSymbols(T targetItem)
        {
            if (ChildSymbols != null)
            {
                targetItem.ChildSymbols = new List<T>();
                for (int i = 0; i < ChildSymbols.Count; i++)
                {
                    var child = ChildSymbols[i].Clone(true);
                    child.ParentSymbol = targetItem;
                    targetItem.ChildSymbols.Add(child); 
                }
            }
        }

        public T? FindParent(SymbolHierarchyNodeKind kind)
        {
            return this.ParentSymbol?.FindThisOrParent(kind);
        }

        public T? FindThisOrParent(SymbolHierarchyNodeKind kind)
        {
            var current = this;

            while ((current != null) && (current.Kind != kind))
                current = current.ParentSymbol;

            if (current == null)
                return null;

            return (T)current;
        }

    }
}
