import { LSConnector } from "../lsConnector";
import { LSModuleClient } from "../lsModuleClient";
import { LSConfigurationChangeRequest } from "./lsConfigurationChangeRequest";
import { LSDocumentChangeRequest } from "./lsDocumentChangeRequest";
import { LSDocumentContentChangeRequest } from "./lsDocumentContentChangeRequest";
import { LSDocumentContentChangeResponse } from "./lsDocumentContentChangeResponse";
import { LSFilesRenameRequest } from "./lsFilesRenameRequest";
import { LSFilesRequest } from "./lsFilesRequest";
import { LSFileSystemFileChangeRequest } from "./lsFileSystemFileChangeRequest";
import { LSWorkspaceFoldersChangeRequest } from "./lsWorkspaceFoldersChangeRequest";

export class LSWorkspaceChangeTrackingClient extends LSModuleClient {
    
    constructor(connector: LSConnector) {
        super(connector);
    }
  
    //workspace and file notifications

    public workspaceFolderChange(params: LSWorkspaceFoldersChangeRequest) {
        this.connector.sendNotification(params, 'ws/workspaceFoldersChange');
    }

    public async documentOpen(params: LSDocumentChangeRequest) {
        this.connector.sendNotification(params, "ws/documentOpen");
    }

    public async documentChange(params: LSDocumentContentChangeRequest) : Promise<LSDocumentContentChangeResponse | undefined> {
        return this.connector.sendRequest<LSDocumentContentChangeRequest, LSDocumentContentChangeResponse>(params, "ws/documentContentChange");
    }

    public async documentSave(params: LSDocumentChangeRequest) {
        this.connector.sendNotification(params, "ws/documentSave");
    }

    public documentClose(params: LSDocumentChangeRequest) {
        this.connector.sendNotification(params, "ws/documentClose");
    }

    public fileCreate(params: LSFilesRequest) {
        this.connector.sendNotification(params, "ws/fileCreate");
    }

    public fileDelete(params: LSFilesRequest) {
        this.connector.sendNotification(params, "ws/fileDelete");
    }

    public fileRename(params: LSFilesRenameRequest) {
        this.connector.sendNotification(params, "ws/fileRename");
    }

    public async fileSystemFileChange(params: LSFileSystemFileChangeRequest) {
        this.connector.sendNotification(params, "ws/fsFileChange");
    }

    public async fileSystemFileCreate(params: LSFileSystemFileChangeRequest) {
        this.connector.sendNotification(params, "ws/fsFileCreate");
    }

    public async fileSystemFileDelete(params: LSFileSystemFileChangeRequest) {
        this.connector.sendNotification(params, "ws/fsFileDelete");
    }

    public async configurationChange(params: LSConfigurationChangeRequest) {
        this.connector.sendNotification(params, "ws/configurationChange");
    }




}   