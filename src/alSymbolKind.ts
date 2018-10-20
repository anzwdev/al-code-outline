export enum ALSymbolKind {
    Undefined = 0,
    TreeRoot = 1,
    //Objects
    UndefinedObject = 2,
	Table = 3,
	Codeunit = 4,
	Page = 5,
	Report = 6,
	Query = 7,
    XmlPort = 8,
    TableExtension = 9,
    PageExtension = 10,
    ControlAddIn = 11,
    Profile = 12,
    PageCustomization = 13,
    Enum = 14,
    DotNetPackage = 15,
    //Object specific child symbols
    Property = 16,
    //Code
    Variable = 17,
    Constant = 18,
	Parameter = 19,
    Method = 20,
    Trigger = 21,
    EventPublisher = 22,
    EventSubscriber = 23,
    //Tables
    Field = 24,
    PrimaryKey = 25,
    Key = 26,
    FieldGroup = 27,
    //Pages
    Group = 28,
    Action = 29,
    //Other special symbols
    SymbolGroup = 30,
    //Enums
    EnumValue = 31,
    //DotNet
    DotNetAssembly = 32,
    DotNetType = 33
}