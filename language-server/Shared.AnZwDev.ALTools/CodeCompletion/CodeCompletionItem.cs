using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionItem
    {

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string label { get; set; }

        [JsonProperty("kind")]
        public CompletionItemKind kind { get; set; }

        [JsonProperty("filterText", NullValueHandling = NullValueHandling.Ignore)]
        public string filterText { get; set; }

        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public string detail { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<CodeCompletionItemTag> tags { get; set; }

        [JsonProperty("commitCharacters", NullValueHandling = NullValueHandling.Ignore)]
        public string[] commitCharacters { get; set; }

        [JsonProperty("additionalTextEdits", NullValueHandling = NullValueHandling.Ignore)]
        public List<CodeCompletionTextEdit> additionalTextEdits { get; set; }

        public CodeCompletionItem()
        { 
        }

        public CodeCompletionItem(string label, CompletionItemKind kind)
        {
            this.label = label;
            this.kind = kind;
        }

        public void AddEdit(CodeCompletionTextEdit edit)
        {
            if (additionalTextEdits == null)
                additionalTextEdits = new List<CodeCompletionTextEdit>();
            additionalTextEdits.Add(edit);
        }

    }
}
