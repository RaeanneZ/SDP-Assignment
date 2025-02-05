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
        private bool isEdited = false;

        public RejectedState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.Collaborators.Add(collaborator);
        }

        public void submit()
        {
            Console.WriteLine("Cannot submit a rejected document.");
        }

        public void setApprover(User collaborator)
        {
            if (doc.IsOwnerOrCollaborator(collaborator))
            {
                Console.WriteLine("Error: Approver cannot be the owner or a collaborator.");
                return;
            }

            doc.Approver = collaborator;
            doc.NotifyObservers($"Approver assigned: {collaborator.Name} for document '{doc.Title}'.");
        }

        public void approve()
        {
            Console.WriteLine("Cannot approve a rejected document.");
        }

        public void reject()
        {
            Console.WriteLine("Document is already rejected.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Cannot push back a rejected document.");
        }

        public void resubmit()
        {
            if (!isEdited)
            {
                Console.WriteLine("Document must be edited first!");
                return;
            }

            Console.WriteLine("Document resubmitted for review.");
            doc.SetState(doc.ReviewState);
        }

        public void edit(List<string> content, string newContent, User collaborator)
        {
            content.Add(newContent);
            isEdited = true;
        }
    }
}
