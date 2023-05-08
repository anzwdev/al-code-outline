import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardData } from "./alObjectWizardData";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALObjectWizard {
    description: string;
    detail: string;
    label: string;
    protected _toolsExtensionContext : DevToolsExtensionContext;
    onInitWizardData: (wizardData: ALObjectWizardData) => void = () => {};
    
    constructor(newToolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        this._toolsExtensionContext = newToolsExtensionContext;
        this.label = newLabel;
        this.description = newDescription;
        this.detail = newDetails;
    }

    run(settings: ALObjectWizardSettings) {
    }

    async initObjectIdFieldsAsync(data: ALObjectWizardData, settings: ALObjectWizardSettings, type: string) {
        let uri = settings.getDestDirectoryUri();
        let idProviders = this._toolsExtensionContext.idReservationService.getReservationProviders(uri); 
        let idProviderName = ((idProviders) && (idProviders.length === 1))? idProviders[0] : this._toolsExtensionContext.idReservationService.getDefaultProviderName();
        let objectId : number = await this._toolsExtensionContext.idReservationService.suggestObjectId(idProviderName, 
            settings.getDestDirectoryUri(), type);

        data.uri = uri;
        data.objectId = objectId.toString();
        data.idResProviders = idProviders;
        data.idResProviderName = idProviderName;
        data.idResObjectType = type;
    }


} 