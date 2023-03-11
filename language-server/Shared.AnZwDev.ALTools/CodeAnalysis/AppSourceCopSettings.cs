using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.CodeAnalysis
{
    public class AppSourceCopSettings
    {

        [JsonProperty("mandatoryAffixes")]
        public string[] MandatoryAffixes { get; set; }

        [JsonProperty("mandatoryPrefix")]
        public string MandatoryPrefix { get; set; }

        [JsonProperty("mandatorySuffix")]
        public string MandatorySuffix { get; set; }

        public AppSourceCopSettings()
        {
        }

        public static AppSourceCopSettings FromString(string data)
        {
            return JsonConvert.DeserializeObject<AppSourceCopSettings>(data);
        }

        public static AppSourceCopSettings FromFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string data = FileUtils.SafeReadAllText(path);
                    if (!String.IsNullOrWhiteSpace(data))
                        return AppSourceCopSettings.FromString(data);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        protected List<string> GetMergedAffixes(string additionalAffix)
        {
            List<string> mergedAffixes = null;
            if ((this.MandatoryAffixes != null) && (this.MandatoryAffixes.Length > 0))
            {
                mergedAffixes = new List<string>();
                mergedAffixes.AddRange(this.MandatoryAffixes);
            }
            if (!String.IsNullOrWhiteSpace(additionalAffix))
            {
                if (mergedAffixes == null)
                    mergedAffixes = new List<string>();
                mergedAffixes.Add(additionalAffix);
            }
            return mergedAffixes;
        }

        public void GetAllAffixes(out List<string> prefixes, out List<string> suffixes)
        {
            prefixes = this.GetMergedAffixes(this.MandatoryPrefix);
            suffixes = this.GetMergedAffixes(this.MandatorySuffix);
        }

    }
}
