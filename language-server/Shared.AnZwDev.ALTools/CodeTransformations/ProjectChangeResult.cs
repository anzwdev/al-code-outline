using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public struct ProjectChangeResult
    {

        public int NoOfChangedFiles { get; set; }
        public int NoOfChanges { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public ProjectChangeResult(Exception exception) : this(0, 0, false, exception.Message)
        {
        }

        public ProjectChangeResult(int noOfChangedFiles, int noOfChanges, bool success = true, string errorMessage = null)
        {
            NoOfChangedFiles = noOfChangedFiles;
            NoOfChanges = noOfChanges;
            Success = success;
            ErrorMessage = errorMessage;
        }

    }
}
