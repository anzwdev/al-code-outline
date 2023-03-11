using AnZwDev.ALTools.DuplicateCodeSearch;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class FindDuplicateCodeRequestHandler : BaseALRequestHandler<FindDuplicateCodeRequest, FindDuplicateCodeResponse>
    {

        public FindDuplicateCodeRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/findDuplicateCode")
        {
        }

#pragma warning disable 1998
        protected override async Task<FindDuplicateCodeResponse> HandleMessage(FindDuplicateCodeRequest parameters, RequestContext<FindDuplicateCodeResponse> context)
        {
            FindDuplicateCodeResponse response = new FindDuplicateCodeResponse();
            try
            {
                DCDuplicateCodeAnalyzer analyzer = new DCDuplicateCodeAnalyzer(parameters.minNoOfStatements, parameters.skipObsoleteCodeLevel);
                response.duplicates = analyzer.FindDuplicates(this.Server.Workspace, parameters.path);
            }
            catch (Exception e)
            {
                response.SetError(e);
                this.LogError(e);
            }
            return response;
        }
#pragma warning restore 1998
    }
}
