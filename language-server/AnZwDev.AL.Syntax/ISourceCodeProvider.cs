using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.System.IO;

namespace AnZwDev.AL.Syntax
{
    public interface ISourceCodeProvider
    {

        IFile? AppJsonFile { get; }
        IEnumerable<IFile> SourceFiles { get; }

    }
}
