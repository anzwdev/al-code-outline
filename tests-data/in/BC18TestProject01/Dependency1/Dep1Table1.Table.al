table 51000 "Dep1 Table 1"
{
    DataClassification = ToBeClassified;

    fields
    {
        field(1; "No."; Code[20])
        {
            Caption = 'No.';
        }
        field(2; "Name"; Text[100])
        {
            Caption = 'Name Caption';
        }
        field(3; "Name 2"; Text[100])
        {
            Caption = 'Name 2 Caption';
        }
        field(4; "Name 3"; Text[100])
        {
            Caption = 'Name 3 Caption';
        }
    }
    keys
    {
        key(Key1; "No.")
        {
            Clustered = true;
        }
    }
    fieldgroups
    {
    }

    var
        myInt: Integer;
}