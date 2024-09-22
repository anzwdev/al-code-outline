page 50002 AppAreasPage1
{
    ApplicationArea = All;
    Caption = 'AppAreasPage1';
    PageType = List;
    SourceTable = "Sales Line";
    UsageCategory = Lists;

    layout
    {
        area(Content)
        {
            repeater(General)
            {
                field("Document Type"; Rec."Document Type")
                {
                    ToolTip = 'Specifies the type of document that you are about to create.';
                }
                field("Sell-to Customer No."; Rec."Sell-to Customer No.")
                {
                    ToolTip = 'Specifies the number of the customer.';
                }
                field("Document No."; Rec."Document No.")
                {
                    ToolTip = 'Specifies the document number.';
                }
                field("Line No."; Rec."Line No.")
                {
                    ToolTip = 'Specifies the line number.';
                }
                field("Type"; Rec."Type")
                {
                    ToolTip = 'Specifies the line type.';
                }
                field("No."; Rec."No.")
                {
                    ToolTip = 'Specifies the number of the record.';
                }
            }
        }
    }
    actions
    {
        area(Processing)
        {
            action(Action1)
            {
                Caption = 'Action 1';
            }
            action(Action2)
            {
                Caption = 'Action 2';
                ApplicationArea = Basic;
            }
        }
    }
}
