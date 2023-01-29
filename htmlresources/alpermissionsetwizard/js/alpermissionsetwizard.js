class PermissionSetWizard extends BaseObjectWizard {

    constructor() {
        super(2);
        this._srcPermSets = new FilteredList('srcpermsfilter', 'srcperms');
        this._destPermSets = new FilteredList('destpermsfilter', 'destperms');
        this.registerPermSSelectionEvents();
        this.initCaptionLenUpdate();
    }

    onMessage(message) {
        switch (message.command) {
            case 'setPermissionSets':
                this.setPermissionSets(message.data);
                break;
            default:
                super.onMessage(message);
                break;
        }
    }

    setData(data) {
        super.setData(data);

        if (this._data) {

            //initialize inputs
            document.getElementById("objectid").value = this._data.objectId;
            document.getElementById("objectname").value = this._data.objectName;
            document.getElementById("inclallobjects").checked = this._data.inclAllObjects;

            let ctObjCaption = document.getElementById("objectcaption");
            if (ctObjCaption)
                ctObjCaption.value = this._data.objectCaption;

            this.updateControls();
        }
    }

    sendFinishMessage() {
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                objectCaption: this._data.objectCaption,
                inclAllObjects : this._data.inclAllObjects,
                selectedPermissionSetList: this._data.selectedPermissionSetList
            }
        });
    }

    collectStepData(finishSelected) {
        switch (this._step) {
            case 1: this.collectStep1Data(finishSelected);
            case 2: this.collectStep2Data(finishSelected);
        }
    }

    collectStep1Data(finishSelected) {
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;

        let ctObjCaption = document.getElementById("objectcaption");
        if (ctObjCaption)
            this._data.objectCaption = ctObjCaption.value;
        this._data.inclAllObjects = document.getElementById("inclallobjects").checked;
    }

    collectStep2Data(finishSelected) {
        this._data.selectedPermissionSetList = this._destPermSets.getAll();
    }

    setPermissionSets(permissionSets) {
        if (!this._data)
            this._data = {};
        this._data.permissionSetList = permissionSets;
        this.loadPermissionSets();
    }

    loadPermissionSets() {
        this._srcPermSets.setData(this._data.permissionSetList);
        this._destPermSets.clear();
    }

    registerPermSSelectionEvents() {
        document.getElementById('mselright').addEventListener('click', event => {
            this.onMovePermSRight();
        });
        document.getElementById('mselallright').addEventListener('click', event => {
            this.onMoveAllRight();
        });      
        document.getElementById('mselallleft').addEventListener('click', event => {
            this.onMoveAllLeft();
        });
        document.getElementById('mselleft').addEventListener('click', event => {
            this.onMovePermSLeft();
        });      
        document.getElementById('srcperms').addEventListener('dblclick', event => {
            this.onMovePermSRight();
        });
        document.getElementById('destperms').addEventListener('dblclick', event => {
            this.onMovePermSLeft();
        });
    }

    onMovePermSRight() {
        this._destPermSets.add(this._srcPermSets.removeSelected());
    }

    onMovePermSLeft() {
        this._srcPermSets.add(this._destPermSets.removeSelected());
    }

    onMoveAllRight() {
        this._destPermSets.add(this._srcPermSets.removeFiltered());
    }

    onMoveAllLeft() {
        this._srcPermSets.add(this._destPermSets.removeFiltered());
    }

    getSelectedPermissionSets() {
        return this._destPermSets.getAll();
    }

}


