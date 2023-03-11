using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementKeyDictionary : Dictionary<string, DCStatementKey>
    {
        public DCStatementKeyDictionary()
        {
        }

        public DCStatementKey GetOrCreate(string value, bool ignore)
        {
            if (this.ContainsKey(value))
                return this[value];
            DCStatementKey key = new DCStatementKey(value, ignore);
            this.Add(value, key);
            return key;
        }

    }
}
