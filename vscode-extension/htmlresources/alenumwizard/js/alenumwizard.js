class EnumWizard extends BaseObjectWizard {

    constructor() {
        super(1);
    }

    setData(data) {
        super.setData(data);
        //initialize fields
        document.getElementById("objectid").value = this._data.objectId;
        
        let objectNameFld = document.getElementById("objectname");        
        objectNameFld.value = this._data.objectName;
        if (this._data.limitNameLength) {
            objectNameFld.maxLength = 30;
        }

        document.getElementById("valuelist").value = this._data.valueList;
        document.getElementById("captionlist").value = this._data.captionList;
        document.getElementById("extensible").checked = this._data.extensible;
    }
   
    sendFinishMessage() {
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                valueList : this._data.valueList,
                captionList : this._data.captionList,
                extensible : this._data.extensible
            }
        });
    }

    collectStepData(finishSelected) {
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.valueList = document.getElementById("valuelist").value;
        this._data.captionList = document.getElementById("captionlist").value;
        this._data.extensible = document.getElementById("extensible").checked;
    }

}

var wizard;

window.onload = function() {
    wizard = new EnumWizard();
};
