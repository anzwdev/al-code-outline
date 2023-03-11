using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseExtendableObjectInformationProvider<T,E> : BaseObjectInformationProvider<T> where T: ALAppObject where E: ALAppObject, IALAppObjectExtension
    {
        public BaseExtendableObjectInformationProvider()
        {
        }

        #region Object list and search

        protected virtual MergedALAppObjectExtensionsCollection<E> GetALAppObjectExtensionsCollection(ALProject project)
        {
            return null;
        }

        #endregion

        #region Methods

        protected override IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            //main methods
            if ((alObject != null) && (alObject.Methods != null))
            {
                foreach (ALAppMethod alAppMethod in alObject.Methods) 
                { 
                    yield return alAppMethod; 
                }
            }

            //extension methods
            IEnumerable<E> extensions = this.GetALAppObjectExtensionsCollection(project)?.FindAllExtensions(alObject.Name);
            if (extensions != null)
            {
                foreach (E ext in extensions)
                {
                    if ((ext != null) && (ext.Methods != null))
                    {
                        foreach (ALAppMethod alAppMethod in ext.Methods) 
                        { 
                            yield return alAppMethod; 
                        }
                    }
                }
            }
        }

        #endregion

        #region Variables

        protected override IEnumerable<ALAppVariable> GetVariables(ALProject project, T alObject)
        {
            //main methods
            if ((alObject != null) && (alObject.Variables != null))
            {
                foreach (ALAppVariable alVariable in alObject.Variables)
                {
                    yield return alVariable;
                }
            }

            //extension methods
            IEnumerable<E> extensions = this.GetALAppObjectExtensionsCollection(project)?.FindAllExtensions(alObject.Name);
            if (extensions != null)
            {
                foreach (E ext in extensions)
                {
                    if ((ext != null) && (ext.Variables != null))
                    {
                        foreach (ALAppVariable alVariable in ext.Variables)
                        {
                            yield return alVariable;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
