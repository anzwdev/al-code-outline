using AnZwDev.ALTools.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class GetCodeAnalyzersRulesResponse
    {

        public string name { get; set; }
        public List<CodeAnalyzerRule> rules { get; set; }

    }
}
