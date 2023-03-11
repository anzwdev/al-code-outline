using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetTableFieldsListRequest : GetSymbolsInformationRequest
    {

        public string table { get; set; }
        public bool includeDisabled { get; set; }
        public bool includeObsolete { get; set; }
        public bool includeNormal { get; set; }
        public bool includeFlowFields { get; set; }
        public bool includeFlowFilters { get; set; }
        public bool includeToolTips { get; set; }
        public string[] toolTipsSourceDependencies { get; set; }

    }
}
