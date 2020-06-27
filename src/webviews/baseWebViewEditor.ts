import * as vscode from 'vscode';

export class BaseWebViewEditor {
    protected _htmlContent : string | undefined;
    protected _extensionContext : vscode.ExtensionContext;
    protected _panel: vscode.WebviewPanel | undefined;
    protected _extensionPath : string;
    protected _documentLoaded : boolean;
    protected _title : string;
    protected _viewColumn: vscode.ViewColumn;
    protected _disposables: vscode.Disposable[] = [];

    constructor(context : vscode.ExtensionContext, title : string | undefined) {
        //initialize variables
        this._documentLoaded = false;
        if (title)
            this._title = title;
        else
            this._title = "";
        this._extensionContext = context;
        this._extensionPath = this._extensionContext.extensionPath;
        this._panel = undefined;
        this._viewColumn = vscode.ViewColumn.Active;
        //load html content
        this._htmlContent = undefined;
    }

    public dispose() {
        // Clean up our resources
        if (this._panel)
            this._panel.dispose();

        while (this._disposables.length) {
            const x = this._disposables.pop();
            if (x) {
                x.dispose();
            }
        }
    }

    public show() {
        this.createWebView();
    }

    public reveal() {
        if (this._panel)
            this._panel.reveal();
    }

    protected getViewType() : string {
        return "BaseWebViewEditor";
    }

    protected getHtmlContentPath() : string {
        return "";
    }

    protected loadHtmlContent() : string {
        //let fullExtensionPath = vscode.Uri.file(this._extensionPath).with({ scheme: 'vscode-resource' });
        let fs = require('fs');
        let filePath = this._extensionContext.asAbsolutePath(this.getHtmlContentPath());
        let content: string = fs.readFileSync(filePath, 'utf8');
        
        //process resources
        let startString = '##EXTENSIONPATH##/';
        let startStringLen = startString.length;
        let pos = content.indexOf(startString);
        while (pos > 0) {
            let endPos = content.indexOf('"', pos + startStringLen);
            if (endPos > 0) {
                let resPathPart = content.substring(pos + startStringLen, endPos);
                let resUri = this._panel!.webview.asWebviewUri(vscode.Uri.file(
                    this._extensionContext.asAbsolutePath(resPathPart))).toString();
                content = content.substr(0, pos) + resUri + content.substr(endPos);  
                
                pos = content.indexOf(startString, pos + resUri.length);
            } else
                pos = -1;
        }

        return content.replace(new RegExp('##CSPSOURCE##', 'g'), this._panel!.webview.cspSource);
    }

    protected getWebviewPanelOptions(): any {
        return {
            // Enable javascript in the webview
            enableScripts: true,
            retainContextWhenHidden: true,
            // And restric the webview to only loading content from our extension's `media` directory.
            localResourceRoots: [
                vscode.Uri.file(this._extensionPath)
            ]
        };
    }

    protected attachToWebView(panel: vscode.WebviewPanel) {
        this._panel = panel;
        this._panel.webview.options = this.getWebviewPanelOptions();
        this.initializeWebView();
    }

    protected createWebView() {
        this._panel = vscode.window.createWebviewPanel(this.getViewType(), this._title, this._viewColumn, 
            this.getWebviewPanelOptions());
        this.initializeWebView();
    }    
    
    protected initializeWebView() {
        if (!this._panel)
            return;

        this.reloadWebViewContent();

        // Listen for when the panel is disposed
        // This happens when the user closes the panel or when the panel is closed programatically
        this._panel.onDidDispose(() => this.dispose(), null, this._disposables);

        // Update the content based on view changes
        this._panel.onDidChangeViewState(e => {
            if ((this._panel) && (this._panel.visible)) {
                this.reloadWebViewContent();
            }
        }, null, this._disposables);

        // Handle messages from the webview
        this._panel.webview.onDidReceiveMessage(message => {
            this.processWebViewMessage(message);
        }, null, this._disposables);

        this._panel.onDidDispose(e => {
            this.onPanelClosed();
        });

    }

    protected onPanelClosed() {       
    }

    protected reloadWebViewContent() {
        this._documentLoaded = false;
        if (this._panel) {
            if (!this._htmlContent)
                this._htmlContent = this.loadHtmlContent();
            this._panel.webview.html = this._htmlContent;
        }
    }

    protected sendMessage(message : any) {
        if (this._panel)
            this._panel.webview.postMessage(message);
    }

    protected processWebViewMessage(message : any) : boolean {
        if (message) {
            switch (message.command) {
                case 'documentLoaded':
                    this._documentLoaded = true;
                    this.onDocumentLoaded();
                    return true;
                case 'showInformation':
                    vscode.window.showInformationMessage(message.message);
                    return true;
                case 'showError':
                    vscode.window.showErrorMessage(message.message);
                    return true;
            }
        }
        
        return false;
    }

    protected onDocumentLoaded() {
    }

    public close() {
        if (this._panel)
            this._panel.dispose();
    }

}