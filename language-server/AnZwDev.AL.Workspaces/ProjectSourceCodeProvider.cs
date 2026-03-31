using AnZwDev.AL.Syntax;
using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Workspaces
{
    public class ProjectSourceCodeProvider : ISourceCodeProvider
    {

        public Project Project { get; }
        public IFile? AppJsonFile => Project.Files.AppJson;
        public IEnumerable<IFile> SourceFiles => Project.Files.FilterByType(ProjectFileType.AL);

        public ProjectSourceCodeProvider(Project project)
        {
            this.Project = project;
        }

    }
}
