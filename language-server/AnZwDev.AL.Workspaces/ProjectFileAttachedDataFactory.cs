using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces
{

    public abstract class ProjectFileAttachedDataFactory<T> where T : ProjectFileAttachedData
    {

        public string Key { get; }

        public ProjectFileAttachedDataFactory() : this(typeof(T).Name)
        {
        }

        public ProjectFileAttachedDataFactory(string key)
        {
            this.Key = key;
        }

        public abstract T Create(ProjectFile projectFile);

    }   

}
