using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Metadata;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Metadata
{
    internal static class NavAppManifestLoader
    {
     
        public static ApplicationSymbol CreateApplicationSymbol(NavAppManifest navAppManifest, string sourceFileName)
        {
            return new ApplicationSymbol()
            {
                ReferenceSourceFileName = sourceFileName,

                AppId = navAppManifest.AppId.ToString(),
                Name = navAppManifest.AppName.NotNull(),
                Publisher = navAppManifest.AppPublisher.NotNull(),
                Version = navAppManifest.AppVersion ?? new Version(0, 0, 0, 0),

                Metadata = CreateMetadataSymbol(navAppManifest)
            };
        }

        private static ApplicationMetadata CreateMetadataSymbol(NavAppManifest navAppManifest)
        {
            var metadata = new ApplicationMetadata()
            {
                BCApplicationVersion = navAppManifest.Application ?? new Version(0, 0, 0, 0),
                BCPlatformVersion = navAppManifest.Platform ?? new Version(0, 0, 0, 0),
                BCTestVersion = navAppManifest.Test ?? new Version(0, 0, 0, 0),
                BCRuntimeVersion = navAppManifest.Runtime ?? new Version(0, 0),

                InternalsVisibleToModules = CreateInternalsVisibleToModules(navAppManifest),
                Dependencies = CreateDependenciesSymbol(navAppManifest),

                PropagateDependencies = navAppManifest.PropagateDependencies,
                IdRanges = CreateIdRangesSymbols(navAppManifest),
                PreprocessorSymbols = navAppManifest.PreprocessorSymbols?.ToArray(),
                ResourceExposurePolicy = CreateResourceExposurePolicySymbol(navAppManifest),

                Target = GetApplicationCompilationTarget(navAppManifest),
                SuppressWarnings = navAppManifest.SuppressWarnings?.ToArray(),

                ApplicationInsightsConnectionString = navAppManifest.ApplicationInsightsConnectionString,
                ApplicationInsightsKey = navAppManifest.ApplicationInsightsKey.ToString(),
                Brief = navAppManifest.AppBrief,
                ContextSensitiveHelpUrl = navAppManifest.ContextSensitiveHelpUrl,
                Description = navAppManifest.AppDescription,
                Eula = navAppManifest.AppEula,
                Help = navAppManifest.AppHelp,
                HelpBaseUrl = navAppManifest.AppHelpBaseUrl,
                PrivacyStatement = navAppManifest.AppPrivacyStatement,
                Logo = navAppManifest.AppLogo,
                KeyVaultUrls = navAppManifest.KeyVaultUrls?.ToArray(),
                Url = navAppManifest.AppUrl,
                Screenshots = navAppManifest.AppScreenshots?.ToArray(),

                Features = GetApplicationCompilationFeatures(navAppManifest),
                ResourceFolders = navAppManifest.AppResourceFolders?.ToArray(),
                SupportedLocales = navAppManifest.AppSupportedLocales?.Locales,

                Build = GetApplicationBuildInformationSymbol(navAppManifest),
                Source = GetApplicationSourceCodeLocationSymbol(navAppManifest)
            };
            metadata.AddMissingMicrosoftDependencies();
            return metadata;
        }

        private static ApplicationSourceCodeLocation GetApplicationSourceCodeLocationSymbol(NavAppManifest navAppManifest)
        {
            return new ApplicationSourceCodeLocation()
            {
                Commit = navAppManifest.SourceSpecification?.Commit,
                RepositoryUrl = navAppManifest.SourceSpecification?.RepositoryUrl
            };
        }

        private static ApplicationBuildInformation GetApplicationBuildInformationSymbol(NavAppManifest navAppManifest)
        {
            return new ApplicationBuildInformation()
            {
                By = navAppManifest.BuildInformation?.By,
                Url = navAppManifest.BuildInformation?.Url,
            };
        }

        private static ApplicationCompilationFeatures GetApplicationCompilationFeatures(NavAppManifest navAppManifest)
        {
            return new ApplicationCompilationFeatures()
            {
                ExcludeGeneratedTranslations = (navAppManifest.CompilerFeatures & CompilerFeatures.ExcludeGeneratedTranslations) == CompilerFeatures.ExcludeGeneratedTranslations,
                NoImplicitWith = (navAppManifest.CompilerFeatures & CompilerFeatures.NoImplicitWith) == CompilerFeatures.NoImplicitWith,
                GenerateLockedTranslations = (navAppManifest.CompilerFeatures & CompilerFeatures.GenerateLockedTranslations) == CompilerFeatures.GenerateLockedTranslations,
                NoPromotedActionProperties = (navAppManifest.CompilerFeatures & CompilerFeatures.NoPromotedActionProperties) == CompilerFeatures.NoPromotedActionProperties,
                AllTranslationItems = (navAppManifest.CompilerFeatures & CompilerFeatures.GenerateTranslationForEverything) == CompilerFeatures.GenerateTranslationForEverything,
                GenerateCaptions = (navAppManifest.CompilerFeatures & CompilerFeatures.GenerateTranslationCaptionRelated) == CompilerFeatures.GenerateTranslationCaptionRelated,
                TranslationFile = (navAppManifest.CompilerFeatures & CompilerFeatures.GenerateXliffTranslationFile) == CompilerFeatures.GenerateXliffTranslationFile,
                UseLegacyAnalyzerStrategy = false
            };
        }

        private static ApplicationCompilationTarget GetApplicationCompilationTarget(NavAppManifest navAppManifest)
        {
#pragma warning disable CS0618 // Handle obsolete values and convert them to current ones
            switch (navAppManifest.Target)
            {
                case Microsoft.Dynamics.Nav.CodeAnalysis.CompilationTarget.Solution:
                case Microsoft.Dynamics.Nav.CodeAnalysis.CompilationTarget.Internal:
                case Microsoft.Dynamics.Nav.CodeAnalysis.CompilationTarget.OnPrem:
                    return ApplicationCompilationTarget.OnPrem;
                case Microsoft.Dynamics.Nav.CodeAnalysis.CompilationTarget.Personalization:
                case Microsoft.Dynamics.Nav.CodeAnalysis.CompilationTarget.Extension:
                case Microsoft.Dynamics.Nav.CodeAnalysis.CompilationTarget.Cloud:
                    return ApplicationCompilationTarget.Cloud;
                default:
                    return ApplicationCompilationTarget.Cloud;
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private static List<IdRange> CreateIdRangesSymbols(NavAppManifest navAppManifest)
        {
            var list = new List<IdRange>();

            if (navAppManifest.IdSpaces?.ApplicationObjectIdRanges != null)
            {
                for (int i=0; i<navAppManifest.IdSpaces.ApplicationObjectIdRanges.Length; i++)
                {
                    var item = navAppManifest.IdSpaces.ApplicationObjectIdRanges[i];
                    list.Add(new IdRange()
                    {
                        From = item.ApplicationObjectIdRangeStart,
                        To = item.ApplicationObjectIdRangeEnd
                    });
                }
            }
            return list;
        }

        private static ALResourceExposurePolicy CreateResourceExposurePolicySymbol(NavAppManifest navAppManifest)
        {
            return new ALResourceExposurePolicy()
            {
                AllowDebugging = navAppManifest.ResourceExposurePolicy?.AllowDebugging ?? true,
                AllowDownloadingSource = navAppManifest.ResourceExposurePolicy?.AllowDownloadingSource ?? true,
                ApplyToDevExtension = navAppManifest.ResourceExposurePolicy?.ApplyToDevExtension ?? true,
                IncludeSourceInSymbolFile = navAppManifest.ResourceExposurePolicy?.IncludeSourceInSymbolFile ?? true
            };
        }


        private static List<ApplicationDependency> CreateDependenciesSymbol(NavAppManifest navAppManifest)
        {
            var list = new List<ApplicationDependency>();
            if (navAppManifest.Dependencies != null)
            {
                foreach (var item in navAppManifest.Dependencies)
                {
                    list.Add(new ApplicationDependency()
                    {
                        Id = item.AppId.ToString(),
                        Name = item.Name,
                        Publisher = item.Publisher,
                        Version = item.MinVersion ?? new Version(0, 0, 0, 0)
                    });
                }
            }
            return list;
        }

        private static Dictionary<string, InternalsVisibleToModule> CreateInternalsVisibleToModules(NavAppManifest navAppManifest)
        {
            var collection = new Dictionary<string, InternalsVisibleToModule>(StringComparer.OrdinalIgnoreCase);

            if (navAppManifest.InternalsVisibleTo != null)
            {
                foreach (var item in navAppManifest.InternalsVisibleTo)
                {
                    var symbol = new InternalsVisibleToModule()
                    {
                        AppId = item.AppId.ToString(),
                        Name = item.Name,
                        Publisher = item.Publisher
                    };

                    if (String.IsNullOrWhiteSpace(symbol.AppId))
                    {
                        if (!collection.ContainsKey(symbol.AppId))
                            collection.Add(symbol.AppId, symbol);
                    }
                }
            }

            return collection;
        }

    }
}
