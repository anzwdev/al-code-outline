using AnZwDev.ALTools.ALSymbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionTextEdit
    {

        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange Range { get; set; }

        [JsonProperty("newText", NullValueHandling = NullValueHandling.Ignore)]
        public string NewText { get; set; }

        public CodeCompletionTextEdit()
        {
        }

        public CodeCompletionTextEdit(TextRange range, string newText)
        {
            Range = range;
            NewText = newText;
        }


    }
}
