using AnZwDev.ALTools.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class CodeCompletionResponse
    {

        public List<CodeCompletionItem> completionItems { get; set; }

    }
}
