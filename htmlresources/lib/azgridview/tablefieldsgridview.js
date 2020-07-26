class TableFieldsGridView extends AZGridView {

    constructor(showPK) {
        let fields = [
            {name: 'id', caption: 'Id', style: 'width: 80px;'},
            {name: 'name', caption: 'Name', style: 'width: 50%;'},
            //{name: 'caption', caption: 'Caption', style: 'width: 30%'},
            {name: 'dataType', caption: 'Data Type', style: 'width: 20%', autocomplete: [                
                'Blob', 'Boolean', 'Code', 'Date', 'DateFormula', 'DateTime', 'Decimal', 'Duration',
                'Enum', 'Guid', 'Integer', 'Media', 'MediaSet', 'Option', 'RecordId', 'TableFilter',
                'Text', 'Time']},
            {name: 'length', caption: 'Length', style: 'width:100px'},
            {name: 'dataClassification', caption: 'Data Classification', style: 'width: 20%', autocomplete: [
                'AccountData', 'CustomerContent', 'EndUserIdentifiableInformation', 'EndUserPseudonymousIdentifiers',
                'OrganizationIdentifiableInformation', 'SystemMetadata', 'ToBeClassified']}];
        
        if (showPK)
            fields.unshift(
                {name: 'pk', caption: 'PK', style: 'width: 32px', type: 'boolean'}
            );

        super('fieldsgrid', fields,
            undefined, undefined, 'Loading data...', true);
    }
}