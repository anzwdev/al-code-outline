pageextension 50000 MAppDep1Dep1Table1CardExt extends Dep1Table1List
{
    layout
    {
        addafter("Name 3")
        {
            field("Dep2 Field1"; Rec."Dep2 Field1")
            {
                ApplicationArea = All;
                ToolTip = 'MainApp PageExt Dep2 Fiel1 field tooltip', Comment = '%';
            }
        }

    }

}