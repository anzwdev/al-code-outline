using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectProperties
    {

        #region Public properties

        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public VersionNumber Version { get; set; }
        public VersionNumber Runtime { get; set; }
        public List<ALProjectIdRange> Ranges { get; set; }
        public List<ALProjectReference> InternalsVisibleTo { get; set; }

        #endregion

        public ALProjectProperties()
        {
            this.Ranges = new List<ALProjectIdRange>();
        }

        public ALProjectIdRange AddIdRange(long fromId, long toId)
        {
            ALProjectIdRange idRange = new ALProjectIdRange(fromId, toId);
            this.Ranges.Add(idRange);
            return idRange;
        }


    }
}
