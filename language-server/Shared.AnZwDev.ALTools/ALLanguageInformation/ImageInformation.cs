using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public class ImageInformation
    {

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }

        public ImageInformation()
        {
        }

        public ImageInformation(string newName, string newContent)
        {
            this.Name = newName;
            this.Content = newContent;
        }

    }
}
