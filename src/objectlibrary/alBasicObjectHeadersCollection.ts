import { ALBasicObjectHeader } from "./alBasicObjectHeader";

export class ALBasicObjectHeadersCollection {
    objectType : string;
    objects : ALBasicObjectHeader[];

    constructor(newObjectType : string, data : any) {
        var symbols : ALBasicObjectHeader[] = [];

        if (data) {
            var len = data.length;

            data = data.sort((n1,n2) => {
                if (n1.Id && n2.Id)
                    return (n1.Id - n2.Id);
                return 0;});

            for (var i=0; i<len; i++) {
                var inf = new ALBasicObjectHeader();
                if (!data[i].Id)
                    data[i].Id = (-1 - i); 

                inf.Id = data[i].Id;
                if (inf.Id > 0)
                    inf.OId = inf.Id;

                inf.Name = data[i].Name;
                symbols.push(inf);
            }
        }

        this.objectType = newObjectType;
        this.objects = symbols;
    }

    findObjectHeader(objectId : number) : ALBasicObjectHeader {
        for (var i=0; i<this.objects.length; i++) {
            if (this.objects[i].Id === objectId)
                return this.objects[i];
        }
        return null;
    }

}