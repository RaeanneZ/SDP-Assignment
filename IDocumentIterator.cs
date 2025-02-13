namespace SDP_Assignment
{
	public interface IDocumentIterator
	{
		bool HasNext();
		Document Next();
	}
}