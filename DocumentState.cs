using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public interface DocState
    {
        public void add(User collaborator);
        public void submit(User collaborator);
        public void approve();
        public void reject();
        public void pushBack(string comment);
        public void resubmit();
        public void edit(User collaborator);
    }

    class DraftState : DocState
    {
        private Document doc;

        public DraftState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.AddCollaborator(collaborator);
        }

        public void submit(User collaborator)
        {
            Console.WriteLine("Document submitted for review.");
            doc.SetState(doc.ReviewState);
        }
        public void approve() => Console.WriteLine("Cannot approve a draft document.");
        public void reject() => Console.WriteLine("Cannot reject a draft document.");
        public void pushBack(string comment) => Console.WriteLine("Cannot push back a draft document.");
        public void resubmit() => Console.WriteLine("Cannot resubmit a draft document.");
        public void edit(User collaborator) => Console.WriteLine("Document edited by collaborator.");
    }

    class ReviewState : DocState
    {
        private Document doc;

        public ReviewState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.AddCollaborator(collaborator);
        }

        public void submit(User collaborator) => Console.WriteLine("Document is already under review.");
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
        public void resubmit() => Console.WriteLine("Document is already under review.");
        public void edit(User collaborator) => Console.WriteLine("Cannot edit document under review.");
    }

    class ApprovedState : DocState
    {
        private Document doc;

        public ApprovedState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator) => Console.WriteLine("Cannot add collaborators to an approved document.");
        public void submit(User collaborator) => Console.WriteLine("Document is already approved.");
        public void approve() => Console.WriteLine("Document is already approved.");
        public void reject() => Console.WriteLine("Cannot reject an approved document.");
        public void pushBack(string comment) => Console.WriteLine("Cannot push back an approved document.");
        public void resubmit() => Console.WriteLine("Cannot resubmit an approved document.");
        public void edit(User collaborator) => Console.WriteLine("Cannot edit an approved document.");
    }

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
        public void edit(User collaborator) => Console.WriteLine("Document edited by collaborator.");
    }

    class ReviseState : DocState
    {
        private Document doc;

        public ReviseState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.AddCollaborator(collaborator);
        }

        public void submit(User collaborator)
        {
            Console.WriteLine("Document resubmitted for review.");
            doc.SetState(doc.ReviewState);
        }
        public void approve() => Console.WriteLine("Cannot approve a document in revision.");
        public void reject() => Console.WriteLine("Cannot reject a document in revision.");
        public void pushBack(string comment) => Console.WriteLine("Document is already in revision.");
        public void resubmit() => Console.WriteLine("Document is already in revision.");
        public void edit(User collaborator) => Console.WriteLine("Document edited by collaborator.");
    }
}
