using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeTreeViews
{
    public class SyntaxTreeTreeViewNode
    {


        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; } = null;

        [JsonPropertyName("kind")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Kind { get; set; } = null;

        [JsonPropertyName("span")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextRange? Span { get; set; } = null;

        [JsonPropertyName("fullSpan")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextRange? FullSpan { get; set; } = null;

        [JsonPropertyName("childNodes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<SyntaxTreeTreeViewNode>? ChildNodes { get; private set; } = null;

        [JsonPropertyName("attributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<SyntaxTreeTreeViewNode>? Attributes { get; private set; } = null;

        [JsonPropertyName("openBraceToken")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SyntaxTreeTreeViewNode? OpenBraceToken { get; set; } = null;

        [JsonPropertyName("closeBraceToken")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SyntaxTreeTreeViewNode? CloseBraceToken { get; set; } = null;

        [JsonPropertyName("varKeyword")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SyntaxTreeTreeViewNode? VarKeyword { get; set; } = null;

        [JsonPropertyName("accessModifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccessModifier { get; set; } = null;

        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identifier { get; set; } = null;

        [JsonPropertyName("dataType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DataType { get; set; } = null;

        [JsonPropertyName("temporary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Temporary { get; set; } = null;

        [JsonPropertyName("containsDiagnostics")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ContainsDiagnostics { get; set; } = null;

        public SyntaxTreeTreeViewNode()
        {
        }

        public SyntaxTreeTreeViewNode(Exception e) : this()
        {
            Name = e.Message;
            Kind = "error";
        }

        public void AddChildNode(SyntaxTreeTreeViewNode? node)
        {
            if (node != null)
            {
                if (ChildNodes == null)
                    ChildNodes = new List<SyntaxTreeTreeViewNode>();
                ChildNodes.Add(node);
            }
        }

        public void AddAttribute(SyntaxTreeTreeViewNode? node)
        {
            if (node != null)
            {
                if (Attributes == null)
                    Attributes = new List<SyntaxTreeTreeViewNode>();
                Attributes.Add(node);
            }
        }

    }
}
