using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces
{
    public abstract class ProjectFileFactory
    {

        public abstract ProjectFile Create(Project project, string fullPath);

    }
}
