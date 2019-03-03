import * as vscode from 'vscode';
import * as crsALExt from 'crs-al-language-extension-api';

export class CRSALLangExtHelper {
    private static _crsALLangExtApi : crsALExt.ICRSExtensionPublicApi | undefined = undefined;

    public static async GetCrsAlLangExt() : Promise<crsALExt.ICRSExtensionPublicApi | undefined> {
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