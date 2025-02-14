using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class DraftState : DocState
    {
        private Document doc;
        private bool isEdited = false;

        public DraftState(Document document)
        {
            doc = document;
        }

        public void add(UserComponent collaborator)
        {
            if (!doc.Collaborators.Contains(collaborator))
            {
                doc.Collaborators.Add(collaborator);
                doc.RegisterObserver(collaborator);

                if (collaborator is UserGroup group)
                {
                    // Notify each member individually without group-level notification
                    foreach (var member in group.GetUsers())
                    {
                        if (!doc.Collaborators.Contains(member))
                        {
                            doc.Collaborators.Add(member);
                            doc.RegisterObserver(member);
                            member.Notify($"You have been added to the document '{doc.Title}' as part of the group '{group.Name}'.");
                        }
                    }
                }
                else
                {
                    collaborator.Notify($"You have been added to the document '{doc.Title}'.");
                }
            }
            else
            {
                Console.WriteLine("User is already a collaborator!");
            }
        }

        public void submit()
        {
            if (doc.Approver == null)
            {
                Console.WriteLine("Please set an approver first!");
                Console.WriteLine();
                return;
            }

            if (doc.IsPushedBack == true)
            {
                if (isEdited == false)
                {
                    Console.WriteLine("Document must be edited first before it can be resubmitted!");
                    return;
                }
            }

            doc.NotifyObservers("Document '" + doc.Title + "' has been submitted for approval.");
            doc.SetState(doc.ReviewState);
        }

        public void setApprover(User collaborator)
        {
            if (doc.Collaborators.Contains(collaborator) || doc.Owner == collaborator)
            {
                Console.WriteLine("Approver cannot be a collaborator or the owner!");
                return;
            }

            if (collaborator != null)
            {
                doc.NotifyObservers(collaborator + " has been added as approver.");
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
                    if (doc.IsPushedBack == true)
                    {
                        isEdited = true;
                    }
                    Console.WriteLine("Line added.");
                    break;

                case "remove":
                    if (lineNumber >= 0 && lineNumber < section.Count)
                    {
                        section.RemoveAt(lineNumber);
                        if (doc.IsPushedBack == true)
                        {
                            isEdited = true;
                        }
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
                        if (doc.IsPushedBack == true)
                        {
                            isEdited = true;
                        }
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
        }
    }
}
