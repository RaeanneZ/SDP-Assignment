namespace SDP_Assignment
{
	public class DocumentCollection
	{
		private List<Document> documents = new List<Document>();

		public void AddDocument(Document document)
		{
			documents.Add(document);
		}

		public List<Document> GetDocuments()
		{
			return documents;
		}

		public IDocumentIterator CreateIterator()
		{
			return new DocumentIterator(documents);
		}

		public IEnumerable<Document> GetUserDocuments(User user, bool isOwnerCheck = false)
		{
			foreach (var doc in documents)
			{
				if (isOwnerCheck)
				{
					if (doc.Owner == user)
					{
						yield return doc;
					}
				}
				else
				{
					if (doc.Owner == user || doc.Approver == user || doc.Collaborators.Contains(user))
					{
						yield return doc;
					}
				}
			}
		}
	}
}