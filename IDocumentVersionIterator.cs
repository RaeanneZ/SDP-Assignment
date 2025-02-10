namespace SDP_Assignment
{
    public abstract class IDocumentVersionIterator
    {
        public abstract bool HasNext();
        public abstract DocumentVersion Next();
    }
}