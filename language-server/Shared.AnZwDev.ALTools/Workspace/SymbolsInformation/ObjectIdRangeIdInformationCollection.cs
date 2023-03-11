using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ObjectIdRangeIdInformationCollection : List<ObjectIdRangeInformation>
    {

        public ObjectIdRangeIdInformationCollection(List<ALProjectIdRange> idRanges)
        {
            this.AddIdRanges(idRanges);
        }

        public void AddIdRanges(List<ALProjectIdRange> idRanges)
        {
            for (int i=0; i< idRanges.Count; i++)
            {
                this.Add(new ObjectIdRangeInformation(idRanges[i]));
            }
        }

        public void AddId(long id)
        {
            for (int i=0; i<this.Count; i++)
            {
                if (this[i].AddId(id))
                    return;
            }
        }

        public void AddIds(IEnumerable<long> idsEnumerable)
        {
            if (idsEnumerable != null)
            {
                foreach (long id in idsEnumerable)
                {
                    this.AddId(id);
                }
            }
        }

        public long FindNextFreeId()
        {
            for (int i=0; i<this.Count; i++)
            {
                long id = this[i].FindNextFreeId();
                if (id != 0)
                    return id;
            }
            return 0;
        }


    }
}
