using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.ChangeTracking
{
    internal class AppJsonProjectFileChangeHandler : ProjectFileChangeHandler
    {

        public AppJsonProjectFileChangeHandler() : base(ProjectFileType.AppJson)
        {
        }

        public override void FileAdded(ProjectFile file)
        {
            base.FileAdded(file);

            file.Project.ReloadAppJson();
        }

        override public void FileChanged(ProjectFile file)
        {
            base.FileChanged(file);

            file.Project.ReloadAppJson();
        }

        override public void FileRemoved(ProjectFile file)
        {
            base.FileRemoved(file);

            file.Project.ReloadAppJson();
        }

    }
}
