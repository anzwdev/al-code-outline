import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { JsonFormEditor } from "./jsonFormEditor";

export class AppJsonEditor extends JsonFormEditor {
    
    constructor(devToolsContext : DevToolsExtensionContext) {
        super(devToolsContext, "app.json");

        this.fieldsOrder = ["id", "name", "description", "publisher", "brief", "version",
            "privacyStatement", "propagateDependencies", "dependencies", "internalsVisibleTo",
            "EULA", "help", "url", "logo", "screenshots", "platform", "application", "test",
            "target", "idRanges", "idRange", "features", "showMyCode", "runtime",
            "contextSensitiveHelpUrl", "helpBaseUrl", "supportedLocales", "applicationInsightsKey"];
    }

    protected getViewType() : string {
        return 'azALDevTools.AppJsonEditor';
    }

    protected afterReadRefList(refList: any) {
        if (refList) {
            for (let i = 0; i<refList.length; i++) {
                if (refList[i].appId) {
                    refList[i].id = refList[i].appId;
                    refList[i].appId = undefined;
                }
            }
        }
    }

    protected beforeSaveRefList(refList: any) {
        if (refList) {
            for (let i = 0; i<refList.length; i++) {
                refList[i].appId = refList[i].id;
                refList[i].id = undefined;
            }
        }
    }

    protected getDocumentData(): any {
        let data = super.getDocumentData();

        if (data) {
            this.afterReadRefList(data.dependencies);
            this.afterReadRefList(data.internalsVisibleTo);

            if (data.idRange) {
                if (!data.idRanges)
                    data.idRanges = [];
                data.idRanges.push(data.idRange);
                data.idRange = undefined;
            }
        }

        return data;
    }

    protected onDataChanged(data: any) {
        //check version
        if ((data) && (data.runtime)) {
            let runtime: string = data.runtime;
            let pos = runtime.indexOf(".");
            if (pos > 0) {
                runtime = runtime.substr(0, pos);
                let runtimeNo = parseInt(runtime);
                if (!isNaN(runtimeNo)) { 
                    if (runtimeNo < 5) {
                        this.beforeSaveRefList(data.dependencies);
                        this.beforeSaveRefList(data.internalsVisibleTo)
                    };
                    if ((runtimeNo < 3) && (data.idRanges) && (data.idRanges.length <= 1)) {
                        if (data.idRanges.length > 0)
                            data.idRange = data.idRanges[0];
                        data.idRanges = undefined; 
                    }
                }
            }
        }

        super.onDataChanged(data);
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
                "type": "longstring",
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
                "type": "longstring",
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
                "caption": "Platform",
                "type": "group"
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
                "autocomplete": [
                    "Extension",
                    "Internal",
                    "Cloud",
                    "OnPrem"
                ],
                "description": "Compilation target"
            },
            {
                "name": "runtime",
                "caption": "Runtime",
                "type": "string",
                "autocomplete": [
                    { "value": "9.2", "description": "Business Central 2022 release wave 1 update 20.4" },
                    { "value": "9.1", "description": "Business Central 2022 release wave 1 update 20.1" },
                    { "value": "9.0", "description": "Business Central 2022 release wave 1" },
                    { "value": "8.1", "description": "Business Central 2021 release wave 2 update 19.1" },
                    { "value": "8.0", "description": "Business Central 2021 release wave 2" },
                    { "value": "7.2", "description": "Business Central 2021 release wave 1 update 18.2" },
                    { "value": "7.1", "description": "Business Central 2021 release wave 1 update 18.1" },
                    { "value": "7.0", "description": "Business Central 2021 release wave 1" },
                    { "value": "6.5", "description": "Business Central 2020 release wave 2 update 17.5" },
                    { "value": "6.4", "description": "Business Central 2020 release wave 2 update 17.4" },
                    { "value": "6.3", "description": "Business Central 2020 release wave 2 update 17.3" },
                    { "value": "6.2", "description": "Business Central 2020 release wave 2 update 17.2" },
                    { "value": "6.1", "description": "Business Central 2020 release wave 2 update 17.1" },
                    { "value": "6.0", "description": "Business Central 2020 release wave 2" },
                    { "value": "5.4", "description": "Business Central 2020 release wave 1 update 16.4" },
                    { "value": "5.3", "description": "Business Central 2020 release wave 1 update 16.3" },
                    { "value": "5.2", "description": "Business Central 2020 release wave 1 update 16.2" },
                    { "value": "5.1", "description": "Business Central 2020 release wave 1 update 16.1" },
                    { "value": "5.0", "description": "Business Central 2020 release wave 1" },
                    { "value": "4.4", "description": "Business Central 2019 release wave 2 update 15.4" },
                    { "value": "4.3", "description": "Business Central 2019 release wave 2 update 15.3" },
                    { "value": "4.2", "description": "Business Central 2019 release wave 2 update 15.2" },
                    { "value": "4.1", "description": "Business Central 2019 release wave 2 update 15.1" },
                    { "value": "4.0", "description": "Business Central 2019 release wave 2" },
                    { "value": "3.0", "description": "Business Central Spring '19 Release" },
                    { "value": "2.0", "description": "Business Central Fall '18 Release" },
                    { "value": "1.0", "description": "Business Central Spring '18 Release" }
                                    ]
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
                "name": "propagateDependencies",
                "caption": "Propagate Dependencies",
                "type": "boolean",
                "description": "Specifies whether the dependencies of this project should be propagated as direct dependencies of projects that depend on this one."
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
                "caption": "Compilation",
                "type": "group"

            },
            {
                "name": "preprocessorSymbols",
                "caption": "Preprocessor Symbols",
                "type": "list",
                "description": "Define symbols that can be used in all source files"
            },
            {
                "name": "suppressWarnings",
                "caption": "Suppress Warnings",
                "type": "list",
                "description": "List of warning ids that the compiler should suppress. E.g. AL0604 or AA0215"
            },
            {
                "name": "features",
                "caption": "Features",
                "type": "list",
                "autocomplete": [
                    {
                        value: "TranslationFile",
                        description: "Generate and utilize translation files in xliff format"
                    },
                    {
                        value: "GenerateCaptions",
                        description: "Generate translation entries for captions for all application objects"
                    },
                    {
                        value: "ExcludeGeneratedTranslations",
                        description: "Exclude the generated translation file from the app"
                    },
                    {
                        value: "NoImplicitWith",
                        description: "Switch off the option of using implicit withs."
                    },
                    {
                        value: "GenerateLockedTranslations",
                        description: "Generate translation entries in the template translation file for locked labels."
                    }
                ],
                "description": "Optional/experimental compiler features can be enabled by specifying them."
            },
            {
                "name": "showMyCode",
                "caption": "Show My Code",
                "type": "boolean",
                "description": "Allows the code to be debugged from other extensions when it has been published. The default setting is false."
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
                "description": "Relative paths to screenshot files that should be contained in the app package"
            },
            {
                "name": "idRanges",
                "caption": "Id Ranges",
                "description": "An optional set of ranges for application object IDs. For all objects outside the range, a compilation error will be raised.",
                "type": "array",
                "inline": true,
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
                "type": "list"
            },
            {
                "name": "applicationInsightsKey",
                "caption": "Application Insights Key",
                "type": "string",
                "description": "Partner specific Azure Application Insights Key. Telemetry from this extension will be published to the relevant Azure resource."
            },
            {
                "name": "applicationInsightsConnectionString",
                "caption": "Application Insights Connection String",
                "type": "string",
                "description": "Partner specific Azure Application Insights connection string. Telemetry from this extension will be published to the relevant Azure resource."
            },
            {
                "name": "keyVaultUrls",
                "caption": "Key Vault Urls",
                "type": "list",
                "description": "The URLs of up to two Azure Key Vaults in the format https://<keyvaultname>.vault.azure.net. AL code in this extension will be able to read secrets in these key vaults. Specify a second key vault to be resilient in case the first key vault is unavailable."
            }
        
        ];
    }

}