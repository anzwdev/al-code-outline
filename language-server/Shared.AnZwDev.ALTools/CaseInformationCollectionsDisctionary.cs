using AnZwDev.ALTools.ALLanguageInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools
{
    public class CaseInformationCollectionsDictionary
    {

        private Dictionary<string, CaseInformationCollection> _collections = new Dictionary<string, CaseInformationCollection>();

        public CaseInformationCollectionsDictionary()
        {
        }

        public void Add(string collectionName, params string[] values)
        {
            var collection = new CaseInformationCollection();
            for (int i = 0; i < values.Length; i++)
                collection.Add(values[i]);
            _collections.Add(collectionName.ToLower(), collection);
        }

        public string FixCase(string collectionName, string value)
        {
            collectionName = collectionName.ToLower();
            if (_collections.ContainsKey(collectionName))
                value = _collections[collectionName].FixCase(value);
            return value;
        }


    }
}
