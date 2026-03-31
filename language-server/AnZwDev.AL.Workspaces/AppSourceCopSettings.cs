using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnZwDev.AL.Workspaces
{
    public class AppSourceCopSettings
    {

        /// <summary>
        /// The name of a previous version of this package with which you want to compare the current package for breaking changes.
        /// This property is being deprecated because the name of the previous extension should be the same as the current extension's name.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// The publisher of a previous version of this package with which you want to compare the current package for breaking changes.
        /// This property is being deprecated because the name of the publisher of the previous extension should be the same as the current extension's publisher.
        /// </summary>
        [JsonPropertyName("publisher")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Publisher { get; set; }

        /// <summary>
        /// The version of a previous version of this package with which you want to compare the current package for breaking changes.
        /// Pattern: (\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)
        /// </summary>
        [JsonPropertyName("version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Version { get; set; }

        /// <summary>
        /// The path to the folder containing the baseline and its dependencies with which you want to compare the current package for breaking changes.
        /// By default, the package cache path for the current project is used (see 'al.packageCachePath' setting).
        /// </summary>
        [JsonPropertyName("baselinePackageCachePath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BaselinePackageCachePath { get; set; }

        /// <summary>
        /// The path to the folder containing packages from where objects are taken from validate move operations of application objects if any.
        /// If not specified validation for moved objects is not performed.
        /// </summary>
        [JsonPropertyName("sourceMovedObjectsPackagesCachePath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SourceMovedObjectsPackagesCachePath { get; set; }

        /// <summary>
        /// Affixes that must be prepended or appended to the name of all new application objects, extension objects, and fields.
        /// </summary>
        [JsonPropertyName("mandatoryAffixes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? MandatoryAffixes { get; set; }

        /// <summary>
        /// Prefix that must be prepended to the name of all new application objects, extension objects, and fields.
        /// This property is being deprecated in favor of mandatoryAffixes.
        /// </summary>
        [JsonPropertyName("mandatoryPrefix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MandatoryPrefix { get; set; }

        /// <summary>
        /// Suffix that must be appended to the name of all new application objects, extension objects, and fields.
        /// This property is being deprecated in favor of mandatoryAffixes.
        /// </summary>
        [JsonPropertyName("mandatorySuffix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MandatorySuffix { get; set; }

        /// <summary>
        /// The set of country codes, in the alpha-2 ISO 3166 format, in which the application will be available.
        /// </summary>
        [JsonPropertyName("supportedCountries")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? SupportedCountries { get; set; }

        /// <summary>
        /// Specifies the next Major.Minor version of the extension in the current branch in order to validate the ObsoleteTag values with AS0072.
        /// This is only relevant when the default obsoleteTagPattern '(\\d+)\\.(\\d+)' is used.
        /// This property is being deprecated in favor of obsoleteTagVersion.
        /// Pattern: ^(\\d+)\\.(\\d+)$
        /// </summary>
        [JsonPropertyName("targetVersion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TargetVersion { get; set; }

        /// <summary>
        /// Specifies the next Major.Minor version of the extension in the current branch in order to validate the ObsoleteTag values with AS0072.
        /// This is only relevant when the default obsoleteTagPattern '(\\d+)\\.(\\d+)' is used.
        /// Pattern: ^(\\d+)\\.(\\d+)$
        /// </summary>
        [JsonPropertyName("obsoleteTagVersion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ObsoleteTagVersion { get; set; }

        /// <summary>
        /// A comma-separated list of Major.Minor versions that will be allowed as ObsoleteTag values by AS0072.
        /// This is only relevant when the default obsoleteTagPattern '(\\d+)\\.(\\d+)' is used.
        /// Pattern: ^(\\d+)\\.(\\d+)(,(\\d+)\\.(\\d+))*$
        /// </summary>
        [JsonPropertyName("obsoleteTagAllowedVersions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ObsoleteTagAllowedVersions { get; set; }

        /// <summary>
        /// The Obsolete tag pattern used by AS0076. This should be a valid regular expression.
        /// By default, the pattern '(\\d+)\\.(\\d+)' is used.
        /// </summary>
        [JsonPropertyName("obsoleteTagPattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ObsoleteTagPattern { get; set; }

        /// <summary>
        /// A human-readable description for the ObsoleteTagPattern regular expression.
        /// This is used in diagnostics reported by AS0076. By default, 'Major.Minor' is used.
        /// </summary>
        [JsonPropertyName("obsoleteTagPatternDescription")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ObsoleteTagPatternDescription { get; set; }

        /// <summary>
        /// The minimum version of ObsoleteTag (Major.Minor) allowed during compilation.
        /// Referencing an obsolete pending object with an obsolete tag lower than the specified version will trigger the rule AS0105.
        /// Note that enabling this setting has a performance impact.
        /// </summary>
        [JsonPropertyName("obsoleteTagMinAllowedMajorMinor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ObsoleteTagMinAllowedMajorMinor { get; set; }

        /// <summary>
        /// Specifies whether source symbols should also be validated by rule AS0105.
        /// By default, only reference symbols are validated. Note that enabling this setting has a performance impact.
        /// </summary>
        [JsonPropertyName("obsoleteTagMinAllowedMajorMinorOnSourceSymbols")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool ObsoleteTagMinAllowedMajorMinorOnSourceSymbols { get; set; }

        /// <summary>
        /// Specifies whether the breaking change validation should be enabled on internal symbols.
        /// </summary>
        [JsonPropertyName("validateInternalSymbols")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool ValidateInternalSymbols { get; set; }

        /// <summary>
        /// Specifies whether the breaking change validation should be enabled on OnPrem symbols.
        /// </summary>
        [JsonPropertyName("validateOnPremSymbols")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool ValidateOnPremSymbols { get; set; }

        /// <summary>
        /// Specifies whether the breaking change validation should be enabled on symbols who are obsolete in the baseline.
        /// </summary>
        [JsonPropertyName("validateObsoleteSymbols")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool ValidateObsoleteSymbols { get; set; }


        public static AppSourceCopSettings? LoadFromFile(string? filePath)
        {
            if ((!String.IsNullOrWhiteSpace(filePath)) && (File.Exists(filePath)))
            {
                try
                {
                    var content = FileHelper.ReadAllTextWithRetry(filePath);
                    return JsonSerializer.Deserialize<AppSourceCopSettings>(content);
                }
                catch (Exception) { }
            }
            return null;
        }

    }
}
