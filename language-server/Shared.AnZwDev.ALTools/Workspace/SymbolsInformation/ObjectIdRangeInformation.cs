using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ObjectIdRangeInformation
    {

        public long From { get; set; }
        public long To { get; set; }
        public List<long> Ids { get; set; }

        private bool _sorted;

        public ObjectIdRangeInformation(ALProjectIdRange range)
        {
            this.From = range.From;
            this.To = range.To;
            this.Ids = new List<long>();
            this._sorted = true;
        }

        public bool AddId(long id)
        {
            if (this.IsInRange(id))
            {
                this.Ids.Add(id);
                this._sorted = false;
                return true;
            }
            return false;
        }

        public bool IsInRange(long id)
        {
            return ((id >= this.From) && (id <= this.To));
        }

        public long FindNextFreeId()
        {
            if (this.Ids.Count == 0)
                return this.From;

            this.SortIds();

            long prevId = this.From;
            for (int i=0; i<this.Ids.Count; i++)
            {
                if (this.Ids[i] > (prevId + 1))
                    return prevId + 1;
                prevId = this.Ids[i];
            }
            if (this.To >= (prevId + 1))
                return prevId + 1;

            return 0;
        }

        protected void SortIds()
        {
            if (!this._sorted)
            {
                this.Ids.Sort();
                this._sorted = true;
            }
        }

    }
}
