pageextension 52000 Dep2Dep1Dep1Table1ListExt extends Dep1Table1List
{
    layout
    {
        addafter("Name 2")
        {
            field("Name 3"; Rec."Name 3")
            {
                ApplicationArea = All;
                ToolTip = 'Dep2 PageExt Name 3 field tooltip', Comment = '%';
            }
        }

    }

}