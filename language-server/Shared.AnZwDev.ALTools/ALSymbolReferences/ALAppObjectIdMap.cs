using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectIdMap
    {

        public Dictionary<int, ALAppTable> TableIdMap { get; }
        public Dictionary<int, ALAppReport> ReportIdMap { get; }
        public Dictionary<int, ALAppCodeunit> CodeunitIdMap { get; }
        public Dictionary<int, ALAppXmlPort> XmlPortIdMap { get; }
        public Dictionary<int, ALAppPage> PageIdMap { get; }
        public Dictionary<int, ALAppQuery> QueryIdMap { get; }

        public ALAppObjectIdMap()
        {
            TableIdMap = new Dictionary<int, ALAppTable>();
            ReportIdMap = new Dictionary<int, ALAppReport>();
            CodeunitIdMap = new Dictionary<int, ALAppCodeunit>();
            XmlPortIdMap = new Dictionary<int, ALAppXmlPort>();
            PageIdMap = new Dictionary<int, ALAppPage>();
            QueryIdMap = new Dictionary<int, ALAppQuery>();
        }

        public void Clear()
        {
            this.TableIdMap.Clear();
        }

        public void Add<T>(Dictionary<int, T> idMap, T item) where T : ALAppElementWithNameId
        {
            if ((item != null) && (!idMap.ContainsKey(item.Id)))
                idMap.Add(item.Id, item);
        }

        public void AddRange<T>(Dictionary<int, T> idMap, ALAppElementsCollection<T> collection) where T : ALAppElementWithNameId
        {
            if (collection != null)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if ((collection[i] != null) && (!idMap.ContainsKey(collection[i].Id)))
                        idMap.Add(collection[i].Id, collection[i]);
                }
            }
        }


    }
}
