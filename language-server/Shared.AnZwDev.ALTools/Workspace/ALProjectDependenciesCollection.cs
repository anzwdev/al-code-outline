using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectDependenciesCollection: List<ALProjectDependency>
    {

        #region Public properties

        private ALProjectDependency _platform;
        public ALProjectDependency Platform 
        { 
            get { return _platform; }
            set { SetPropertyValue(ref _platform, value); }
        }

        private ALProjectDependency _application;
        public ALProjectDependency Application
        {
            get { return _application; }
            set { SetPropertyValue(ref _application, value); }
        }

        private ALProjectDependency _test;
        public ALProjectDependency Test
        {
            get { return _test; }
            set { SetPropertyValue(ref _test, value); }
        }

        #endregion

        public ALProjectDependenciesCollection()
        {
            _platform = null;
            _application = null;
            _test = null;
        }

        protected void SetPropertyValue(ref ALProjectDependency propertyValue, ALProjectDependency newValue)
        {
            if (propertyValue != newValue)
            {
                if (propertyValue != null)
                    this.Remove(propertyValue);
                propertyValue = newValue;
                if (propertyValue != null)
                    this.Add(propertyValue);
            }
        }

        public new void Clear()
        {
            _platform = null;
            _application = null;
            _test = null;
            base.Clear();
        }

        public void RemovePropagatedDependencies()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (this[i].Propagated)
                    this.RemoveAt(i);
            }
        }

        public void AddPropagatedDependency(string id, string name, string publisher, string version)
        {
            ALProjectDependency dependency = this.Find(id, name, publisher);
            if (dependency == null)
            {
                dependency = new ALProjectDependency(id, name, publisher, version);
                dependency.Propagated = true;
                this.Add(dependency);
            }
        }

        public ALProjectDependency Find(string id, string name, string publisher)
        {
            for (int i=0; i<this.Count; i++)
            {
                ALProjectDependency dependency = this[i];

                bool emptyId = (String.IsNullOrWhiteSpace(id)) || (String.IsNullOrWhiteSpace(dependency.Id));

                if ((!emptyId) && (dependency.Id == id))
                    return dependency;

                if ((dependency.Name == name) && (dependency.Publisher == publisher) && (emptyId))
                    return dependency;
            }

            return null;
        }

        public ALProjectDependency FindAppPackageDependency(string path)
        {
            for (int i = 0; i < this.Count; i++)
            {
                ALProjectDependency dependency = this[i];
                if ((dependency.SourceAppFile != null) && (dependency.SourceAppFile.FullPath == path))
                    return dependency;
            }
            return null;
        }

        #region Replace id references with names

        public bool IdReferencesReplaced()
        {
            for (int i = 0; i < this.Count; i++)
                if (!this[i].IdReferencesReplaced())
                    return false;
            return true;
        }

        public ALAppObjectIdMap CreateALAppObjectIdMap()
        {
            ALAppObjectIdMap idMap = new ALAppObjectIdMap();
            for (int i=0; i<this.Count; i++)
            {
                if (this[i].Symbols != null)
                    this[i].Symbols.AddToObjectsIdMap(idMap);
            }
            return idMap;
        }

        public void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].IdReferencesReplaced())
                    this[i].Symbols.ReplaceIdReferences(idMap);
            }
        }

        #endregion

    }
}
