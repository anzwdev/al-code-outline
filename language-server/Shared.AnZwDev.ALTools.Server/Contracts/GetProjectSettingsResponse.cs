using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class GetProjectSettingsResponse
    {

        public List<string> mandatoryPrefixes { get; set; }
        public List<string> mandatorySuffixes { get; set; }
        public List<string> mandatoryAffixes { get; set; }

    }
}
