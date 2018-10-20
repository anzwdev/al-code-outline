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
    //Object specific child symbols
    Property = 14,
    //Code
    Variable = 15,
    Constant = 16,
	Parameter = 17,
    Method = 18,
    Trigger = 19,
    EventPublisher = 20,
    EventSubscriber = 21,
    //Tables
    Field = 22,
    PrimaryKey = 23,
    Key = 24,
    FieldGroup = 25,
    //Pages
    Group = 26,
    Action = 27,
    //Other special symbols
    SymbolGroup = 28
}