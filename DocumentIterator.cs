using System.Collections.Generic;

namespace SDP_Assignment
{
    public class DocumentIterator : IDocumentIterator
    {
        private List<Document> documents;
        private int position = 0;

        public DocumentIterator(List<Document> documents)
        {
            this.documents = documents;
        }

        public bool HasNext()
        {
            return position < documents.Count;
        }

        public Document Next()
        {
            return HasNext() ? documents[position++] : null;
        }
    }
}