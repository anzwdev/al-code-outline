using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectSymbolsLibrarySource : ALAppBaseSymbolsLibrarySource
    {

        public ALProject Project { get; }

        public ALProjectSymbolsLibrarySource(ALProject project)
        {
            this.Project = project;
        }

        public override ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol)
        {
            return this.GetSymbolSourceLocation(symbol, false);
        }

        public ALSymbolSourceLocation GetSymbolSourceProjectLocation(ALSymbol symbol)
        {
            return this.GetSymbolSourceLocation(symbol, true);
        }

        protected ALSymbolSourceLocation GetSymbolSourceLocation(ALSymbol symbol, bool projectSource)
        {
            ALSymbolSourceLocation location = new ALSymbolSourceLocation(symbol);

            ALAppObject alAppObject;
            if (this.Project.Symbols != null)
            {
                alAppObject = this.Project.Symbols.FindObjectByName(symbol.kind, symbol.name, false);
                if (alAppObject != null)
                {
                    this.SetSource(location, this.Project.Symbols, alAppObject, projectSource);
                    return location;
                }
            }

            if (this.Project.Dependencies != null)
            {
                foreach (ALProjectDependency dependency in this.Project.Dependencies)
                {
                    if (dependency.Symbols != null)
                    {
                        alAppObject = dependency.Symbols.FindObjectByName(symbol.kind, symbol.name, false);
                        if (alAppObject != null)
                        {
                            this.SetSource(location, dependency.Symbols, alAppObject, projectSource);
                            return location;
                        }
                    }
                }
            }

            return location;
        }

        protected void SetSource(ALSymbolSourceLocation location, ALAppSymbolReference symbolReference, ALAppObject alAppObject, bool projectSource)
        {
            this.SetSource(location, symbolReference, alAppObject);
            if ((projectSource) && (location.schema == ALSymbolSourceLocationSchema.ALApp) && (alAppObject.GetALSymbolKind().ServerDefinitionAvailable()))
            {
                //for old version of Business Central and NAV 2018 objects can be modified and defined in C/AL, so we have to download source from the server
#if BC
                bool serverSideSymbols = (
                    (this.Project.Properties.Runtime == null) ||
                    (this.Project.Properties.Runtime.Parts == null) ||
                    (this.Project.Properties.Runtime.Parts.Length == 0) ||
                    (this.Project.Properties.Runtime.Parts[0] <= 4));
#else
                bool serverSideSymbols = true;
#endif

                if (serverSideSymbols)
                {
                    location.schema = ALSymbolSourceLocationSchema.Server;
                    location.sourcePath = alAppObject.Name;
                }
                else
                {
                    location.schema = ALSymbolSourceLocationSchema.ALPreview;
                    location.sourcePath = alAppObject.GetALSymbolKind().ToObjectTypeName() + "/" + alAppObject.Id.ToString() + "/" + System.Net.WebUtility.UrlEncode(alAppObject.Name) + ".dal";
                }
            }
        }

    }
}
