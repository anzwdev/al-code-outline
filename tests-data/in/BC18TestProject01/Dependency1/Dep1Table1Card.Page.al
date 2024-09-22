page 51001 Dep1Table1Card
{
    Caption = 'Dep1Table1Card';
    PageType = Card;
    SourceTable = "Dep1 Table 1";

    layout
    {
        area(Content)
        {
            group(General)
            {
                Caption = 'General';

                field("No."; Rec."No.")
                {
                    ToolTip = 'No. field card tooltip', Comment = '%';
                }
                field(Name; Rec.Name)
                {
                    ToolTip = 'Name field card tooltip', Comment = '%';
                }
            }
        }
    }
}
