using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionItemComparer : IComparer<CodeCompletionItem>
    {

        public CodeCompletionItemComparer()
        {
        }

        public int Compare(CodeCompletionItem x, CodeCompletionItem y)
        {
            return x.label.CompareTo(y.label);
        }
    }
}
