using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class RejectedState : DocState
    {
        private Document doc;

        public RejectedState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.AddCollaborator(collaborator);
        }

        public void submit(User collaborator) => Console.WriteLine("Cannot submit a rejected document.");
        public void approve() => Console.WriteLine("Cannot approve a rejected document.");
        public void reject() => Console.WriteLine("Document is already rejected.");
        public void pushBack(string comment) => Console.WriteLine("Cannot push back a rejected document.");
        public void resubmit()
        {
            Console.WriteLine("Document resubmitted for review.");
            doc.SetState(doc.ReviewState);
        }
        public void edit(List<string> content, string newContent, User collaborator)
        {
            content.Add(newContent);
        }
    }
}
