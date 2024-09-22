namespace MainProject;

table 50300 TableWithoutCaptions
{
    DataClassification = ToBeClassified;

    fields
    {
        field(1; "No."; Code[20])
        {
        }
        field(2; "Name"; Text[100])
        {
        }
        field(3; "Name 2"; Text[100])
        {
            Caption = 'Name 2 Caption';
        }
        field(4; "Name 3"; Text[100])
        {
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