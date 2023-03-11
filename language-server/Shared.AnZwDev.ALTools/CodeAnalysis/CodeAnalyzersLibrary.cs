using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.CodeAnalysis
{
    //default analyzers
    //"${AppSourceCop}","${CodeCop}","${PerTenantExtensionCop}","${UICop}"

    public class CodeAnalyzersLibrary
    {

        public string Name { get; protected set; }
        public List<CodeAnalyzerRule> Rules { get; }

        public CodeAnalyzersLibrary(string newName)
        {
            this.Name = newName;
            this.Rules = new List<CodeAnalyzerRule>();
        }

        public virtual void Load()
        {
        }

    }
}
