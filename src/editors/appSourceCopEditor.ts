import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { JsonFormEditor } from "./jsonFormEditor";

export class AppSourceCopEditor extends JsonFormEditor {

    constructor(devToolsContext : DevToolsExtensionContext) {
        super(devToolsContext, "AppSourceCop.json");

        this.fieldsOrder = ["name", "publisher", "version", "mandatoryAffixes", "mandatoryPrefix", 
            "mandatorySuffix", "supportedCountries", "targetVersion", "obsoleteTagPattern", 
            "obsoleteTagPatternDescription"];
    }

    protected getViewType() : string {
        return 'azALDevTools.AppSourceCopEditor';
    }

    protected getFieldsDefinition(): any {
        return [
            {
                "name": "name",
                "caption": "Name",
                "description": "The name of a previous version of this package with which you want to compare the current package for breaking changes. This property is being deprecated because the name of the previous extension should be the same as the current extension's name.",
                "type": "string"
            },
            {
                "name": "publisher",
                "caption": "Publisher",
                "description": "The publisher of a previous version of this package with which you want to compare the current package for breaking changes. This property is being deprecated because the name of the publisher of the previous extension should be the same as the current extension's publisher.",
                "type": "string"
            },
            {
                "name": "version",
                "caption": "Version",
                "type": "string",
                "description": "The version of a previous version of this package with which you want to compare the current package for breaking changes.",
                "pattern": "(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)"
            },
            {
                "name": "baselinePackageCachePath",
                "caption": "Baseline Package Cache Path",
                "description": "The path to the folder containing the baseline and its dependencies with which you want to compare the current package for breaking changes. By default, the package cache path for the current project is used (see 'al.packageCachePath' setting).",
                "type": "string"
            },
            {
                "name": "mandatoryAffixes",
                "caption": "Mandatory Affixes",
                "type": "list",
                "description": "Affixes that must be prepended or appended to the name of all new application objects, extension objects, and fields."
            },
            {
                "name": "mandatoryPrefix",
                "caption": "Mandatory Prefix",
                "type": "string",
                "description": "Prefix that must be prepended to the name of all new application objects, extension objects, and fields. This property is being deprecated in favor of mandatoryAffixes."
            },
            {
                "name": "mandatorySuffix",
                "caption": "Mandatory Suffix",
                "type": "string",
                "description": "Suffix that must be appended to the name of all new application objects, extension objects, and fields. This property is being deprecated in favor of mandatoryAffixes."
            },
            {
                "name": "supportedCountries",
                "caption": "Supported Countries",
                "description": "The set of country codes, in the alpha-2 ISO 3166 format, in which the application will be available.",
                "type": "list"
            },
            {
                "name": "targetVersion",
                "caption": "Target Version",
                "type": "string",
                "description": "Specifies the next minor version of the extension for the current branch. Only relevant when the default obsoleteTagPattern is used. For example this can take the values 16.0, 16.1 etc.",
                "pattern": "(\\d+)\\.(\\d+)"
            },
            {
                "name": "obsoleteTagVersion",
                "caption": "Obsolete Tag Version",
                "type": "string",
                "description": "Specifies the next Major.Minor version of the extension in the current branch in order to validate the ObsoleteTag values with AS0072. This is only relevant when the default obsoleteTagPattern '(\\d+)\\.(\\d+)' is used.",
                "pattern": "^(\\d+)\\.(\\d+)$"
            },
            {
                "name": "obsoleteTagAllowedVersions",
                "caption": "Obsolete Tag Allowed Versions",
                "type": "string",
                "description": "A comma-separated list of Major.Minor versions that will be allowed as ObsoleteTag values by AS0072. This is only relevant when the default obsoleteTagPattern '(\\d+)\\.(\\d+)' is used. ",
                "pattern": "^(\\d+)\\.(\\d+)(,(\\d+)\\.(\\d+))*$"
            },
            {
                "name": "obsoleteTagPattern",
                "caption": "Obsolete Tag Pattern",
                "description": "The Obsolete tag pattern to verify with Rule 76. This should be a valid regular expression. By default the pattern \\d+\\.\\d+ is used",
                "type": "string"
            },
            {
                "name": "obsoleteTagPatternDescription",
                "caption": "Obsolete Tag Pattern Description",
                "description": "A human-readable description for the ObsoleteTagPattern regular expression. By Default Major.Minor is used",
                "type": "longstring"
            },
            {
                "name": "obsoleteTagMinAllowedMajorMinor",
                "caption": "Obsolete Tag Min Allowed Major.Minor",
                "type": "string",
                "description": "The minimum version of ObsoleteTag (Major.Minor) allowed during compilation. Referencing an obsolete pending object with an obsolete tag lower than the specified version will trigger the rule AS0105. Note that enabling this setting has a performance impact."
            }
    

        ];
    }
}