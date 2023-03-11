using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetWarningDirectivesResponse
    {

        public List<WarningDirectiveInfo> directives { get; set; }

    }
}
