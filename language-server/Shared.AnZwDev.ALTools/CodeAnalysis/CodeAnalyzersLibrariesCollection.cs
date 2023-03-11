using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public class CodeAnalyzersLibrariesCollection
    {

        public ALDevToolsServer ALDevToolsServer { get; }
        protected Dictionary<string, CodeAnalyzersLibrary> LibrariesCache { get; }

        public CodeAnalyzersLibrariesCollection(ALDevToolsServer newALDevToolsServer)
        {
            this.ALDevToolsServer = newALDevToolsServer;
            this.LibrariesCache = new Dictionary<string, CodeAnalyzersLibrary>();
        }

        public CodeAnalyzersLibrary GetCodeAnalyzersLibrary(string name)
        {
            name = ValidateAnalyzerName(name);

            if (String.IsNullOrWhiteSpace(name))
                return null;

            if (this.LibrariesCache.ContainsKey(name))
                return this.LibrariesCache[name];

            try
            {
                CodeAnalyzersLibrary library = this.CreateCodeAnalyzersLibrary(name);
                library.Load();
                this.LibrariesCache.Add(name, library);
                return library;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string ValidateAnalyzerName(string name)
        {
            const string analyzersFolder = "${analyzerFolder}";
            if (!String.IsNullOrWhiteSpace(name))
                if (name.StartsWith(analyzersFolder))
                    name = Path.Combine(ALDevToolsServer.ExtensionBinPath, "Analyzers", name.Substring(analyzersFolder.Length));
            return name;
        }

        protected CodeAnalyzersLibrary CreateCodeAnalyzersLibrary(string name)
        {
            if ((name != null) && (name.Equals("Compiler", StringComparison.CurrentCultureIgnoreCase)))
                return new CompilerCodeAnalyzersLibrary(name);
            return new DllCodeAnalyzersLibrary(this.ALDevToolsServer, name);
        }

        public void LoadCodeAnalyzers(ALWorkspace workspace)
        {
            GetCodeAnalyzersLibrary("${AppSourceCop}");
            GetCodeAnalyzersLibrary("${CodeCop}");
            GetCodeAnalyzersLibrary("${PerTenantExtensionCop}");
            GetCodeAnalyzersLibrary("${UICop}");
            GetCodeAnalyzersLibrary("${analyzerFolder}BusinessCentral.LinterCop.dll");
            GetCodeAnalyzersLibrary("Compiler");

            for (int projectIdx=0; projectIdx < workspace.Projects.Count; projectIdx++)
            {
                if (workspace.Projects[projectIdx].CodeAnalyzers != null)
                    for (int analyzerIdx = 0; analyzerIdx < workspace.Projects[projectIdx].CodeAnalyzers.Count; analyzerIdx++)
                        GetCodeAnalyzersLibrary(workspace.Projects[projectIdx].CodeAnalyzers[analyzerIdx]);
            }
        }

        public CodeAnalyzerRule FindCachedRule(string ruleId)
        {
            foreach (var library in this.LibrariesCache.Values)
            {
                var rule = library.Rules
                    .Where(p => (ruleId.Equals(p.id, StringComparison.CurrentCultureIgnoreCase)))
                    .FirstOrDefault();
                if (rule != null)
                    return rule;
            }
            return null;
        }

    }
}
