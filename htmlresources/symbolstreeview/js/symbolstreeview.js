class SymbolsTreeView {

    constructor() {
        this._vscode = acquireVsCodeApi();
        this._symTree = new SymbolsTreeControl('symbols', undefined, false);
        this._symTree.sortNodes = false;
        this._symTree.resetCollapsedState = false;
        this._symTree.enableSimpleFilter('filter', 'filterbtn');

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
                this.setData(message.data);
                if (message.selected)
                    this._symTree.selectNodeByPath(message.selected);               
                break;
            case 'selectSymbol':
                if (message.selected)
                    this._symTree.selectNodeByPath(message.selected);               
                break
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data) {
        if (data)
            this._symTree.applyTreeItemState(data);
        this._symTree.setData(data);
    }

}