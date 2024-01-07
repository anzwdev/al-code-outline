using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using AnZwDev.ALTools.Workspace.SymbolReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseObjectInformationProvider<T> where T : ALAppObject
    {

        private readonly Func<ALAppSymbolReference, IEnumerable<T>> _objectsEnumerable;

        public BaseObjectInformationProvider(Func<ALAppSymbolReference, IEnumerable<T>> objectsEnumerable)
        {
            _objectsEnumerable = objectsEnumerable;
        }

        #region Objects list and search

        protected IEnumerable<T> GetALAppObjectsCollection(ALProject project, bool includeNonAccessible = false)
        {
            return project
                .GetAllSymbolReferences()
                .GetAllObjects<T>(_objectsEnumerable, includeNonAccessible);
        }

        #endregion

        #region Object methods

        protected virtual IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            return alObject?.Methods;
        }

        public IEnumerable<ALAppMethod> GetMethods(ALProject project, ALObjectReference objectReference)
        {
            IEnumerable<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetMethods(project, alObjectsCollection?.FindFirst(objectReference));
        }

        public List<MethodInformation> GetMethodsInformation(ALProject project, ALObjectReference objectReference)
        {
            List<MethodInformation> methodsInformation = new List<MethodInformation>();
            IEnumerable<ALAppMethod> alObjectMethods = this.GetMethods(project, objectReference);
            foreach (ALAppMethod method in alObjectMethods)
            {
                methodsInformation.Add(new MethodInformation(method));
            }
            return methodsInformation;
        }

        #endregion

        #region Object variables

        protected virtual IEnumerable<ALAppVariable> GetVariables(ALProject project, T alObject)
        {
            return alObject?.Variables;
        }

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, ALObjectReference objectReference)
        {
            IEnumerable<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetVariables(project, alObjectsCollection?.FindFirst(objectReference));
        }

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, int id)
        {
            IEnumerable<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetVariables(project, alObjectsCollection?.FindFirst(id));
        }

        #endregion

    }
}
