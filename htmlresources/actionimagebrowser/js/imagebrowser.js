class ImageBrowser {

    constructor() {
        let that = this;

        this._vscode = acquireVsCodeApi();

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
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data) {
        //TO-DO: this._data[i].name should be html encoded before adding it to the html content text
        let content = "";
        this._data = data;
        if (this._data) {
            for (let i=0; i<this._data.length; i++) {
                content = content + 
                    '<div class="image"><img class="img" src="' + 
                    this._data[i].content + 
                    '"><div class="name">' + 
                    this._data[i].name + 
                    '</div></div>';
            }
        }
        $("#content").html(content);
    }

}