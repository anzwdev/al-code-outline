export interface ICRSExtensionPublicApi {
    RunObjectApi : ICRSRunObjectApi;
    ObjectNamesApi : ICRSObjectNamesApi;
}

export interface ICRSObjectNamesApi {
    GetObjectFileName(objectType: string, objectId : string, objectName: string) : string;
    GetObjectExtensionFileName(objectType: string, objectId : string, objectName: string, extendedObjectId : string, extendedObjectName : string) : string;
    GetObjectExtensionName(objectType: string, objectId : string, objectName: string, extendedObjectId : string, extendedObjectName : string) : string;
    GetBestPracticeAbbreviatedObjectType(ObjectType: String): string;
}

export interface ICRSRunObjectApi {
    RunObjectInWebClient(objecttype: any, objectid: any) : void;
    RunObjectInTabletClient(objecttype: any, objectid: any) : void;
    RunObjectInPhoneClient(objecttype: any, objectid: any) : void;
    RunObjectInWindowsClient(objecttype: any, objectid: any) : void;    
}
