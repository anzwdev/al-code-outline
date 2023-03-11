using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCDuplicateComparer : IComparer<DCDuplicate>
    {
        public int Compare(DCDuplicate x, DCDuplicate y)
        {
            if (x == null && y == null)
                return 0;
            if (y == null)
                return 1;
            if (x == null)
                return -1;            

            int value = (y.noOfStatements - x.noOfStatements);
            if (value != 0)
                return value;

            return (y.ranges.Count - x.ranges.Count);
        }
    }
}
