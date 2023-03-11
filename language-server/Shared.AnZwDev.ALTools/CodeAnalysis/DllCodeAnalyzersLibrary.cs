using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public class DllCodeAnalyzersLibrary : CodeAnalyzersLibrary
    {
        public ALDevToolsServer ALDevToolsServer { get; }
        public string FilePath { get; }

        public DllCodeAnalyzersLibrary(ALDevToolsServer newALDevToolsServer, string newName) : base(newName)
        {
            this.ALDevToolsServer = newALDevToolsServer;

            newName = newName.Trim();
            if (newName.StartsWith("${analyzerFolder}"))
            {
                newName = newName.Replace("${analyzerFolder}", "");
                this.FilePath = System.IO.Path.Combine(this.ALDevToolsServer.ExtensionBinPath, "Analyzers", newName);
                this.Name = System.IO.Path.GetFileNameWithoutExtension(this.FilePath);
            } 
            else if ((newName.StartsWith("${")) && (newName.EndsWith("}")))
            {
                newName = newName.Substring(2, newName.Length - 3);
                this.Name = newName;
                this.FilePath = System.IO.Path.Combine(this.ALDevToolsServer.ExtensionBinPath, "Analyzers", "Microsoft.Dynamics.Nav." + this.Name + ".dll");
            }
            else
            {
                this.FilePath = newName;
                this.Name = System.IO.Path.GetFileNameWithoutExtension(this.FilePath);
            }
        }

        public override void Load()
        {
            this.Rules.Clear();

            Assembly codeAnalyzerAssembly = Assembly.LoadFrom(this.FilePath);

            foreach (Module module in codeAnalyzerAssembly.Modules)
            {
                try
                {
                    Type[] types = module.GetTypes();
                    foreach (Type type in types)
                    {
                        if (IsAnalyzer(type))
                        {
                            DiagnosticAnalyzer analyzer = (DiagnosticAnalyzer)type.GetConstructor(Type.EmptyTypes).Invoke(null);
                            ImmutableArray<DiagnosticDescriptor> diagnostics = analyzer.SupportedDiagnostics;
                            if (diagnostics != null)
                            {
                                foreach (DiagnosticDescriptor diag in diagnostics)
                                {
                                    CodeAnalyzerRule rule = new CodeAnalyzerRule(diag);
                                    this.Rules.Add(rule);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        var typeLoadException = ex as ReflectionTypeLoadException;
                        var loaderExceptions = typeLoadException.LoaderExceptions;
                        if (loaderExceptions.Length > 0)
                            throw loaderExceptions[0];
                    }
                    throw;
                }
            }

        }

        protected bool IsAnalyzer(Type type)
        {
            IEnumerable<Attribute> attributes = type.GetCustomAttributes();
            foreach (Attribute attribute in attributes)
            {
                if (attribute.GetType().Name == "DiagnosticAnalyzerAttribute")
                    return true;
            }
            return false;
        }


    }
}
