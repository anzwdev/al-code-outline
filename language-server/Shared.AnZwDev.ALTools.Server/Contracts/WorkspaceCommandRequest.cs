﻿using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class WorkspaceCommandRequest
    {

        public string command { get; set; }
        public string source { get; set; }
        public string projectPath { get; set; }
        public string filePath { get; set; }
        public TextRange range { get; set; }
        public Dictionary<string, string> parameters { get; set; }
        public List<string> excludeFiles { get; set; }
        public List<string> includeFiles { get; set; }

    }
}
