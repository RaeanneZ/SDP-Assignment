using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class ReviewState : DocState
    {
        private Document doc;

        public ReviewState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            if (doc.IsOwnerOrCollaborator(collaborator))
            {
                Console.WriteLine("Approver cannot be added as a collaborator!");
                return;
            }
            doc.Collaborators.Add(collaborator);
        }

        public void submit()
        {
            Console.WriteLine("Document is already under review.");
        }

        public void setApprover(User collaborator)
        {
            Console.WriteLine("Document is already under review.");
        }

        public void approve()
        {
            Console.WriteLine("Document approved.");
            doc.SetState(doc.ApprovedState);
        }

        public void reject()
        {
            Console.WriteLine("Document rejected.");
            doc.SetState(doc.RejectedState);
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Document needs revision: " + comment);
            doc.SetState(doc.ReviseState);
        }

        public void resubmit() 
        { 
            Console.WriteLine("Document is already under review."); 
        }

        public void edit(List<string> section, User collaborator)
        {
            Console.WriteLine("Cannot edit document under review.");
        }
    }

}
