using AnZwDev.ALTools.DuplicateCodeSearch;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class FindDuplicateCodeRequestHandler : RequestHandler
    {

        public FindDuplicateCodeRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/findDuplicateCode", UseSingleObjectParameterDeserialization = true)]
        public FindDuplicateCodeResponse FindDuplicateCode(FindDuplicateCodeRequest parameters)
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

    }
}
