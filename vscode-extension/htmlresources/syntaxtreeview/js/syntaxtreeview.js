class SyntaxTreeView {
    constructor() {
        this._vscode = acquireVsCodeApi();

        //this._propView = new PropertyViewer('prop');
        this._propView = new AZGridView('prop', [
            {name:'name', caption:'Name', style: 'width:45%;'},
            {name:'value', caption:'Value', style: 'width:55%' }
        ]);

        this._symTree = new SymbolsTreeControl('symbols', undefined, false);
        this._symTree.sortNodes = false;
        this._symTree.resetCollapsedState = false;
        this._symTree.setShowIds(false);
        this._symTree.syntaxTreeMode = true;
        this._symTree.showIcons = false;

        this._symTree.nodeSelected = (node => {
            this.onNodeSelected(node);
        });
        
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
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });
        
        document.getElementById('syncbtn').addEventListener('click', event => {
            this.sendMessage({
                command: 'sync'
            });   
        });

        //initialize splitter
        Split(['#symbols', '#proppanel'], {
            minSize: 0,
            gutter: function (index, direction) {
                var gutter = document.createElement('div')
                gutter.className = 'gutter gutter-' + direction
                return gutter
            },
            gutterSize: 6,
            direction: 'vertical'
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
            case 'setSymbolInfo':
                this.setSymbolInfo(message.data);
                break;
            case 'selectSymbol':
                if (message.selected)
                    this._symTree.selectNodeByPath(message.selected);               
                break;
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

    setSymbolInfo(data) {
        if (data) {
            this._propView.setData(data.properties);
            document.getElementById('stype').innerText = "Type " + data.type;
            document.getElementById('skind').innerText = "Kind " + data.name;
        } else {
            document.getElementById('stype').innerText = "Type";
            document.getElementById('skind').innerText = "Kind";
        }
    }

    onNodeSelected(node) {
        this.sendMessage({
            command: 'symbolselected',
            path: this._symTree.getNodePath(node)
        });
    }

}