export class SymbolWithNameInformation {
    name: string | undefined;
    caption: string | undefined;

    public static toNamesList(items: SymbolWithNameInformation[] | undefined): string[] {
        let values: string[] = [];
        if (items)
            for (let i=0; i<items.length; i++)
                if (items[i].name)
                    values.push(items[i].name!);
        return values;
    }

}