using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public partial class ApplicationMetadata
    {

        public required Version BCApplicationVersion { get; init; }
        public required Version BCPlatformVersion { get; init; }
        public required Version? BCTestVersion { get; init; }
        public required Version BCRuntimeVersion { get; init; }
        public required Dictionary<string, InternalsVisibleToModule> InternalsVisibleToModules { get; init; }
        public required List<ApplicationDependency> Dependencies { get; init; }
        public required bool PropagateDependencies { get; init; }
        public required List<IdRange> IdRanges { get; init; }
        public required string[]? PreprocessorSymbols { get; init; }
        public required ALResourceExposurePolicy ResourceExposurePolicy { get; init; }
        public required ApplicationCompilationTarget Target { get; init; }
        public required string[]? SuppressWarnings { get; init; }

        public required string? ApplicationInsightsConnectionString { get; init; }
        public required string? ApplicationInsightsKey { get; init; }
        public required string? Brief { get; init; }
        public required string? ContextSensitiveHelpUrl { get; init; }
        public required string? Description { get; init; }
        public required string? Eula { get; init; }
        public required string? Help { get; init; }
        public required string? HelpBaseUrl { get; init; }
        public required string? PrivacyStatement { get; init; }
        public required string? Logo { get; init; }
        public required string[]? KeyVaultUrls { get; init; }
        public required string? Url { get; init; }
        public required string[]? Screenshots { get; init; }

        public required ApplicationCompilationFeatures Features { get; init; }
        public required string[]? ResourceFolders { get; init; }
        public required string[]? SupportedLocales { get; init; }

        public required ApplicationBuildInformation? Build { get; init; }
        public required ApplicationSourceCodeLocation? Source { get; init; }

    }
}
