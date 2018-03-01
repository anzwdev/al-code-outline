import { ALBasicObjectHeadersCollection } from "./alBasicObjectHeadersCollection";
import { ALBasicObjectHeader } from "./alBasicObjectHeader";

export class ALBasicLibrary {
    name : string;
    objectCollections : ALBasicObjectHeadersCollection[];  
    
    constructor(newName : string, symbolRef : any) {
        this.name = newName;
        this.objectCollections = [];
        this.fromAppSymbolReferences(symbolRef);
    }

    private fromAppSymbolReferences(symbolRef) {
        this.objectCollections.push(new ALBasicObjectHeadersCollection('Table', symbolRef.Tables));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('Page', symbolRef.Pages));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('Report', symbolRef.Reports));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('XmlPort', symbolRef.XmlPorts));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('Query', symbolRef.Queries));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('Codeunit', symbolRef.Codeunits));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('ControlAddIn', symbolRef.ControlAddIns));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('PageExtension', symbolRef.PageExtensions));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('TableExtension', symbolRef.TableExtensions));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('Profile', symbolRef.Profiles));
        this.objectCollections.push(new ALBasicObjectHeadersCollection('PageCustomization', symbolRef.PageCustomizations));
    }

    private findObjectsCollection(objectType : string) : ALBasicObjectHeadersCollection {
        for (var i=0; i<this.objectCollections.length; i++) {
            if (this.objectCollections[i].objectType === objectType)
                return this.objectCollections[i];
        }
        return null;
    }

    findObjectHeader(objectType : string, objectId : number) : ALBasicObjectHeader {
        var objectsCollection = this.findObjectsCollection(objectType);
        if (objectsCollection)
            return objectsCollection.findObjectHeader(objectId);
        return null;
    }

}