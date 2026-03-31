using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces
{
    public class ProjectChangeTracker
    {

        public Project Project { get; }

        public ProjectChangeTracker(Project project)
        {
            this.Project = project;
        }

    }
}
