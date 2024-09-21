page 50001 RemoveWithPage1
{
    PageType = List;
    ApplicationArea = All;
    UsageCategory = Administration;
    SourceTable = "Sales Line";

    layout
    {
        area(Content)
        {
            group(GroupName)
            {
                field(FldNo; "No.")
                {
                    ApplicationArea = All;
                }
                field(FldType; Type)
                {
                    Visible = Type <> Type::Item;
                    ApplicationArea = All;
                }
                field(FldDescription; Rec.Description)
                {
                    ApplicationArea = All;
                }
            }
        }
    }

}