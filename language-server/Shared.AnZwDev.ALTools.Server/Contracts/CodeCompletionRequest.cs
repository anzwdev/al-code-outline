﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class CodeCompletionRequest
    {

        public Position position { get; set; }
        public string path { get; set; }
        public List<string> providers { get; set; }
        public CodeCompletionParameters parameters { get; set; }

    }
}
