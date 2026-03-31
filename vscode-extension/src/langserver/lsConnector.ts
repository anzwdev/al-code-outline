import * as vscode from 'vscode';
import * as cp from 'child_process';
import * as rpc from 'vscode-jsonrpc/node';

export class LSConnector implements vscode.Disposable {
    _context : vscode.ExtensionContext;
    _childProcess : cp.ChildProcess | undefined;
    _connection : rpc.MessageConnection | undefined;
    errorLogUri : vscode.Uri | undefined;

    constructor(context : vscode.ExtensionContext) {
        this._context = context;
        this._childProcess = undefined;
        this._connection = undefined;
        this.errorLogUri = undefined;
        this.initialize();
    }

    dispose() {
        if (this._connection) {
            this.exit();
            this._connection.dispose();
            this._connection = undefined;
        }
    }

    protected initialize() {
        try {
            let os = require('os');
            let platform = os.platform();
            let langServerPath : string;

            //find binaries path
            langServerPath = this._context.asAbsolutePath("bin/" + platform + "/AnZwDev.AL.LanguageServer.Runner");
            this.errorLogUri = vscode.Uri.file(this._context.asAbsolutePath("bin/" + platform + "/log.txt"));
            if ((platform === "darwin") || (platform === "linux")) {
                let fs = require('fs');
                fs.chmodSync(langServerPath, 0o755);
            } else {
                langServerPath += ".exe";
            }

            //start child process
            this._childProcess = cp.spawn(langServerPath, []);
            if (this._childProcess) {
                let stdOutStream = this._childProcess.stdout;
                let stdInStream = this._childProcess.stdin;
                if ((stdOutStream !== null) && (stdInStream !== null)) {
                    this._connection = rpc.createMessageConnection(
                        new rpc.StreamMessageReader(stdOutStream),
                        new rpc.StreamMessageWriter(stdInStream));
                    this._connection.listen();
                }
            }
        }
        catch (e) {
        }
    }

    //exit
    public exit() {
        this.sendNotification({}, 'exit');
    }

    //communication methods

    sendNotification<T>(params: T, command: string) {
        try {
            if (!this._connection) {
                return undefined;
            }

            let reqType = new rpc.NotificationType<T>(command);
            this._connection.sendNotification(reqType, params);
        }
        catch (e) {
        }
    }

    async sendRequest<Req, Res>(params: Req, command: string) : Promise<Res | undefined> {
        try {
            if (!this._connection) {
                return undefined;
            }
            let reqType = new rpc.RequestType<Req, Res, void>(command);
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch(e) {
            return undefined;
        }
    }

    public isEnabled() : boolean {
        return !!this._connection;        
    }

}
