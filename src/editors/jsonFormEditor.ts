import { FormEditor } from "./formEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class JsonFormEditor extends FormEditor {
    
    constructor(devToolsContext : DevToolsExtensionContext, title : string | undefined) {
        super(devToolsContext, title);
    }

    protected getViewType() : string {
        return 'azALDevTools.JsonFormEditor';
    }

    protected onDataChanged(data: any) {
        if (!data)
            data = {};

        //sort fields
        if (this._currentFields) {
            let sorted: any = {};

            //add fields from the definition
            for (let i=0; i < this._currentFields.length; i++) {
                let name: string = this._currentFields[i].name; 
                let val = data[name];
                if (val)
                    sorted[name] = val;
            }
            
            //add fields that are not in the current fields definition


            data = sorted;
        }

        this.updateTextDocumentFromJson(data);        
    }

    protected getDocumentData() {
        return this.getTextDocumentAsJson(false);
    }

}