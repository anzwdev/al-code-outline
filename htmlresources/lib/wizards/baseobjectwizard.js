class BaseObjectWizard {

    constructor(maxStepNo) {        
        this._step = 1;
        this._maxStepNo = maxStepNo;
        this._vscode = acquireVsCodeApi();
     
        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('prevBtn').addEventListener('click', event => {
            this.onPrev();
        });

        document.getElementById('nextBtn').addEventListener('click', event => {
            this.onNext();
        });

        document.getElementById('finishBtn').addEventListener('click', event => {
            this.onFinish();
        });

        document.getElementById('cancelBtn').addEventListener('click', event => {
            this.onCancel();
        });      

        this.initSteps();
        this.updateMainButtons();
        this.initNameLenUpdate();
        this.initIdProviders();

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    initSteps() {
        let noOfSteps = this._maxStepNo + 10;
        for (let i=2; i<=noOfSteps; i++) {
            let stepctl = document.getElementById("wizardstep" + i.toString());
            if (stepctl)
                htmlHelper.hide(stepctl);
            else
                return;
        }
    }

    updateMainButtons() {
        document.getElementById("prevBtn").disabled = (this._step <= 1);
        document.getElementById("nextBtn").disabled = (this._step >= this._maxStepNo);
    }

    updateControls() {
        this.updateMainButtons();
    }

    setStep(newStep) {
        htmlHelper.hideById("wizardstep" + this._step.toString());        
        this._step = newStep;
        htmlHelper.showById("wizardstep" + this._step.toString());
        this.updateMainButtons();
    }   

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data);
                break;
            case 'setIdProvider':
                this.setIdProvider(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);
    }

    setData(data) {   
        this._data = data;
        this.updateIdProviders();
    }

    onFinish() {
        this.collectStepData(true);
        if (this.canFinish())
            this.sendFinishMessage();
    }

    sendFinishMessage() {
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        })
    }

    onPrev() {
        if (this._step > 1) {
            this.collectStepData(false);
            this.setStep(this._step - 1);
        }
    }

    onNext() {
        if (this._step < this._maxStepNo) {
            this.collectStepData(false);
            this.setStep(this._step + 1);
        }
    }

    collectStepData(finishSelected) {
    }

    canFinish() {
        if ((!this._data.objectName) || (this._data.objectName == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter object name.'
            });
            return false;
        }
        return true;
    }  

    initNameLenUpdate() {
        this._ctName = document.getElementById('objectname');
        this._ctNameLen = document.getElementById('objectnamelen');
        if ((this._ctName) && (this._ctNameLen)) {
            this.updateNameLen();
            this._ctName.addEventListener('input', event => {
                this.updateNameLen();
            });   
        }
    }

    updateNameLen() {
        this._ctNameLen.innerText = this._ctName.value.length.toString();
    }

    initIdProviders() {
        let idControls = document.getElementById("idproviderline");
        if (idControls) {
            htmlHelper.hideById("idproviderline");
            document.getElementById('idprovider').addEventListener('change', event => {
                this.onIdProviderChanged();
            });
        }
    }

    updateIdProviders() {
        let idprovctl = document.getElementById("idprovider");
        if (idprovctl) {
            let visible = ((this._data) && (this._data.idResProviders) && (this._data.idResProviders.length > 0));
            let editable = ((!!this._data) && (!!this._data.idResProviders) && (this._data.idResProviders.length > 1));
            if (visible) {
                htmlHelper.showById("idproviderline");
                
                if ((this._data) && (this._data.idResProviders)) {
                    htmlHelper.setElementOptions(idprovctl, this._data.idResProviders, false);
                    idprovctl.value = this._data.idResProviderName;
                    idprovctl.disabled = !editable;
                }
            } else
                htmlHelper.hideById("idproviderline");
        }
    }

    onIdProviderChanged() {
        let name = document.getElementById("idprovider").value;
        if (name)
            this.sendMessage({
                command: "idProviderChanged",
                data: {
                    idResProviderName: name
                }});
    }

    setIdProvider(data) {
        if ((data) && (this._data)) {
            let providerChanged = (this._data.idResProviderName !== data.idResProviderName);

            this._data.idResProviderName = data.idResProviderName;
            this._data.objectId = data.objectId;
            
            if ((providerChanged) && (this._data.idResProviderName))
                document.getElementById("idprovider").value = this._data.idResProviderName;
            if (this._data.objectId)
                document.getElementById("objectid").value = this._data.objectId;
            
        }
    }

}