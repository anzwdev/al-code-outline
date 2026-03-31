import { SettingsWrapper } from "../../core/settingsWrapper";
import { MSALLanguageServerConst } from "./msalLanguageServerConst";

export class MSALLanguageServerSettings extends SettingsWrapper {

    constructor(uri: any) {
        super(MSALLanguageServerConst.cfgConfigurationSection, uri);
    }

    public getPackageCachePath() : string[] | undefined {
        let alPackages: string[] | undefined = undefined;
        let alPackagesAllTypes: string | string[] | undefined = this.configuration.get(MSALLanguageServerConst.cfgPackageCachePath);
        if ((alPackagesAllTypes) && (typeof(alPackagesAllTypes) === 'string')) {
            let alPackagesStr = alPackagesAllTypes as string;
            alPackages = [alPackagesStr];
        } else {
            alPackages = alPackagesAllTypes as string[] | undefined;
        }
        return alPackages;
    }



}