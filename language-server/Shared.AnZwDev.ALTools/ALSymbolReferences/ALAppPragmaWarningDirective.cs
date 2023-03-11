using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPragmaWarningDirective : ALAppDirective
    {

        public bool Disabled { get; set; }
        public List<string> Rules { get; set; }

        public ALAppPragmaWarningDirective(Range range, bool disabled, List<string> rules) : base(range)
        {
            Disabled = disabled;
            Rules = rules;
        }

        public bool ContainsRule(string ruleId)
        {
            return
                (Rules == null) ||
                (Rules.Count == 0) ||
                (Rules.Contains(ruleId));
        }

    }
}
