namespace SDP_Assignment
{
    public class DocumentVersionIterator : IDocumentVersionIterator
    {
        private DocumentVersionHistory history;
        private int position = 0;

        public DocumentVersionIterator(DocumentVersionHistory history)
        {
            this.history = history;
        }

        public override bool HasNext()
        {
            return position < history.GetVersions().Count;
        }

        public override DocumentVersion Next()
        {
            return HasNext() ? history.GetVersions()[position++] : null;
        }
    }
}