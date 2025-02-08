using System.Collections.Generic;

namespace SDP_Assignment
{
    public class DocumentVersionHistory : VersionAggregate
    {
        private List<DocumentVersion> versions = new List<DocumentVersion>();

        public void AddVersion(DocumentVersion version)
        {
            versions.Add(version);
        }

        public override IDocumentVersionIterator CreateIterator()
        {
            return new DocumentVersionIterator(this);
        }

        public List<DocumentVersion> GetVersions()
        {
            return versions;
        }
    }
}