import * as vscode from 'vscode';
import { DevToolsExtensionService } from './devToolsExtensionService';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { API, GitExtension, Repository } from '../types/git';

export class GitClientService extends DevToolsExtensionService {
    private _gitApi: API | undefined;

    constructor(newContext: DevToolsExtensionContext) {
        super(newContext);
    }

    public async getUncommitedFiles(uri: vscode.Uri | undefined): Promise<string[] | undefined> {
        await this.loadGitExtension();
        
        if ((this._gitApi?.repositories) && (this._gitApi.repositories.length > 0)) {          
            let repository : Repository | undefined = undefined;
            if (uri) {
                let uriPath = uri.fsPath.toLowerCase();
                repository = this._gitApi.repositories.find(repo => uriPath.startsWith(repo.rootUri.fsPath.toLowerCase()));
            } else {
                repository = this._gitApi.repositories[0];
            }

            if (repository) {
                const changes = repository.state.workingTreeChanges;
                return changes.map((change) => change.uri.fsPath);
            }
        }

        return [];
    }

    private async loadGitExtension() {
        if (!this._gitApi) {
            const gitExtension = vscode.extensions.getExtension<GitExtension>('vscode.git');
            if (gitExtension) {
                if (!gitExtension.isActive) {
                    await gitExtension.activate();
                }
                const gitExtensionExports = gitExtension.exports;
                this._gitApi = gitExtensionExports.getAPI(1);
            }
        }
    }

}