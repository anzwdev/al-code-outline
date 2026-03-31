using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.Metadata
{
    internal class AppJsonMetadata
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("application")]
        public string? Application { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("applicationInsightsConnectionString")]
        public string? ApplicationInsightsConnectionString { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("applicationInsightsKey")]
        public string? ApplicationInsightsKey { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("brief")]
        public string? Brief { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("build")]
        public AppJsonBuild? Build { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("contextSensitiveHelpUrl")]
        public string? ContextSensitiveHelpUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("dependencies")]
        public AppJsonDependency[]? Dependencies { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EULA")]
        public string? Eula { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("features")]
        public string[]? Features { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("help")]
        public string? Help { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("helpBaseUrl")]
        public string? HelpBaseUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("idRange")]
        public AppJsonIdRange? IdRange { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("idRanges")]
        public AppJsonIdRange[]? IdRanges { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("internalsVisibleTo")]
        public AppJsonInternalsVisibleTo[]? InternalsVisibleTo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("keyVaultUrls")]
        public string[]? KeyVaultUrls { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("preprocessorSymbols")]
        public string[]? PreprocessorSymbols { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("privacyStatement")]
        public string? PrivacyStatement { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("propagateDependencies")]
        public bool? PropagateDependencies { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("resourceExposurePolicy")]
        public AppJsonResourceExposurePolicy? ResourceExposurePolicy { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("resourceFolders")]
        public string[]? ResourceFolders { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("runtime")]
        public string? Runtime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("screenshots")]
        public string[]? Screenshots { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("showMyCode")]
        public bool? ShowMyCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("source")]
        public AppJsonSourceCodeSource? Source { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("supportedLocales")]
        public string[]? SupportedLocales { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("suppressWarnings")]
        public string[]? SuppressWarnings { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("target")]
        public string? Target { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("test")]
        public string? Test { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        public ApplicationSymbol CreateSymbol(string sourceFileName)
        {
            return new ApplicationSymbol()
            {
                ReferenceSourceFileName = sourceFileName,

                AppId = Id.NotNull(),
                Name = Name.NotNull(),
                Publisher = Publisher.NotNull(),
                Version = ALSymbolExpressionParser.ParseVersion(Version, 0, 0, 0, 0),

                Metadata = CreateMetadataSymbol()
            };
        }

        private List<ApplicationDependency> CreateDependenciesSymbol()
        {
            if ((Dependencies == null) || (Dependencies.Length == 0))
                return new List<ApplicationDependency>();

            var list = new List<ApplicationDependency>(Dependencies.Length);
            for (int i = 0; i < Dependencies.Length; i++)
                list.Add(Dependencies[i].CreateSymbol());
            return list;
        }

        private Dictionary<string, InternalsVisibleToModule> CreateInternalsVisibleToModules()
        {
            var collection = new Dictionary<string, InternalsVisibleToModule>(StringComparer.OrdinalIgnoreCase);
            if (InternalsVisibleTo != null)
                for (int i = 0; i < InternalsVisibleTo.Length; i++)
                {
                    var symbol = InternalsVisibleTo[i].CreateSymbol();
                    if (String.IsNullOrWhiteSpace(symbol.AppId))
                    {
                        if (collection.ContainsKey(symbol.AppId))
                            collection[symbol.AppId] = symbol;
                        else
                            collection.Add(symbol.AppId, symbol);
                    }
                }
            return collection;
        }

        private List<IdRange> CreateIdRangesSymbols()
        {
            var list = new List<IdRange>();

            if (IdRange != null)
                list.Add(IdRange.CreateSymbol());

            if ((IdRanges != null) && (IdRanges.Length > 0))
                for (int i = 0; i < IdRanges.Length; i++)
                    list.Add(IdRanges[i].CreateSymbol());

            return list;
        }

        private ALResourceExposurePolicy CreateResourceExposurePolicySymbol()
        {
            if (ResourceExposurePolicy != null)
                return new ALResourceExposurePolicy()
                {
                    AllowDebugging = ResourceExposurePolicy.AllowDebugging ?? true,
                    AllowDownloadingSource = ResourceExposurePolicy.AllowDownloadingSource ?? true,
                    ApplyToDevExtension = ResourceExposurePolicy.ApplyToDevExtension ?? true,
                    IncludeSourceInSymbolFile = ResourceExposurePolicy.IncludeSourceInSymbolFile ?? true
                };

            var showMyCodeValue = ShowMyCode ?? true;
            return new ALResourceExposurePolicy()
            {
                AllowDebugging = showMyCodeValue,
                AllowDownloadingSource = showMyCodeValue,
                ApplyToDevExtension = showMyCodeValue,
                IncludeSourceInSymbolFile = showMyCodeValue
            };
        }

        private ApplicationMetadata CreateMetadataSymbol()
        {
            var metadata = new ApplicationMetadata()
            {
                BCApplicationVersion = ALSymbolExpressionParser.ParseVersion(Application, 0, 0, 0, 0),
                BCPlatformVersion = ALSymbolExpressionParser.ParseVersion(Platform, 0, 0, 0, 0),
                BCTestVersion = ALSymbolExpressionParser.ParseVersion(Test, 0, 0, 0, 0),
                BCRuntimeVersion = ALSymbolExpressionParser.ParseVersion(Runtime, 0, 0),

                InternalsVisibleToModules = CreateInternalsVisibleToModules(),
                Dependencies = CreateDependenciesSymbol(),

                PropagateDependencies = PropagateDependencies ?? false,
                IdRanges = CreateIdRangesSymbols(),
                PreprocessorSymbols = PreprocessorSymbols,
                ResourceExposurePolicy = CreateResourceExposurePolicySymbol(),

                Target = GetApplicationCompilationTarget(Target),
                SuppressWarnings = SuppressWarnings,

                ApplicationInsightsConnectionString = ApplicationInsightsConnectionString,
                ApplicationInsightsKey = ApplicationInsightsKey,
                Brief = Brief,
                ContextSensitiveHelpUrl = ContextSensitiveHelpUrl,
                Description = Description,
                Eula = Eula,
                Help = Help,
                HelpBaseUrl = HelpBaseUrl,
                PrivacyStatement = PrivacyStatement,
                Logo = Logo,
                KeyVaultUrls = KeyVaultUrls,
                Url = Url,
                Screenshots = Screenshots,

                Features = GetApplicationCompilationFeatures(Features),
                ResourceFolders = ResourceFolders,
                SupportedLocales = SupportedLocales,

                Build = Build?.CreateSymbol(),
                Source = Source?.CreateSymbol()
            };
            metadata.AddMissingMicrosoftDependencies();
            return metadata;
        }

        private ApplicationCompilationTarget GetApplicationCompilationTarget(string? value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return ApplicationCompilationTarget.Cloud;

            if ((value.Equals("Solution", StringComparison.OrdinalIgnoreCase)) ||
                (value.Equals("Internal", StringComparison.OrdinalIgnoreCase)) ||
                (value.Equals("OnPrem", StringComparison.OrdinalIgnoreCase)))
                return ApplicationCompilationTarget.OnPrem;

            return ApplicationCompilationTarget.Cloud;
        }

        private ApplicationCompilationFeatures GetApplicationCompilationFeatures(string[]? features)
        {
            var featuresValue = new ApplicationCompilationFeatures();

            if (features != null)
                for (int i = 0; i < features.Length; i++)
                {
                    if (features[i] != null)
                    {
                        var feature = features[i];
                        if (feature.Equals("TranslationFile", StringComparison.OrdinalIgnoreCase))
                            featuresValue.TranslationFile = true;
                        else if (feature.Equals("GenerateCaptions", StringComparison.OrdinalIgnoreCase))
                            featuresValue.GenerateCaptions = true;
                        else if (feature.Equals("ExcludeGeneratedTranslations", StringComparison.OrdinalIgnoreCase))
                            featuresValue.ExcludeGeneratedTranslations = true;
                        else if (feature.Equals("NoImplicitWith", StringComparison.OrdinalIgnoreCase))
                            featuresValue.NoImplicitWith = true;
                        else if (feature.Equals("NoPromotedActionProperties", StringComparison.OrdinalIgnoreCase))
                            featuresValue.NoPromotedActionProperties = true;
                        else if (feature.Equals("GenerateLockedTranslations", StringComparison.OrdinalIgnoreCase))
                            featuresValue.GenerateLockedTranslations = true;
                        else if (feature.Equals("AllTranslationItems", StringComparison.OrdinalIgnoreCase))
                            featuresValue.AllTranslationItems = true;
                        else if (feature.Equals("UseLegacyAnalyzerStrategy", StringComparison.OrdinalIgnoreCase))
                            featuresValue.UseLegacyAnalyzerStrategy = true;
                    }
                }
            return featuresValue;
        }

    }
}
