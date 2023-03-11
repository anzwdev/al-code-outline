class SymbolsTreeView {

    constructor() {
        var me = this;

        this._vscode = acquireVsCodeApi();
        this._symTree = new SymbolsTreeControl('symbols', undefined, false);
        this._symTree.sortNodes = false;
        this._symTree.resetCollapsedState = false;
        this._symTree.enableSimpleFilter('filter', 'filterbtn');
        this._symTree.nodeDefaultAction = function(node) {
            me.onNodeDefaultAction(node);
        }

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

        this.initializeContextMenu();

        // Handle messages sent from the extension to the webview
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

    initializeContextMenu() {
        let me = this;

        $('#symbols').contextMenu({
            selector: '.shead', 
            callback: function(key, options) {
                if (key === "expandcollapse") {
                    me._symTree.toggleChildNodes(this);
                }
                else {
                    me.sendMessage({
                        command : key,
                        path : me._symTree.getNodePath(this),
                        selpaths : me._symTree.getSelectedPaths(this),
                        uid : $(this).data('uid'),
                        kind : $(this).data('kind')
                    });
                }
            },
            items: {
                "definition": {name: "Go to definition"},
                "expandcollapse": {name: "Expand/collapse child nodes"},
            }
        });
    }

    onNodeDefaultAction(node) {
        if (node)
            this.sendMessage({
                command : 'definition',
                path : this._symTree.getNodePath(node),
                selpaths : this._symTree.getSelectedPaths(node),
                uid : $(node).data('uid'),
                kind : $(node).data('kind')
            });
    }

}