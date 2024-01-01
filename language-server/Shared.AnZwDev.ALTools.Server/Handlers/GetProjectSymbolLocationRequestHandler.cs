﻿using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetProjectSymbolLocationRequestHandler : RequestHandler
    {

        public GetProjectSymbolLocationRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/projectsymbollocation", UseSingleObjectParameterDeserialization = true)]
        public GetProjectSymbolLocationResponse GetProjectSymbolLocation(GetProjectSymbolLocationRequest parameters)
        {
            GetProjectSymbolLocationResponse response = new GetProjectSymbolLocationResponse();
            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.projectPath);
                if (project != null)
                {
                    ALProjectSymbolsLibrarySource symbolsSource = new ALProjectSymbolsLibrarySource(project);
                    ALSymbol symbol = new ALSymbol(parameters.kind, parameters.name);
                    response.location = symbolsSource.GetSymbolSourceProjectLocation(symbol);
                }
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            return response;
        }

    }
}
