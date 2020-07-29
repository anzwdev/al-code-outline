class FormEditor {

    constructor() {
        this._vscode = acquireVsCodeApi();
        this._form = new AZFormView("main", undefined);
        this._form.fieldChanged = ((idx) => this.onFieldChanged(idx));
        this._hasError = false;

        this._errorMessage = document.createElement("div");
        this._errorMessage.className = 'errorPanel';
        htmlHelper.hide(this._errorMessage);
        document.getElementById("main").appendChild(this._errorMessage);

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    setError(hasError, message) {
        if (this._hasError != hasError) {
            this._hasError = hasError;
            if (this._hasError) {
                this._form.hide();
                this._errorMessage.innerText = message;
                htmlHelper.show(this._errorMessage);
            } else {
                htmlHelper.hide(this._errorMessage);
                this._errorMessage.innerText = "";
                this._form.show();
            }
        } else if (this._hasError) {
            this._errorMessage.innerText = message;
        }
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setError(false, undefined);
                if (message.fields)
                    this._form.setFields(message.fields);
                this._form.setData(message.data);
                break;
            case 'setAutocomplete':
                if ((message.path) && (message.data))
                    this._form.setAutocomplete(message.path, message.data);
                break;
            case 'dataError':
                if (message.fields)
                    this._form.setFields(message.fields);
                this.setError(true, message.message);                
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
