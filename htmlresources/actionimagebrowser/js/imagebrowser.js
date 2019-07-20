class ImageBrowser {

    constructor() {
        let that = this;

        this._vscode = acquireVsCodeApi();
        this._controlId = 'content';

        this.initializeContextMenu();

        // Handle messages sent from the extension to the webview
        var me = this;
        window.addEventListener('message', event => {
            me.onMessage(event.data);
        });

        document.getElementById('searchname').addEventListener('keydown', event => {
            me.onSearchKeyDown(event);
        });

        document.getElementById('searchbtn').addEventListener('click', event => {
            me.search();
        });

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    initializeContextMenu() {
        let browser = this;
        $('#content').contextMenu({
            selector: '.image', 
            callback: function(key, options) {
                let idx = $(this).data('idx');                
                let name = browser._data[idx].name;
                browser.sendMessage({
                    command : key,
                    name: name
                });
            },
            items: {
                "copyname": {name: "Copy"},
                "copyaction": {name: "Copy as action"},
                "copypromotedaction": {name: "Copy as promoted action"}
            }
        });
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data) {
        this._data = data;
        this.renderData();
    }

    renderData() {
        let element = document.getElementById(this._controlId);
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }

        if (this._data) {
            for (let i=0; i<this._data.length; i++) {
                if (this.validItem(this._data[i])) {
                    let item = document.createElement('div');
                    item.className = 'image';
                    item.dataset['idx'] = i;

                    let img = document.createElement('img');
                    img.src = this._data[i].content;
                    item.appendChild(img);

                    let label = document.createElement('div');
                    label.className = 'name';
                    label.innerText = this._data[i].name;
                    item.appendChild(label);

                    element.appendChild(item);
                }
            }
        }

    }

    compileFilters() {
        let nameFilterText = document.getElementById('searchname').value;
        if (nameFilterText)
            this._nameFilter = compileFilter('text', nameFilterText);
        else
            this._nameFilter = undefined;
    }

    validItem(item) {
        if ((this._nameFilter) && (!this._nameFilter({TEXT: item.name})))
            return false;
        return true;
    }

    search() {
        this.compileFilters();
        this.renderData();
    }

    onSearchKeyDown(e) {
        let handled = false;

        switch (e.which) {
            case 13:
                this.search();
                handled = true;
                break;
        }

        if (handled) {
            e.preventDefault();
            return false;
        }
    }

}