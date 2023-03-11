using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols
{
    public enum ALSymbolKind
    {

        Undefined = 0,

        //BooleanLiteralValue = 169,
        //Int32SignedLiteralValue = 170,
        //Int64SignedLiteralValue = 171,
        //DecimalSignedLiteralValue = 172,
        //DateLiteralValue = 173,
        //TimeLiteralValue = 174,
        //DateTimeLiteralValue = 175,
        //StringLiteralValue = 176,

        //IdentifierName = 220,
        //QualifiedName = 221,
        //IdentifierNameOrEmpty = 222,
        //ArgumentList = 223,
        //BracketedArgumentList = 224,

        CompilationUnit = 227,

        PropertyList = 228,

        //OptionValues = 229,
        //ObjectReference = 230,
        //ObjectId = 231,
        //ObjectNameReference = 232,
        ParameterList = 233,
        //MethodBody = 234,
        VarSection = 235,
        TriggerDeclaration = 236,
        EventTriggerDeclaration = 237,
        MethodDeclaration = 238,
        EventDeclaration = 239,
        Parameter = 240,
        VariableDeclaration = 241,
        //ReturnValue = 242,
        //SimpleTypeReference = 243,
        //RecordTypeReference = 244,
        //DotNetTypeReference = 245,
        //DataType = 246,
        //GenericDataType = 247,
        //OptionDataType = 248,
        //TextConstDataType = 249,
        //LabelDataType = 250,
        //DotNetDataType = 251,
        //LengthDataType = 252,
        //SubtypedDataType = 253,
        //EnumDataType = 254,
        //Array = 255,
        //BracketedDimensionList = 256,
        //Dimension = 257,
        //MemberAttribute = 258,
        FieldList = 259,
        Field = 260,
        DotNetAssembly = 261,
        DotNetTypeDeclaration = 262,
        FieldExtensionList = 263,
        FieldModification = 264,
        KeyList = 265,
        Key = 266,
        FieldGroupList = 267,
        FieldGroup = 268,

        PageLayout = 269,
        PageActionList = 270,
        GroupActionList = 271,
        PageArea = 272,
        PageGroup = 273,
        PageField = 274,
        PageLabel = 275,
        PagePart = 276,
        PageSystemPart = 277,
        PageChartPart = 278,
        PageUserControl = 279,
        PageAction = 280,
        PageActionGroup = 281,
        PageActionArea = 282,
        PageActionSeparator = 283,

        PageExtensionActionList = 284,
        ActionAddChange = 285,
        ActionMoveChange = 286,
        ActionModifyChange = 287,
        PageExtensionLayout = 288,
        ControlAddChange = 289,
        ControlMoveChange = 290,
        ControlModifyChange = 291,
        PageExtensionViewList = 292,
        ViewAddChange = 293,
        ViewMoveChange = 294,
        ViewModifyChange = 295,

        ReportDataSetSection = 296,
        ReportLabelsSection = 297,
        ReportDataItem = 298,
        ReportColumn = 299,
        ReportLabel = 300,
        ReportLabelMultilanguage = 301,

        XmlPortSchema = 302,
        XmlPortTableElement = 303,
        XmlPortFieldElement = 304,
        XmlPortTextElement = 305,
        XmlPortFieldAttribute = 306,
        XmlPortTextAttribute = 307,
        RequestPage = 308,

        QueryElements = 309,
        QueryDataItem = 310,
        QueryColumn = 311,
        QueryFilter = 312,
        //Label = 313,

        EnumType = 314,
        EnumValue = 315,
        EnumExtensionType = 316,
        //FieldGroupExtensionList = 317,
        //FieldGroupAddChange = 318,

        PageViewList = 319,
        PageView = 320,

        //PageFieldReferencePropertyValue = 340,

        CodeunitObject = 411,
        TableObject = 412,
        TableExtensionObject = 413,
        PageObject = 414,
        PageExtensionObject = 415,
        ReportObject = 416,
        XmlPortObject = 417,
        QueryObject = 418,
        ControlAddInObject = 419,
        ProfileObject = 420,
        PageCustomizationObject = 421,
        DotNetPackage = 422,

        //AttributeArgumentList = 423,
        //LiteralAttributeArgument = 424,
        //MethodReferenceAttributeArgument = 425,
        //OptionAccessAttributeArgument = 426,
        //InvalidAttributeArgument = 427,

        GlobalVarSection = 428,

        VariableDeclarationName = 429,

        Entitlement = 430,
        PermissionSet = 431,
        PermissionSetExtension = 432,
        //ReportExtension = 433,
        ReportExtensionAddColumnChange = 434,
        ReportExtensionAddDataItemChange = 435,
        ReportExtensionDataSetAddColumn = 436,
        ReportExtensionDataSetAddDataItem = 437,
        ReportExtensionDataSetModify = 438,
        ReportExtensionDataSetSection = 439,
        ReportExtensionModifyChange = 440,
        ReportExtensionObject = 441,
        RequestPageExtension = 442,

        //AddKeyword
        //EntitlementKeyword
        //ReportExtensionKeyword
        //PermissionSetExtensionKeyword
        //PermissionSetKeyword

        LocalMethodDeclaration = 50001,
        InternalMethodDeclaration = 50064,
        ProtectedMethodDeclaration = 50065,
        PrimaryKey = 50002,
        Module = 50003,

        TableObjectList = 50004,
        PageObjectList = 50005,
        ReportObjectList = 50006,
        XmlPortObjectList = 50007,
        QueryObjectList = 50008,
        CodeunitObjectList = 50009,
        ControlAddInObjectList = 50010,
        PageExtensionObjectList = 50011,
        TableExtensionObjectList = 50012,
        ProfileObjectList = 50013,
        PageCustomizationObjectList = 50014,
        EnumObjectList = 50015,
        DotNetPackageList = 50016,
        EnumTypeList = 50017,
        EnumExtensionTypeList = 50018,
        InterfaceObjectList = 50059,
        ReportExtensionObjectList = 50060,
        PermissionSetList = 50061,
        PermissionSetExtensionList = 50062,
        EntitlementList = 50063,

        Namespace = 50019,
        Package = 50020,
        Class = 50021,
        Property = 50022,
        Constructor = 50023,
        Interface = 50024,
        Constant = 50025,
        String = 50026,
        Number = 50027,
        Boolean = 50028,
        Array = 50029,
        Null = 50030,
        Object = 50031,
        Struct = 50032,
        Operator = 50033,

        PageRepeater = 50034,

        //events
        IntegrationEventDeclaration = 50035,
        BusinessEventDeclaration = 50036,
        EventSubscriberDeclaration = 50037,
        
        //tests
        TestDeclaration = 50038,
        ConfirmHandlerDeclaration = 50039,
        FilterPageHandlerDeclaration = 50040,
        HyperlinkHandlerDeclaration = 50041,
        MessageHandlerDeclaration = 50042,
        ModalPageHandlerDeclaration = 50043,
        PageHandlerDeclaration = 50044,
        ReportHandlerDeclaration = 50045,
        RequestPageHandlerDeclaration = 50046,
        SendNotificationHandlerDeclaration = 50047,
        SessionSettingsHandlerDeclaration = 50048,
        StrMenuHandlerDeclaration = 50049,

        ProjectDefinition = 50050,
        PackagesList = 50051,
        Dependencies = 50052,
        Document = 50053,
        SymbolGroup = 50054,

        AnyALObject = 50055,         //any symbol, used in requests to specify kind of objects

        //Syntax tree
        SyntaxTreeNode = 50056,
        SyntaxTreeToken = 50057,
        SyntaxTreeTrivia = 50058,

        Region = 50066
        //Next available id 50067
    }
}
