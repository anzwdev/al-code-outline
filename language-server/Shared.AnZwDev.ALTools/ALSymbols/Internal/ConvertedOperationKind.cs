using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols.Internal
{
    public enum ConvertedOperationKind
    {
        None = 0,
        InvalidStatement = 1,
        StatementList = 2,
        BlockStatement = 3,
        ExpressionStatement = 4,
        AssignmentStatement = 5,
        CompoundAssignmentStatement = 6,
        AssertErrorStatement = 7,
        WithStatement = 8,
        BreakStatement = 9,
        IfStatement = 10,
        LoopStatement = 11,
        CaseStatement = 12,
        CaseLine = 13,
        EmptyStatement = 14,
        ExitStatement = 15,
        InvalidExpression = 16,
        LiteralExpression = 17,
        ConversionExpression = 18,
        InvocationExpression = 19,
        ArrayElementReferenceExpression = 20,
        LocalReferenceExpression = 21,
        GlobalReferenceExpression = 22,
        ReturnValueReferenceExpression = 23,
        ParameterReferenceExpression = 24,
        UnaryOperatorExpression = 25,
        BinaryOperatorExpression = 26,
        ParenthesizedExpression = 27,
        FieldAccess = 28,
        TextIndexAccess = 29,
        OptionAccess = 30,
        TestFieldAccess = 31,
        TestPartAccess = 32,
        TestActionAccess = 33,
        TestFilterAccess = 34,
        TestFilterFieldAccess = 35,
        InListExpression = 36,
        PagePartAccess = 37,
        XmlPortDataItemAccess = 38,
        XmlPortAttributeAccess = 39,
        QueryElementAccess = 40,
        ReportDataItemAccess = 41,
        QueryDataItemAccess = 42,
        TableKeyAccess = 43,
        Argument = 44,
        TableFieldGroupAccess = 45,
        EnumTypeAccess = 46,
        ReportLabelAccess = 47
    }
}
