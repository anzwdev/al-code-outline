import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { JsonFormEditor } from "../jsonFormEditor";

export class AppJsonEditor extends JsonFormEditor {
    
    constructor(devToolsContext : DevToolsExtensionContext) {
        super(devToolsContext, "app.json");
    }

    protected getViewType() : string {
        return 'azALDevTools.AppJsonEditor';
    }

    protected getFieldsDefinition(): any {
        return [
            {
                "caption": "General",
                "type": "group"
            },
            {
                "name": "id",
                "caption": "Id",
                "type": "string",
                "description": "GUID of the app package"
            },
            {
                "name": "name",
                "caption": "Name",
                "type": "string",
                "description": "Name of the app package"
            },
            {
                "name": "description",
                "caption": "Description",
                "type": "string",
                "description": "Description of the app package"
            },
            {
                "name": "publisher",
                "caption": "Publisher",
                "type": "string",
                "description": "Name of the publisher of the app package"
            },
            {
                "name": "brief",
                "caption": "Brief",
                "type": "string",
                "description": "Brief description of the app package"
            },
            {
                "name": "version",
                "caption": "Version",
                "type": "string",
                "description": "Version of the app package in the format X.Y.U.Z"
            },
            {
                "name": "privacyStatement",
                "caption": "Privacy Statement",
                "type": "string",
                "description": "URL to the privacy statement"
            },
            {
                "name": "propagateDependencies",
                "caption": "Propagate Dependencies",
                "type": "boolean",
                "description": "Specifies whether the dependencies of this project should be propagated as direct dependencies of projects that depend on this one."
            },
            {
                "caption": "Dependencies",
                "type": "group"
            },
            {
                "name": "dependencies",
                "caption": "Dependencies",
                "description": "List of dependent packages for this app package",
                "type": "array",
                "fields": [
                    {
                        "name": "id",
                        "caption": "Id",
                        "style": "width: 80px"
                    },
                    {
                        "name": "name",
                        "caption": "Name",
                        "style": "width: 40%"
                    },
                    {
                        "name": "publisher",
                        "caption": "Publisher",
                        "style": "width: 40%"
                    },
                    {
                        "name": "version",
                        "caption": "Version",
                        "style": "width: 80px"
                    }
                ]
            },
            {
                "caption": "Internals Visibility",
                "type": "group"
            },
            {
                "name": "internalsVisibleTo",
                "caption": "Internals Visible To",
                "description": "List of packages that can access internals in this package",
                "type": "array",
                "fields": [
                    {
                        "name": "id",
                        "caption": "Id",
                        "style": "width: 80px"
                    },
                    {
                        "name": "name",
                        "caption": "Name",
                        "style": "width: 40%"
                    },
                    {
                        "name": "publisher",
                        "caption": "Publisher",
                        "style": "width: 40%"
                    },
                    {
                        "name": "version",
                        "caption": "Version",
                        "style": "width: 80px"
                    }
                ]
            },
            {
                "caption": "Additional",
                "type": "group"
            },
            {
                "name": "EULA",
                "caption": "EULA",
                "type": "string",
                "description": "URL to the End User License Agreement for the app package"
            },
            {
                "name": "help",
                "caption": "Help",
                "type": "string",
                "description": "URL to the help content for the app package"
            },
            {
                "name": "url",
                "caption": "Url",
                "type": "string",
                "description": "URL of the app package"
            },
            {
                "name": "logo",
                "caption": "Logo",
                "type": "string",
                "description": "Relative path to the app package logo from the root of the package"
            },
            {
                "name": "screenshots",
                "caption": "Screenshots",
                "type": "list",
                "items": {
                    "type": "string"
                },
                "description": "Relative paths to screenshot files that should be contained in the app package"
            },
            {
                "name": "platform",
                "caption": "Platform",
                "type": "string",
                "description": "Version of the dependent platform in the format X.Y.U.Z"
            },
            {
                "name": "application",
                "caption": "Application",
                "type": "string",
                "description": "Version of the dependent application in the format X.Y.U.Z"
            },
            {
                "name": "test",
                "caption": "Test",
                "type": "string",
                "description": "Version of the dependent test framework in the format X.Y.U.Z"
            },
            {
                "name": "target",
                "caption": "Target",
                "type": "string",
                "enum": [
                    "Extension",
                    "Internal",
                    "Cloud",
                    "OnPrem"
                ],
                "description": "Compilation target"
            },
            {
                "name": "idRanges",
                "caption": "Id Ranges",
                "description": "An optional set of ranges for application object IDs. For all objects outside the range, a compilation error will be raised.",
                "type": "array",
                "fields": [
                    { 
                        "name": "from",
                        "caption": "From",
                        "type": "number",
                        "minimum": 1,
                        "style": "width: 50%"
                    },
                    {
                        "name": "to",
                        "caption": "To",
                        "type": "number",
                        "minimum": 1,
                        "style": "width: 50%"
                    }
                ]
            },
            {
                "name": "idRange",
                "caption": "Id Range",
                "$ref": "#/definitions/customIdRange",
                "description": "An optional range for application object IDs. For all objects outside the range, a compilation error will be raised."
            },
            {
                "name": "features",
                "caption": "Features",
                "type": "array",
                "items": {
                    "type": "string",
                    "enum": [
                        "TranslationFile",
                        "GenerateCaptions",
                        "ExcludeGeneratedTranslations"
                    ],
                    "enumDescriptions": [
                        "Generate and utilize translation files in xliff format",
                        "Generate translation entries for captions for all application objects",
                        "Exclude the generated translation file from the app"
                    ]
                },
                "description": "Optional/experimental compiler features can be enabled by specifying them."
            },
            {
                "name": "showMyCode",
                "caption": "Show My Code",
                "type": "boolean",
                "description": "Allows the code to be debugged from other extensions when it has been published. The default setting is false."
            },
            {
                "name": "runtime",
                "caption": "Runtime",
                "type": "string",
                "enum": [
                    "5.0",
                    "4.0",
                    "3.0",
                    "2.0",
                    "1.0"
                ],
                "enumDescriptions": [
                    "Business Central 2020 release wave 1",
                    "Business Central 2019 release wave 2",
                    "Business Central Spring '19 Release",
                    "Business Central Fall '18 Release",
                    "Business Central Spring '18 Release"
                ]            },
            {
                "name": "contextSensitiveHelpUrl",
                "caption": "Context Sensitive Help Url",
                "type": "string",
                "format": "uri",
                "description": "Defines the base help URL for objects defined in this extension. The URL can contain a placeholder for the user locale: {0}."
            },
            {
                "name": "helpBaseUrl",
                "caption": "Help Base Url",
                "type": "string",
                "format": "uri",
                "description": "The base of the help URL that overrides the default help URL. The URL must contain a placeholder for the user locale: {0}."
            },
            {
                "name": "supportedLocales",
                "caption": "Supported Locales",
                "description": "List of locales supported by this app in the format <languageCode>-<CountryCode> i.e. en-US, en-CA, da-DK. The first locale on the list is considered the default.",
                "type": "array",
                "items": {
                    "type": "string"
                }
            },
            {
                "name": "applicationInsightsKey",
                "caption": "Application Insights Key",
                "type": "string",
                "description": "Partner specific Azure Application Insights Key. Telemetry from this extension will be published to the relevant Azure resource."
            }];
    }

}