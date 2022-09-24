import * as vscode from 'vscode';
import { CARulesCollection } from '../carulesviewer/caRulesCollection';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { SyntaxProvider } from "./syntaxProvider";

export class AppJsonSyntaxProvider extends SyntaxProvider {
    constructor(newContext: DevToolsExtensionContext) {
        super(newContext, "/appJsonSyntax.json");
    }

    async provideTextDocumentContent(uri: vscode.Uri, token: vscode.CancellationToken): Promise<string> {
        let rulesCollection = new CARulesCollection(this._context);
        await rulesCollection.loadRules();
        if ((rulesCollection.rules) && (rulesCollection.rules.length > 0)) {
            let ruleEnum: string[] = [];
            let ruleEnumDesc: string[] = [];

            let oneOf: any[] = [];

            for (let i=0; i<rulesCollection.rules.length; i++) {
                var rule = rulesCollection.rules[i];
                if (rule.id) {
                    ruleEnum.push(rule.id);
                    if (rule.description)
                        ruleEnumDesc.push(rule.description);
                    else
                        ruleEnumDesc.push(rule.id);
                }
            }

            let syntax = {
                "$id": "AL Language Project File Syntax",
                "$schema": "http://json-schema.org/draft-07/schema",
                "description": "schema for an AL Language Project File",
                "type": "object",
                "properties": {
                    "suppressWarnings": {
                        "type": "array",
                        "items": {
                            "type": "string",
                            "enum": ruleEnum,
                            "enumDescriptions": ruleEnumDesc           
                        }
                    }
                }
            };

            return JSON.stringify(syntax);
        }

        return "";
    }


}