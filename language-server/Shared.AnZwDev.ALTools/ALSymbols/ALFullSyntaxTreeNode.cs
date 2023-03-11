using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{

    public class ALFullSyntaxTreeNode
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string kind { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range span { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Range fullSpan { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ALFullSyntaxTreeNode> childNodes { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ALFullSyntaxTreeNode> attributes { get; private set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ALFullSyntaxTreeNode openBraceToken { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ALFullSyntaxTreeNode closeBraceToken { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ALFullSyntaxTreeNode varKeyword { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string accessModifier { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string identifier { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dataType { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string temporary { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? containsDiagnostics { get; set; }

        public ALFullSyntaxTreeNode()
        {
            this.name = null;
            this.kind = null;
            this.span = null;
            this.fullSpan = null;
            this.childNodes = null;
            this.attributes = null;
            this.openBraceToken = null;
            this.closeBraceToken = null;
            this.varKeyword = null;
            this.accessModifier = null;
            this.identifier = null;
            this.dataType = null;
            this.temporary = null;
            this.containsDiagnostics = null;
        }

        public ALFullSyntaxTreeNode(Exception e): this()
        {
            this.name = e.Message;
            this.kind = "error";
        }

        public void AddChildNode(ALFullSyntaxTreeNode node)
        {
            if (node != null)
            {
                if (this.childNodes == null)
                    this.childNodes = new List<ALFullSyntaxTreeNode>();
                this.childNodes.Add(node);
            }
        }

        public void AddAttribute(ALFullSyntaxTreeNode node)
        {
            if (node != null)
            {
                if (this.attributes == null)
                    this.attributes = new List<ALFullSyntaxTreeNode>();
                this.attributes.Add(node);
            }
        }

    }
}
