using AnZwDev.ALTools.DuplicateCodeSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class FindDuplicateCodeResponse
    {
        public List<DCDuplicate> duplicates { get; set; }
        bool isError { get; set; }
        public string message { get; set; }

        public FindDuplicateCodeResponse()
        {
            isError = false;
            message = null;
        }

        public void SetError(Exception e)
        {
            isError = true;
            message = e.Message;
        }

    }
}
