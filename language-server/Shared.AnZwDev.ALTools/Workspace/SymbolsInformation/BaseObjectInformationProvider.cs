using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
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

        private readonly Func<ALProjectAllALAppSymbolsReference, ALProjectAllALAppObjectsCollection<T>> _objectsCollection;

        public BaseObjectInformationProvider(Func<ALProjectAllALAppSymbolsReference, ALProjectAllALAppObjectsCollection<T>> objectsCollection)
        {
            _objectsCollection = objectsCollection;
        }

        #region Objects list and search

        protected ALProjectAllALAppObjectsCollection<T> GetALAppObjectsCollection(ALProject project)
        {
            return _objectsCollection(project.SymbolsWithDependencies);
        }

        #endregion

        #region Object methods

        protected virtual IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            return alObject?.Methods;
        }

        public IEnumerable<ALAppMethod> GetMethods(ALProject project, ALObjectReference objectReference)
        {
            var alObjectsCollection = this.GetALAppObjectsCollection(project);
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
            var alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetVariables(project, alObjectsCollection?.FindFirst(objectReference));
        }

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, int id)
        {
            var alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetVariables(project, alObjectsCollection?.FindFirst(id));
        }

        #endregion

    }
}
