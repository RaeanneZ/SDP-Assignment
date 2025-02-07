using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class ApprovedState : DocState
    {
        private Document doc;

        public ApprovedState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            Console.WriteLine("Cannot add collaborators to an approved document.");
        }

        public void submit() 
        {
            Console.WriteLine("Document is already approved.");
        }

        public void setApprover(User collaborator)
        {
            Console.WriteLine("Document is already approved.");
        }

        public void approve()
        {
            Console.WriteLine("Document is already approved.");
        }

        public void reject()
        {
            Console.WriteLine("Cannot reject an approved document.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Cannot push back an approved document.");
        }

        public void edit(List<string> section, User collaborator, string action, string text = "", int lineNumber = -1)
        {
            Console.WriteLine("Cannot edit an approved document.");
        }
    }

}
