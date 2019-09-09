class SymbolsTreeView {

    constructor() {
        this._vscode = acquireVsCodeApi();
        this._symTree = new SymbolsTreeControl('symbols', undefined, false);
        this._symTree.sortNodes = false;

        //prevent standard Ctrl+A inside tree elements
        $('body').on('keydown', '.symbolscont', function(evt)
        {
            if ((!evt.ctrlKey) || (evt.keyCode !== 65))
                return;
            evt.preventDefault();
            return false;
        });

        // Prevent text select from shift + click inside tree elements 
        $('body').on('mousedown', '.symbolscont', function(evt)
        {
            if (evt.shiftKey || evt.ctrlKey) {
                evt.preventDefault();
            }
        });

        // Handle messages sent from the extension to the webview
        var me = this;
        window.addEventListener('message', event => {
            me.onMessage(event.data);
        });

        this.sendMessage({
            command: 'documentLoaded'
        });       
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this._symTree.setData(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

}