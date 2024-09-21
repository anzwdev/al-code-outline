namespace MainProject;
using Dependency1;

page 50000 MainAppDep1Table1List
{
    ApplicationArea = All;
    Caption = 'MainAppDep1Table1List';
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
                    ToolTip = 'No. field MainApp list tooltip', Comment = '%';
                }
                field("Name 2"; Rec."Name 2")
                {
                    ToolTip = 'Name 2 field MainApp list tooltip', Comment = '%';
                }
            }
        }
    }
}
