using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class ReviseState : DocState
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

        public void edit(List<string> section, User collaborator, string action, string text = "", int lineNumber = -1)
        {
            if (!doc.IsOwnerOrCollaborator(collaborator))
            {
                Console.WriteLine("Only owner or collaborators can edit.");
                return;
            }

            if (doc.getState() == doc.ReviewState)
            {
                Console.WriteLine("Document cannot be edited in review state!");
                return;
            }

            switch (action)
            {
                case "add":
                    section.Add(text);
                    Console.WriteLine("Line added.");
                    isEdited = true;
                    break;

                case "remove":
                    if (lineNumber >= 0 && lineNumber < section.Count)
                    {
                        section.RemoveAt(lineNumber);
                        Console.WriteLine("Line deleted.");
                        isEdited = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid line number.");
                    }
                    break;

                case "replace":
                    if (lineNumber >= 0 && lineNumber < section.Count)
                    {
                        section[lineNumber] = text;
                        Console.WriteLine("Line replaced.");
                        isEdited = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid line number.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }

            doc.NotifyObservers($"Document '{doc.Title}' was edited by {collaborator.Name}.");
        }
    }
}
