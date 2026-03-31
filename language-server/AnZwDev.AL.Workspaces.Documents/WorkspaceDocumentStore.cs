using AnZwDev.System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AnZwDev.AL.Workspaces.Documents
{
    public class WorkspaceDocumentStore<T> where T : WorkspaceDocument
    {

        private Dictionary<int, T> _documentByUid = new Dictionary<int, T>();
        private int _lastDocumentUid = 0;

        public WorkspaceDocumentStore()
        {
        }

        public T? Get(int documentUid)
        {
            if (_documentByUid.ContainsKey(documentUid))
                return _documentByUid[documentUid];
            return null;
        }

        public int Add(T document)
        {
            _lastDocumentUid++;
            document.Uid = _lastDocumentUid;
            _documentByUid[document.Uid] = document;
            return document.Uid;
        }

        public void Remove(int documentUid)
        {
            _documentByUid.Remove(documentUid);
        }

        public void Remove(T document)
        {
            Remove(document.Uid);
        }

    }
}
