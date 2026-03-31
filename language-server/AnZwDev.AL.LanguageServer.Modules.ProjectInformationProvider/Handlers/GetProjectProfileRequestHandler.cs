using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts;
using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.ProjectProfile;
using AnZwDev.AL.Symbols.Platform;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Handlers
{
    internal class GetProjectProfileRequestHandler : RequestHandler
    {

        public GetProjectProfileRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/projectinformation/getprojectprofile", UseSingleObjectParameterDeserialization = true)]
        public GetProjectProfileResponse GetProjectProfile(GetProjectProfileRequest parameters)
        {
            var project = this.Services
                .GetService<Workspace>()?
                .Projects.FindByPath(parameters.Path ?? "");

            PIAffixesSettings? piAffixesSettings = null;
            PIProjectProperties? piProjectProperties = null;
            PIProjectPlatformCapabilities? piProjectPlatformCapabilities = null;

            if (project != null)
            {
                // Get project affixes settings
                piAffixesSettings = new PIAffixesSettings()
                {
                    MandatoryAffixes = project.Settings.MandatoryAffixes,
                    MandatoryPrefixes = project.Settings.MandatoryPrefixes,
                    MandatorySuffixes = project.Settings.MandatorySuffixes
                };


                // Get project symbols to get runtime version and ID ranges
                var projectSymbols = project.SymbolsProvider.ProjectCodeSymbolsProvider.GetSymbols();
                if (projectSymbols != null)
                {
                    var firstId = 0;
                    var lastId = 0;
                    if (projectSymbols.Metadata.IdRanges.Count > 0)
                    {
                        firstId = projectSymbols.Metadata.IdRanges[0].From;
                        lastId = projectSymbols.Metadata.IdRanges[0].To;
                        for (var i = 1; i < projectSymbols.Metadata.IdRanges.Count; i++)
                        {
                            if (projectSymbols.Metadata.IdRanges[i].From < firstId)
                                firstId = projectSymbols.Metadata.IdRanges[i].From;
                            if (projectSymbols.Metadata.IdRanges[i].To > lastId)
                                lastId = projectSymbols.Metadata.IdRanges[i].To;
                        }
                    }

                    piProjectProperties = new PIProjectProperties()
                    {
                        Path = project.RootPath,
                        Name = projectSymbols.Name,
                        RuntimeVersion = projectSymbols.Metadata.BCRuntimeVersion.ToString(),
                        FirstIdRangeStart = firstId,
                        LastIdRangeEnd = lastId
                    };


                    var platformCapabilities = new PlatformCapabilities(projectSymbols.Metadata.BCRuntimeVersion);
                    piProjectPlatformCapabilities = new PIProjectPlatformCapabilities()
                    {
                        Interfaces = platformCapabilities.Interfaces,
                        AppAreasInheritance = platformCapabilities.AppAreasInheritance,
                        Namespaces = platformCapabilities.Namespaces,
                        TableToolTips = platformCapabilities.TableToolTips
                    };
                }
            }

            return new GetProjectProfileResponse()
            { 
                Profile = new PIProjectProfile()
                {
                    Affixes = piAffixesSettings,
                    Properties = piProjectProperties,
                    PlatformCapabilities = piProjectPlatformCapabilities
                }
            };
        }

    }
}
