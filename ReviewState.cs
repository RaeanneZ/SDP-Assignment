using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SDP_Assignment
{
    public class ReviewState : DocState
    {
        private Document doc;

        public ReviewState(Document document)
        {
            doc = document;
        }

        public void add(UserComponent collaborator)
        {
            Console.WriteLine($"Select access level for {collaborator.Name}:");
            Console.WriteLine("1. Read-Only");
            Console.WriteLine("2. Read & Write");
            Console.Write("Enter choice: ");
            string accessChoice = Console.ReadLine();

            AccessLevel accessLevel = accessChoice switch
            {
                "1" => AccessLevel.ReadOnly,
                "2" => AccessLevel.ReadWrite,
                _ => AccessLevel.ReadOnly
            };

            doc.AddCollaborator(collaborator, accessLevel);

            //doc.NotifyObservers($"{collaborator.Name} has been added as collaborator.");
            //doc.Collaborators.Add(collaborator);
            //doc.RegisterObserver(collaborator);
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
            doc.NotifyObservers("Document approved.");
            doc.SetState(doc.ApprovedState);
        }

        public void reject(string reason)
        {
            doc.NotifyObservers("Document rejected. Reason: " + reason);
            doc.SetState(doc.RejectedState);
        }

        public void pushBack(string comment)
        {
            doc.NotifyObservers("Document needs revision: " + comment);
            doc.SetState(doc.ReviseState);
        }

        public void edit(List<string> section, User collaborator, string action, string text = "", int lineNumber = -1)
        {
            Console.WriteLine("Cannot edit document under review.");
        }
    }

}
