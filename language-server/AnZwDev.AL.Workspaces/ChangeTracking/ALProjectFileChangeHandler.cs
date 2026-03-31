using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.ChangeTracking
{
    internal class ALProjectFileChangeHandler : ProjectFileChangeHandler
    {
        public ALProjectFileChangeHandler() : base(ProjectFileType.AL)
        {
        }

        public override void FileAdded(ProjectFile file)
        {
            base.FileAdded(file);

            file.Project.SymbolsProvider.ProjectCodeSymbolsProvider.FileAdded(file);
        }

        override public void FileChanged(ProjectFile file)
        {
            base.FileChanged(file);

            file.Project.SymbolsProvider.ProjectCodeSymbolsProvider.FileChanged(file);
        }

        override public void FileRemoved(ProjectFile file)
        {
            base.FileRemoved(file);

            file.Project.SymbolsProvider.ProjectCodeSymbolsProvider.FileRemoved(file);
        }

    }
}
