using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using AnZwDev.ALTools.ALSymbolReferences.Compiler;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppSymbolReference : ALBaseSymbolReference
    {

        public ALAppSymbolReference()
        {
        }

        #region Objects processing

        /*
        public T FindObjectById<T>(ALAppSymbolsCollection<T> collection, int id, bool parsed) where T : ALAppObject
        {
            if ((collection == null) || (id == 0))
                return null;
            T alObject = collection
                .Where(p => (id == p.Id))
                .FirstOrDefault();

            if ((alObject != null) && (parsed) && (!alObject.INT_Parsed))
            {
                this.ParseObject(alObject);
                alObject = collection
                    .Where(p => (id == p.Id))
                    .FirstOrDefault();
            }

            return alObject;
        }
        */

        #endregion

        #region Parse object

        /*
        public void ParseObject(ALAppObject alAppObject)
        {
            if ((!String.IsNullOrWhiteSpace(alAppObject.ReferenceSourceFileName)) &&
                (!String.IsNullOrWhiteSpace(alAppObject.ReferenceSourceFileName)) &&
                (this.ReferenceSourceFileName.EndsWith(".app", StringComparison.OrdinalIgnoreCase)))
            {
                string content = AppFileHelper.GetAppFileContent(this.ReferenceSourceFileName, alAppObject.ReferenceSourceFileName);
                if (!String.IsNullOrWhiteSpace(content))
                {
                    ALSymbolReferenceCompiler compiler = new ALSymbolReferenceCompiler();
                    List<ALAppObject> objectsList = compiler.CreateObjectsList(alAppObject.ReferenceSourceFileName, content);
                    this.AllObjects.ReplaceRange(objectsList);
                }
            }
            alAppObject.INT_Parsed = true;
        }
        */

        #endregion

        public string GetNameWithPublisher()
        {
            return this.Publisher.NotNull() + " - " + this.Name.NotNull();
        }

        public void OnAfterDeserialized()
        {
            ProcessNamespaces();
        }

        private void ProcessNamespaces()
        {
            if (Namespaces != null)
                for (int i = 0; i < Namespaces.Count; i++)
                    Namespaces[i].Process(this, null);
        }

    }
}
