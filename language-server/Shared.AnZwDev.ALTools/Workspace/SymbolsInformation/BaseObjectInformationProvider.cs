using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class BaseObjectInformationProvider<T> where T : ALAppObject
    {

        public BaseObjectInformationProvider()
        {
        }

        #region Objects list and search

        protected virtual MergedALAppObjectsCollection<T> GetALAppObjectsCollection(ALProject project)
        {
            return null;
        }

        #endregion

        #region Object methods

        protected virtual IEnumerable<ALAppMethod> GetMethods(ALProject project, T alObject)
        {
            return alObject?.Methods;
        }

        public IEnumerable<ALAppMethod> GetMethods(ALProject project, string name)
        {
            MergedALAppObjectsCollection<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetMethods(project, alObjectsCollection?.FindObject(name));
        }

        public IEnumerable<ALAppMethod> GetMethods(ALProject project, int id)
        {
            MergedALAppObjectsCollection<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetMethods(project, alObjectsCollection?.FindObject(id));
        }

        public List<MethodInformation> GetMethodsInformation(ALProject project, string name)
        {
            List<MethodInformation> methodsInformation = new List<MethodInformation>();
            IEnumerable<ALAppMethod> alObjectMethods = this.GetMethods(project, name);
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

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, string name)
        {
            MergedALAppObjectsCollection<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetVariables(project, alObjectsCollection?.FindObject(name));
        }

        public IEnumerable<ALAppVariable> GetVariables(ALProject project, int id)
        {
            MergedALAppObjectsCollection<T> alObjectsCollection = this.GetALAppObjectsCollection(project);
            return this.GetVariables(project, alObjectsCollection?.FindObject(id));
        }

        #endregion

    }
}
