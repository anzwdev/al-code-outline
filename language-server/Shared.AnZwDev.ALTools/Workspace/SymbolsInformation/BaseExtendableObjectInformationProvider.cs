using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseExtendableObjectInformationProvider<T,E> : BaseObjectInformationProvider<T> where T: ALAppObject where E: ALAppObject, IALAppObjectExtension
    {

        private readonly Func<ALProjectAllALAppSymbolsReference, ALProjectAllALAppObjectExtensionsCollection<E>> _objectExtensionsCollection;

        public BaseExtendableObjectInformationProvider(
            Func<ALProjectAllALAppSymbolsReference, ALProjectAllALAppObjectsCollection<T>> objectsCollection,
            Func<ALProjectAllALAppSymbolsReference, ALProjectAllALAppObjectExtensionsCollection<E>> objectExtensionsCollection) : base(objectsCollection)
        {
            _objectExtensionsCollection = objectExtensionsCollection;
        }

        #region Object list and search

        protected IEnumerable<E> GetALAppObjectExtensionsCollection(ALProject project, T baseObject)
        {
            var baseObjectIdentifier = baseObject.GetIdentifier();
            return _objectExtensionsCollection(project.SymbolsWithDependencies).GetObjectExtensions(baseObjectIdentifier);
        }

        protected IEnumerable<E> GetALAppObjectExtensionsCollection(ALProject project, ALObjectIdentifier baseObjectIdentifier)
        {
            return _objectExtensionsCollection(project.SymbolsWithDependencies).GetObjectExtensions(baseObjectIdentifier);
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
