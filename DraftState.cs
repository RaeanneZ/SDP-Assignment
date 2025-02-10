﻿using System;
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

        public DraftState(Document document)
        {
            doc = document;
        }

        public void add(UserComponent collaborator)
        {
            if (!doc.Collaborators.Contains(collaborator))
            {
                doc.NotifyObservers($"{collaborator.Name} has been added as collaborator.");
                doc.Collaborators.Add(collaborator);
                doc.RegisterObserver(collaborator);
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
        }
    }
}
