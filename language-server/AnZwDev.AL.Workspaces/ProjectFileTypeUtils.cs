using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.System.IO;

namespace AnZwDev.AL.Workspaces
{
    public static class ProjectFileTypeUtils
    {

        public static ProjectFileType GetFileType(string projectPath, string path)
        {
            if (path.EndsWith(".al", StringComparison.OrdinalIgnoreCase))
                return ProjectFileType.AL;

            var fileName = Path.GetFileName(path);
            if (fileName != null)
            {
                if (PathUtils.Equals(path, Path.Join(projectPath, WorkspacesConst.ProjectDefinitionFileName)))
                    return ProjectFileType.AppJson;

                if (PathUtils.Equals(path, Path.Join(projectPath,  WorkspacesConst.AppSourceCopFileName)))
                    return ProjectFileType.AppSourceCopJson;
            }
            return ProjectFileType.Unknown;
        }

    }
}
