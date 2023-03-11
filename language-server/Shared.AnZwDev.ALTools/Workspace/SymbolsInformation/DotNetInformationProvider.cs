using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class DotNetInformationProvider
    {

        public DotNetInformationProvider()
        {
        }

        public DotNetTypeInformation GetDotNetTypeInformation(ALProject project, string aliasName)
        {
            if (aliasName != null)
            {
                DotNetTypeInformation typeInformation = this.GetDotNetTypeInformation(project.Symbols?.DotNetPackages, aliasName);
                if (typeInformation != null)
                    return typeInformation;

                if (project.Dependencies != null)
                {
                    for (int i=0; i<project.Dependencies.Count; i++)
                    {
                        typeInformation = this.GetDotNetTypeInformation(project.Dependencies[i].Symbols?.DotNetPackages, aliasName);
                        if (typeInformation != null)
                            return typeInformation;
                    }
                }
            }
            return null;        
        }

        protected DotNetTypeInformation GetDotNetTypeInformation(ALAppObjectsCollection<ALAppDotNetPackage> dotNetPackages, string aliasName)
        {
            if (dotNetPackages != null)
            {
                for (int packageIdx = 0; packageIdx < dotNetPackages.Count; packageIdx++)
                {
                    ALAppDotNetPackage package = dotNetPackages[packageIdx];
                    if (package.AssemblyDeclarations != null)
                    {
                        for (int assemblyIdx = 0; assemblyIdx < package.AssemblyDeclarations.Count; assemblyIdx++)
                        {
                            ALAppDotNetAssemblyDeclaration assembly = package.AssemblyDeclarations[assemblyIdx];
                            if (assembly.TypeDeclarations != null)
                            {
                                for (int typeIdx = 0; typeIdx < assembly.TypeDeclarations.Count; typeIdx++)
                                {
                                    ALAppDotNetTypeDeclaration type = assembly.TypeDeclarations[typeIdx];
                                    if ((type.AliasName != null) && (aliasName.Equals(type.AliasName, StringComparison.CurrentCultureIgnoreCase)))
                                    {
                                        return new DotNetTypeInformation(type.TypeName, type.AliasName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }


    }
}
