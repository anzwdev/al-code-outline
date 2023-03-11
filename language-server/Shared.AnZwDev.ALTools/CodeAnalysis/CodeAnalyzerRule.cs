using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public class CodeAnalyzerRule
    {

        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string defaultSeverity { get; set; }
        public bool isEnabledByDefault { get; set; }

        public CodeAnalyzerRule()
        {
        }

        internal CodeAnalyzerRule(DiagnosticDescriptor diag)
        {
            this.id = diag.Id.ToString();
            this.title = diag.Title.ToString();
            this.description = diag.Description.ToString();
            this.category = diag.Category.ToString();
            this.defaultSeverity = diag.DefaultSeverity.ToString();
            this.isEnabledByDefault = diag.IsEnabledByDefault;
            if ((String.IsNullOrWhiteSpace(this.description)) && (diag.MessageFormat != null))
                this.description = diag.MessageFormat.ToString();
            if ((String.IsNullOrWhiteSpace(this.title)) && (diag.MessageFormat != null))
                this.title = diag.MessageFormat.ToString();
        }

    }
}
