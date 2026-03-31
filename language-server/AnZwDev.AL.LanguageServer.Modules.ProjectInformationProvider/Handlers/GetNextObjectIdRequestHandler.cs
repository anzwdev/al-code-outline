using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.InformationProviders.ObjectIds;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers
{
    internal class GetNextObjectIdRequestHandler : RequestHandler
    {

        public GetNextObjectIdRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/projectinformation/getnextobjectid", UseSingleObjectParameterDeserialization = true)]
        public GetNextObjectIdResponse GetNextObjectId(GetNextObjectIdRequest parameters)
        {
            int id = -1;

            if (parameters.Path != null)
            {
                var project = this.Services
                    .GetService<Workspace>()?
                    .Projects.FindByPath(parameters.Path);
                if (project != null)
                    id = ObjectIdInformationProvider.GetNextFreeId(project, parameters.Kind);
            }

            return new GetNextObjectIdResponse() 
            { 
                Kind = parameters.Kind, 
                Id = id 
            };
        }

    }
}
