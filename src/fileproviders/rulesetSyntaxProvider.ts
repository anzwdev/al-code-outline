import * as vscode from 'vscode';
import { CARulesCollection } from '../carulesviewer/caRulesCollection';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxProvider } from "./syntaxProvider";

export class RulesetSyntaxProvider extends SyntaxProvider {
    constructor(newContext: DevToolsExtensionContext) {
        super(newContext, "/ruleSetSyntax.json");
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
                "$id": "AL Rule Set File Syntax",
                "$schema": "http://json-schema.org/draft-07/schema",
                "description": "Schema for a file containing a rule set.",
                "type": "object",
                "properties": {
                    "rules": {
                        "type": "array",
                        "items": {
                            "type": "object",
                            "properties": {
                                "id": {
                                    "enum": ruleEnum,
                                    "enumDescriptions": ruleEnumDesc
                                }
                            }
                        }
                    }
                }
            };

            return JSON.stringify(syntax);
        }

        return "";
    }

}