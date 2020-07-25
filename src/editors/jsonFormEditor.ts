import { FormEditor } from "./formEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class JsonFormEditor extends FormEditor {
    fieldsOrder: string[] | undefined;
    undefFields: string[] | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, title : string | undefined) {
        super(devToolsContext, title);
        this.fieldsOrder = undefined;
        this.undefFields = undefined;
    }

    protected getViewType() : string {
        return 'azALDevTools.JsonFormEditor';
    }

    protected onDataChanged(data: any) {
        if (!data)
            data = {};
        
        //sort fields
        if (this.fieldsOrder) {
            let sorted: any = {};
            
            //add fields from the definition
            for (let i=0; i < this.fieldsOrder.length; i++) {
                let val = data[this.fieldsOrder[i]];
                if (val)
                    sorted[this.fieldsOrder[i]] = val;
            }

            //add fields that are not in the current fields definition
            if (this.undefFields) {
                for (let i=0; i < this.undefFields.length; i++) {
                    let val = data[this.undefFields[i]];
                    if (val)
                        sorted[this.undefFields[i]] = val;
                }
            }

            data = sorted;
        }

        this.updateTextDocumentFromJson(data);        
    }

    protected onBeforeDataSave(data: any) {
    }

    protected getDocumentData(): any {
        let data = this.getTextDocumentAsJson();
        //store names of unknown fields

        this.undefFields = undefined;
        if ((this.fieldsOrder) && (data)) {
            this.undefFields = [];
            for(let key in data) {
                if (this.fieldsOrder.indexOf(key) < 0)
                    this.undefFields.push(key);
            }
        }

        return data;
    }

}