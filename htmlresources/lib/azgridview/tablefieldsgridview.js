class TableFieldsGridView extends AZGridView {

    constructor() {
        super('fieldsgrid', 
            [{name: 'id', caption: 'Id', style: 'width: 80px;'},
            {name: 'name', caption: 'Name', style: 'width: 50%;'},
            //{name: 'caption', caption: 'Caption', style: 'width: 30%'},
            {name: 'dataType', caption: 'Data Type', style: 'width: 20%', autocomplete: [                
                'Blob', 'Boolean', 'Code', 'Date', 'DateFormula', 'DateTime', 'Decimal', 'Duration',
                'Enum', 'Guid', 'Integer', 'Media', 'MediaSet', 'Option', 'RecordId', 'TableFilter',
                'Text', 'Time']},
            {name: 'length', caption: 'Length', style: 'width:100px'},
            {name: 'dataClassification', caption: 'Data Classification', style: 'width: 20%', autocomplete: [
                'AccountData', 'CustomerContent', 'EndUserIdentifiableInformation', 'EndUserPseudonymousIdentifiers',
                'OrganizationIdentifiableInformation', 'SystemMetadata', 'ToBeClassified']}],
            undefined, undefined, 'Loading data...', true);
    }
}