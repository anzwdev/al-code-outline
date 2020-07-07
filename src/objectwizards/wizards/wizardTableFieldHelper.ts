import { ALTableWizardFieldData } from "./alTableWizardFieldData";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { Uri } from "vscode";

export class WizardTableFieldHelper {
    public static async getAllFieldTypes(extensionContext: DevToolsExtensionContext, resourceUri: Uri | undefined): Promise<string[]> {
        let types: string[] = ['Blob', 'Boolean', 'Code', 'Date', 'DateFormula', 'DateTime', 'Decimal', 'Duration',
            'Guid', 'Integer', 'Media', 'MediaSet', 'Option', 'RecordId', 'TableFilter',
            'Text', 'Time'];

        let enumList: string[] = await extensionContext.alLangProxy.getEnumList(resourceUri);
        if (enumList.length > 0) {
            for (let i = 0; i < enumList.length; i++) {
                types.push('Enum ' + enumList[i]);
            }
        }
        else {
            types.push('Enum');
        }
            
        return types;
    }

    public static validateFields(data : any) : ALTableWizardFieldData[] {
        let fields : ALTableWizardFieldData[] = [];
    
        if ((data) && (data.length > 0)) {
            for (let i = 0; i < data.length; i++) {
                fields.push(new ALTableWizardFieldData(data[i].id, data[i].name, data[i].dataType, data[i].length, data[i].dataClassification));
            }
        }
    
        return fields;
    }
}
