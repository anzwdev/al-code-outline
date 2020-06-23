class FormEditor {

    constructor() {
        this._vscode = acquireVsCodeApi();
        this._form = new AZFormView("main", undefined);
        this._form.fieldChanged = ((idx) => this.onFieldChanged(idx));

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                if (message.fields)
                    this._form.setFields(message.fields);
                this._form.setData(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    onFieldChanged(idx) {
        this.sendMessage({
            command: 'dataChanged',
            data: this._form.getData()
        });
    }

}

var editor;

window.onload = function() {
    editor = new FormEditor();
};
