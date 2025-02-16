﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class RejectedState : DocState
    {
        private Document doc;
        private bool isEdited = false;

        public RejectedState(Document document)
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
            Console.WriteLine("Cannot approve a rejected document.");
        }

        public void reject(string reason)
        {
            Console.WriteLine("Document is already rejected.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Cannot push back a rejected document.");
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
