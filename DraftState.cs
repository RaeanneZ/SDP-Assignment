using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class DraftState : DocState
    {
        private Document doc;

        public DraftState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.Collaborators.Add(collaborator);
        }

        public void submit()
        {
            doc.SetState(doc.ReviewState);
            doc.NotifyObservers("Document '" + doc.Title + "' submitted for approval.");
        }

        public void setApprover(User collaborator)
        {
            if (collaborator == null)
            {
                doc.Approver = null;  
                Console.WriteLine("Approver has been removed.");
                return;
            }

            doc.Approver = collaborator;
        }

        public void approve()
        {
            Console.WriteLine("Cannot approve a draft document.");
        }

        public void reject(string reason)
        {
            Console.WriteLine("Cannot reject a draft document.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Cannot push back a draft document.");
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
                    break;

                case "remove":
                    if (lineNumber >= 0 && lineNumber < section.Count)
                    {
                        section.RemoveAt(lineNumber);
                        Console.WriteLine("Line deleted.");
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
