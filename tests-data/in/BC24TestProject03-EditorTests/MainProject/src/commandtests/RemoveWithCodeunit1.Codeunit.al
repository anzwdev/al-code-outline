codeunit 50000 RemoveWithCodeunit1
{
    procedure WithTests()
    var
        SalesLine: Record "Sales Line";
        SalesHeader: Record "Sales Header";
    begin
        with SalesHeader do begin
            FindFirst();
            with SalesLine do begin
                Reset();
                SetRange("Document Type", SalesHeader."Document Type");
                SetRange("Document No.", SalesHeader."No.");
                if (FindFirst()) then begin
                    Message('%1 %2 %3',
                        "Sell-to Customer No.",
                        "Document Type",
                        "No. Series");
                end;
            end;
        end;
    end;

}