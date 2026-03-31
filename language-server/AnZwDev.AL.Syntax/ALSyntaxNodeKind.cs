using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AnZwDev.AL.Syntax
{
    public enum ALSyntaxNodeKind
    {

        [Description("Undefined")]
        Undefined = 0,

        [Description("CompilationUnit")]
        CompilationUnit = 227,

        [Description("Properties")]
        PropertyList = 228,

        [Description("parameters")]
        ParameterList = 233,

        [Description("var")]
        VarSection = 235,

        [Description("trigger")]
        TriggerDeclaration = 236,

        [Description("event trigger")]
        EventTriggerDeclaration = 237,

        [Description("procedure")]
        MethodDeclaration = 238,

        [Description("event")]
        EventDeclaration = 239,

        [Description("parameter")]
        Parameter = 240,

        [Description("variable")]
        VariableDeclaration = 241,

        [Description("fields")]
        FieldList = 259,

        [Description("Field")]
        Field = 260,

        [Description("DotNetAssembly")]
        DotNetAssembly = 261,

        [Description("DotNetTypeDeclaration")]
        DotNetTypeDeclaration = 262,

        [Description("FieldExtensionList")]
        FieldExtensionList = 263,

        [Description("FieldModification")]
        FieldModification = 264,

        [Description("keys")]
        KeyList = 265,

        [Description("Key")]
        Key = 266,

        [Description("fieldgroups")]
        FieldGroupList = 267,

        [Description("FieldGroup")]
        FieldGroup = 268,

        [Description("layout")]
        PageLayout = 269,

        [Description("actions")]
        PageActionList = 270,

        [Description("groupactions")]
        GroupActionList = 271,

        [Description("Area")]
        PageArea = 272,

        [Description("Group")]
        PageGroup = 273,

        [Description("Field")]
        PageField = 274,

        [Description("Label")]
        PageLabel = 275,

        [Description("Part")]
        PagePart = 276,

        [Description("SystemPart")]
        PageSystemPart = 277,

        [Description("ChartPart")]
        PageChartPart = 278,

        [Description("UserControl")]
        PageUserControl = 279,

        [Description("Action")]
        PageAction = 280,

        [Description("Group")]
        PageActionGroup = 281,

        [Description("Area")]
        PageActionArea = 282,

        [Description("Separator")]
        PageActionSeparator = 283,

        [Description("actions")]
        PageExtensionActionList = 284,

        [Description("AddChange")]
        ActionAddChange = 285,

        [Description("MoveChange")]
        ActionMoveChange = 286,

        [Description("ModifyChange")]
        ActionModifyChange = 287,

        [Description("Layout")]
        PageExtensionLayout = 288,

        [Description("AddChange")]
        ControlAddChange = 289,

        [Description("MoveChange")]
        ControlMoveChange = 290,

        [Description("ModifyChange")]
        ControlModifyChange = 291,

        [Description("Views")]
        PageExtensionViewList = 292,

        [Description("AddChange")]
        ViewAddChange = 293,

        [Description("MoveChange")]
        ViewMoveChange = 294,

        [Description("ModifyChange")]
        ViewModifyChange = 295,

        [Description("dataset")]
        ReportDataSetSection = 296,

        [Description("labels")]
        ReportLabelsSection = 297,

        [Description("Data Item")]
        ReportDataItem = 298,

        [Description("Column")]
        ReportColumn = 299,

        [Description("Label")]
        ReportLabel = 300,

        [Description("Label")]
        ReportLabelMultilanguage = 301,

        [Description("schema")]
        XmlPortSchema = 302,

        [Description("TableElement")]
        XmlPortTableElement = 303,

        [Description("Field")]
        XmlPortFieldElement = 304,

        [Description("TextElement")]
        XmlPortTextElement = 305,

        [Description("Attribute")]
        XmlPortFieldAttribute = 306,

        [Description("TextAttribute")]
        XmlPortTextAttribute = 307,

        [Description("RequestOptionsPage")]
        RequestPage = 308,

        [Description("elements")]
        QueryElements = 309,

        [Description("Data Item")]
        QueryDataItem = 310,

        [Description("Column")]
        QueryColumn = 311,

        [Description("Filter")]
        QueryFilter = 312,

        [Description("EnumType")]
        EnumType = 314,

        [Description("EnumValue")]
        EnumValue = 315,

        [Description("EnumExtensionType")]
        EnumExtensionType = 316,

        [Description("PageViewList")]
        PageViewList = 319,

        [Description("PageView")]
        PageView = 320,

        [Description("Codeunit")]
        CodeunitObject = 411,

        [Description("Table")]
        TableObject = 412,

        [Description("TableExtension")]
        TableExtensionObject = 413,

        [Description("Page")]
        PageObject = 414,

        [Description("PageExtension")]
        PageExtensionObject = 415,

        [Description("Report")]
        ReportObject = 416,

        [Description("XmlPort")]
        XmlPortObject = 417,

        [Description("Query")]
        QueryObject = 418,

        [Description("ControlAddIn")]
        ControlAddInObject = 419,

        [Description("Profile")]
        ProfileObject = 420,

        [Description("PageCustomization")]
        PageCustomizationObject = 421,

        [Description("DotNetPackage")]
        DotNetPackage = 422,

        [Description("var")]
        GlobalVarSection = 428,


        VariableDeclarationName = 429,

        [Description("Entitlement")]
        Entitlement = 430,

        [Description("PermissionSet")]
        PermissionSet = 431,

        [Description("PermissionSetExtension")]
        PermissionSetExtension = 432,

        //ReportExtension = 433,

        [Description("AddColumn")]
        ReportExtensionAddColumnChange = 434,

        [Description("AddDataItem")]
        ReportExtensionAddDataItemChange = 435,

        [Description("AddDataSetColumn")]
        ReportExtensionDataSetAddColumn = 436,

        [Description("AddDataItem")]
        ReportExtensionDataSetAddDataItem = 437,

        [Description("ModifyDataSet")]
        ReportExtensionDataSetModify = 438,

        [Description("dataset")]
        ReportExtensionDataSetSection = 439,

        [Description("Modify")]
        ReportExtensionModifyChange = 440,

        [Description("Report Extension")]
        ReportExtensionObject = 441,

        [Description("RequestOptionsPage")]
        RequestPageExtension = 442,

        [Description("local procedure")]
        LocalMethodDeclaration = 50001,

        [Description("internal procedure")]
        InternalMethodDeclaration = 50064,

        [Description("protected procedure")]
        ProtectedMethodDeclaration = 50065,

        [Description("Primary Key")]
        PrimaryKey = 50002,

        [Description("Module")]
        Module = 50003,

        [Description("Tables")]
        TableObjectList = 50004,

        [Description("Pages")]
        PageObjectList = 50005,

        [Description("Reports")]
        ReportObjectList = 50006,

        [Description("XmlPorts")]
        XmlPortObjectList = 50007,

        [Description("Queries")]
        QueryObjectList = 50008,

        [Description("Codeunits")]
        CodeunitObjectList = 50009,

        [Description("ControlAddIns")]
        ControlAddInObjectList = 50010,

        [Description("PageExtensions")]
        PageExtensionObjectList = 50011,

        [Description("TableExtensions")]
        TableExtensionObjectList = 50012,

        [Description("Profiles")]
        ProfileObjectList = 50013,

        [Description("PageCustomizations")]
        PageCustomizationObjectList = 50014,

        [Description("Enums")]
        EnumObjectList = 50015,

        [Description("DotNetPackages")]
        DotNetPackageList = 50016,

        [Description("Enums")]
        EnumTypeList = 50017,

        [Description("EnumExtensions")]
        EnumExtensionTypeList = 50018,

        [Description("Interfaces")]
        InterfaceObjectList = 50059,

        [Description("ReportExtensions")]
        ReportExtensionObjectList = 50060,

        [Description("PermissionSets")]
        PermissionSetList = 50061,

        [Description("PermissionSetExtensions")]
        PermissionSetExtensionList = 50062,

        [Description("Entitlements")]
        EntitlementList = 50063,

        [Description("ProfileExtensions")]
        ProfileExtensionObject = 50070,

        [Description("Namespace")]
        Namespace = 50019,

        [Description("Package")]
        Package = 50020,

        [Description("Class")]
        Class = 50021,

        [Description("Property")]
        Property = 50022,

        [Description("Constructor")]
        Constructor = 50023,

        [Description("Interface")]
        Interface = 50024,

        [Description("Constant")]
        Constant = 50025,

        [Description("String")]
        String = 50026,

        [Description("Number")]
        Number = 50027,

        [Description("Boolean")]
        Boolean = 50028,

        [Description("Array")]
        Array = 50029,

        [Description("Null")]
        Null = 50030,

        [Description("Object")]
        Object = 50031,

        [Description("Struct")]
        Struct = 50032,

        [Description("Operator")]
        Operator = 50033,

        [Description("Repeater")]
        PageRepeater = 50034,

        //events
        [Description("Integration Event")]
        IntegrationEventDeclaration = 50035,

        [Description("Business Event")]
        BusinessEventDeclaration = 50036,

        [Description("Event Subscriber")]
        EventSubscriberDeclaration = 50037,

        [Description("Internal Event")]
        InternalEventDeclaration = 50068,

        [Description("External Business Event")]
        ExternalBusinessEventDeclaration = 50069,

        //tests
        [Description("Test")]
        TestDeclaration = 50038,

        [Description("Confirm Handler")]
        ConfirmHandlerDeclaration = 50039,

        [Description("Filter Page Handler")]
        FilterPageHandlerDeclaration = 50040,

        [Description("Hyperlink Handler")]
        HyperlinkHandlerDeclaration = 50041,

        [Description("Message Handler")]
        MessageHandlerDeclaration = 50042,

        [Description("Modal Page Handler")]
        ModalPageHandlerDeclaration = 50043,

        [Description("Page Handler")]
        PageHandlerDeclaration = 50044,

        [Description("Report Handler")]
        ReportHandlerDeclaration = 50045,

        [Description("Request Page Handler")]
        RequestPageHandlerDeclaration = 50046,

        [Description("Send Notification Handler")]
        SendNotificationHandlerDeclaration = 50047,

        [Description("Session Settings Handler")]
        SessionSettingsHandlerDeclaration = 50048,

        [Description("StrMenu Handler")]
        StrMenuHandlerDeclaration = 50049,

        [Description("Project")]
        ProjectDefinition = 50050,

        [Description("Packages")]
        PackagesList = 50051,

        [Description("Dependencies")]
        Dependencies = 50052,

        [Description("Document")]
        Document = 50053,

        [Description("SymbolGroup")]
        SymbolGroup = 50054,

        [Description("AL Object")]
        AnyALObject = 50055,         //any symbol, used in requests to specify kind of objects

        //Syntax tree
        [Description("SyntaxTreeNode")]
        SyntaxTreeNode = 50056,

        [Description("SyntaxTreeToken")]
        SyntaxTreeToken = 50057,

        [Description("SyntaxTreeTrivia")]
        SyntaxTreeTrivia = 50058,

        [Description("Region")]
        Region = 50066,

        [Description("Using")]
        UsingDirective = 50067,

        [Description("FieldGroupAddChange")]
        FieldGroupAddChange = 50071,

        [Description("FieldGroupExtensionList")]
        FieldGroupExtensionList = 50072,

        [Description("PageActionRef")]
        PageActionRef = 50073,

        [Description("PageCustomAction")]
        PageCustomAction = 50074,

        [Description("PageFieldUploadAction")]
        PageFieldUploadAction = 50075,

        [Description("PageSystemAction")]
        PageSystemAction = 50076,

        [Description("ReportLayout")]
        ReportLayout = 50077,

        [Description("ReportRenderingSection")]
        ReportRenderingSection = 50078,

        [Description("ProfileExtensionsList")]
        ProfileExtensionObjectList = 50079,

        [Description("Permission")]
        Permission = 50080

        //Next available id 50081

    }
}
