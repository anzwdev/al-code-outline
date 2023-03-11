using AnZwDev.ALTools.ALSymbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class BaseSymbolsResponse
    {

        public ALSymbol root { get; set; }
        public bool error { get; set; }

        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string errorMessage { get; set; }

        public BaseSymbolsResponse()
        {
            this.error = false;
            this.errorMessage = null;
        }

        public void SetError(string errorMessage)
        {
            this.error = true;
            this.errorMessage = errorMessage;
        }

    }
}
