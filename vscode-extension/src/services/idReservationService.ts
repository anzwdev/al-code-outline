import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALObjectIdNinjaReservationProvider } from '../idreservation/alObjectIdNinjaReservationProvider';
import { IIdReservationProvider } from '../idreservation/IIdReservationProvider';
import { LocalIdReservationProvider } from '../idreservation/LocalIdReservationProvider';
import { DevToolsExtensionService } from "./devToolsExtensionService";

export class IdReservationService extends DevToolsExtensionService {
    private _providers: IIdReservationProvider[];
    private _defaultProvider: IIdReservationProvider;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._providers = [];
        this._defaultProvider = this.registerReservationProvider(new LocalIdReservationProvider(context));
        this.registerReservationProvider(new ALObjectIdNinjaReservationProvider());
    }

    async suggestObjectId(providerName: string | undefined, uri: vscode.Uri | undefined, type: string): Promise<number> {
        let provider = this.getProviderOrDefault(providerName);

        if (!uri) {
            if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
                return 0;
            uri = vscode.workspace.workspaceFolders[0].uri;
        }

        let id = await provider.suggestObjectId(uri, type);
        if (id)
            return id;
        return 0;
    }

    async reserveObjectId(providerName: string | undefined, uri: vscode.Uri | undefined, type: string, id: number): Promise<number> {
        let provider = this.getProviderOrDefault(providerName);

        if (!uri) {
            if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
                return id;
            uri = vscode.workspace.workspaceFolders[0].uri;
        }

        let newId = await provider.reserveObjectId(uri, type, id);
        if (newId)
            return newId;
        return id;
    }

    registerReservationProvider(provider: IIdReservationProvider): IIdReservationProvider {
        this._providers.push(provider);
        return provider;
    }

    getReservationProviders(uri: vscode.Uri | undefined): string[] {
        let names: string[] = [];

        let settingsProviderName = this.getProviderNameFromSettings(uri);
        if (settingsProviderName) {
            let provider = this.getProvider(settingsProviderName);
            if (!provider) {
                vscode.window.showErrorMessage("AL ID Reservation provider " + settingsProviderName + " specified in the alOutline.idReservationProvider setting is not available. Using default local files provider.");    
                settingsProviderName = this._defaultProvider.getName();
            }
            return [ settingsProviderName ];
        }

        for (let i=0; i < this._providers.length; i++)
            if (this._providers[i].isAvailable())
                names.push(this._providers[i].getName());
        return names;
    }

    getDefaultProviderName(): string {
        return this._defaultProvider.getName();
    }

    private getProviderNameFromSettings(uri: vscode.Uri | undefined): string | undefined {
        let settings = vscode.workspace.getConfiguration('alOutline', uri);        
        return settings.get<string>('idReservationProvider');
    }

    private getProviderOrDefault(name: string | undefined): IIdReservationProvider {
        if (name) {
            let provider = this.getProvider(name);
            if ((provider) && (provider.isAvailable()))
                return provider;
            let defaultName = this._defaultProvider.getName();
            vscode.window.showErrorMessage("AL ID Reservation provider " + name + " is not available. Using default " + defaultName + " provider.");
        }
        return this._defaultProvider;
    }

    private getProvider(name: string): IIdReservationProvider | undefined {
        for (let i=0; i < this._providers.length; i++)
            if ((this._providers[i].getName() === name) && (this._providers[i].isAvailable()))
                return this._providers[i];
        return undefined;
    }

}