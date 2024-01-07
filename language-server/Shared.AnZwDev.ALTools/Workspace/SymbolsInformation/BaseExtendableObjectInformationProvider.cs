using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseExtendableObjectInformationProvider<T,E> : BaseObjectInformationProvider<T> where T: ALAppObject where E: ALAppObject, IALAppObjectExtension
    {

        private readonly Func<ALAppSymbolReference, IEnumerable<E>> _objectExtensionsEnumerable;

        public BaseExtendableObjectInformationProvider(Func<ALAppSymbolReference, IEnumerable<T>> objectsEnumberable, Func<ALAppSymbolReference, IEnumerable<E>> objectExtensionsEnumerable) : base(objectsEnumberable)
        {
            _objectExtensionsEnumerable = objectExtensionsEnumerable;
        }

        #region Object list and search

        protected IEnumerable<E> GetALAppObjectExtensionsCollection(ALProject project, T baseObject)
        {
            return project
                .GetAllSymbolReferences()
                .GetObjectExtensions<E>(_objectExtensionsEnumerable, baseObject.GetIdentifier());
        }

        #endregion

        #region Methods

        protected override IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            if (alObject != null)
            {
                //main methods
                if (alObject.Methods != null)
                    foreach (ALAppMethod alAppMethod in alObject.Methods)
                        yield return alAppMethod;

                //extension methods
                IEnumerable<E> extensions = this.GetALAppObjectExtensionsCollection(project, alObject);
                if (extensions != null)
                    foreach (E ext in extensions)
                        if (ext.Methods != null)
                            foreach (ALAppMethod alAppMethod in ext.Methods)
                                yield return alAppMethod;
            }
        }

        #endregion

        #region Variables

        protected override IEnumerable<ALAppVariable> GetVariables(ALProject project, T alObject)
        {
            if (alObject != null)
            {
                //main methods
                if (alObject.Variables != null)
                    foreach (ALAppVariable alVariable in alObject.Variables)
                        yield return alVariable;

                //extension methods
                IEnumerable<E> extensions = this.GetALAppObjectExtensionsCollection(project, alObject);
                if (extensions != null)
                    foreach (E ext in extensions)
                        if ((ext != null) && (ext.Variables != null))
                            foreach (ALAppVariable alVariable in ext.Variables)
                                yield return alVariable;
            }
        }

        #endregion

    }
}
