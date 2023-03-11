export class XmlHelper {

    static EncodeXmlAttributeValue(value: string) : string {
        return value.replace(/&/g, '&amp;')
               .replace(/</g, '&lt;')
               .replace(/>/g, '&gt;')
               .replace(/"/g, '&quot;')
               .replace(/'/g, '&apos;');
    }

}