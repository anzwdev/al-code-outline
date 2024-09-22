using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetFileContentRequestHandler : RequestHandler
    {

        public GetFileContentRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getfilecontent", UseSingleObjectParameterDeserialization = true)]
        public GetFileContentResponse GetFileContent(GetFileContentRequest parameters)
        {
            GetFileContentResponse response = new GetFileContentResponse();
            if ((!String.IsNullOrWhiteSpace(parameters.path)) && (File.Exists(parameters.path)))
            {
                try
                {
                    var project = this.LanguageServerHost.ALDevToolsServer.Workspace.FindProject(parameters.path);
                    if (project != null)
                        response.content = FileUtils.SafeReadAllText(parameters.path);
                }
                catch (Exception e)
                {
                    response.content = null;
                    this.LogError(e);
                }
            }
            return response;
        }

    }
}
