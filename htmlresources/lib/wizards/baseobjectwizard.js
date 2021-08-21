class BaseObjectWizard {

    constructor(maxStepNo) {        
        this._step = 1;
        this._maxStepNo = maxStepNo;
        this._vscode = acquireVsCodeApi();
        this.initSteps();
        this.updateMainButtons();
     
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

        this.sendMessage({
            command: 'documentLoaded'
        });   
    }

    initSteps() {
        for (let i=2; i<=this._maxStepNo; i++) {
            htmlHelper.hideById("wizardstep" + i.toString());
        }
    }

    updateMainButtons() {
        document.getElementById("prevBtn").disabled = (this._step <= 1);
        document.getElementById("nextBtn").disabled = (this._step == this._maxStepNo);
        //document.getElementById("finishBtn").disabled = (this._step < 2);
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
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);
    }

    setData(data) {   
        this._data = data;
    }

    onFinish() {
        this.collectStepData(true);
        if (!this.canFinish())
            return false;
        return true;
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

}