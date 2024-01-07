import * as vscode from 'vscode';
import { ICRSExtensionPublicApi } from './CRSExtensionPublicApiInterfaces';

export class CRSALLangExtHelper {
    private static _crsALLangExtApi : ICRSExtensionPublicApi | undefined = undefined;

    public static async GetCrsAlLangExt() : Promise<ICRSExtensionPublicApi | undefined> {
        if (!this._crsALLangExtApi) {
            let crsExtension = vscode.extensions.getExtension('waldo.crs-al-language-extension');            
            if (crsExtension)
            {
                if (crsExtension.isActive)
                    this._crsALLangExtApi = crsExtension.exports;
                else
                    this._crsALLangExtApi = await crsExtension.activate();
            }
            //else
            //throw exception here
        }
        return this._crsALLangExtApi;
    }


}