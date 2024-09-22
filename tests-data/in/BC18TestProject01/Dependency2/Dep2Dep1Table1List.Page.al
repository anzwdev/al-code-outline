page 52000 Dep2Dep1Table1List
{
    ApplicationArea = All;
    Caption = 'Dep2Dep1Table1List';
    PageType = List;
    SourceTable = "Dep1 Table 1";
    UsageCategory = Lists;

    layout
    {
        area(Content)
        {
            repeater(General)
            {
                field("No."; Rec."No.")
                {
                    ToolTip = 'No. field list Dep2 tooltip', Comment = '%';
                }
                field("Dep2 Fiel1"; Rec."Dep2 Field1")
                {
                    ToolTip = 'Dep2 Field1 list tooltip', Comment = '%';
                }
            }
        }
    }
}
