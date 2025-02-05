using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class ReviseState : DocState
    {
        private Document doc;
        private bool isEdited = false;
        public ReviseState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.Collaborators.Add(collaborator);
        }

        public void submit()
        {
            if (!isEdited)
            {
                Console.WriteLine("Document must be edited first!");
                return;
            }
            Console.WriteLine("Document resubmitted for review.");
            doc.SetState(doc.ReviewState);
        }

        public void setApprover(User collaborator)
        {
            Console.WriteLine("New approver cannot be chosen at this time!");
        }

        public void approve()
        {
            Console.WriteLine("Cannot approve a document in revision.");
        }

        public void reject()
        {
            Console.WriteLine("Cannot reject a document in revision.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Document is already in revision.");
        }

        public void resubmit()
        {
            Console.WriteLine("Document is already in revision.");
        }

        public void edit(List<string> content, string newContent, User collaborator)
        {
            content.Add(newContent);
            isEdited = true;
        }
    }
}
