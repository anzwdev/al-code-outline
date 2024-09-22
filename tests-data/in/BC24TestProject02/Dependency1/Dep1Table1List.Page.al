namespace Dependency1;

page 51000 Dep1Table1List
{
    ApplicationArea = All;
    Caption = 'Dep1Table1List';
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
                    ToolTip = 'No. field list tooltip', Comment = '%';
                }
                field(Name; Rec.Name)
                {
                    ToolTip = 'Name field list tooltip', Comment = '%';
                }
                field("Name 2"; Rec."Name 2")
                {
                    ToolTip = 'Name 2 field list tooltip', Comment = '%';
                }
            }
        }
    }
}
