using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.ChangeTracking
{
    internal class AppSourceCopJsonProjectFileChangeHandler : ProjectFileChangeHandler
    {
        public AppSourceCopJsonProjectFileChangeHandler() : base(ProjectFileType.AppSourceCopJson)
        {
        }

        public override void FileAdded(ProjectFile file)
        {
            base.FileAdded(file);

            file.Project.LoadAppSourceCopJson();
        }

        override public void FileChanged(ProjectFile file)
        {
            base.FileChanged(file);

            file.Project.LoadAppSourceCopJson();
        }

        override public void FileRemoved(ProjectFile file)
        {
            base.FileRemoved(file);

            file.Project.LoadAppSourceCopJson();
        }

    }
}
