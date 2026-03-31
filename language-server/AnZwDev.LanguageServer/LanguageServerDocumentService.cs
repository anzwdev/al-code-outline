using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.LanguageServer
{
    public class LanguageServerDocumentService
    {

        private readonly Dictionary<string, LanguageServerDocument> _documents = new Dictionary<string, LanguageServerDocument>();

        public T? GetDocument<T>(string documentId) where T: LanguageServerDocument
        {
            if (_documents.TryGetValue(documentId, out var document))
                return document as T;
            return null;
        }

        public void AddDocument(LanguageServerDocument document)
        {
            document.Id = Guid.NewGuid().ToString();
            _documents[document.Id] = document;
        }

        public void RemoveDocument(string documentId)
        {
            if (_documents.ContainsKey(documentId))
                _documents.Remove(documentId);
        }

    }
}
