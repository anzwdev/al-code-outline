using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{
    public class CodeCompletionParameters
    {

        [JsonProperty("keepVariableNamesAffixes")]
        public bool KeepVariableNamesAffixes { get; set; }

    }
}
