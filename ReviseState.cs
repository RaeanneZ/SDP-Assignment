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

        public void edit(List<string> section, User collaborator)
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

            if (section.Count == 1)
            {
                section.Clear();
            }

            while (true)
            {
                Console.WriteLine("\nChoose an edit option:");
                Console.WriteLine("1. Add a new line");
                Console.WriteLine("2. Delete a line");
                Console.WriteLine("3. Replace a line");
                Console.WriteLine("4. Finish editing");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter text to add: ");
                        string newText = Console.ReadLine();
                        section.Add(newText);
                        Console.WriteLine("Line added.");
                        isEdited = true;
                        break;

                    case "2":
                        Console.WriteLine();
                        Console.WriteLine("Current content:");
                        for (int i = 0; i < section.Count; i++)
                        {
                            Console.WriteLine($"{i}: {section[i]}");
                        }

                        Console.Write("Enter the line number to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteIndex) && deleteIndex >= 0 && deleteIndex < section.Count)
                        {
                            section.RemoveAt(deleteIndex);
                            Console.WriteLine("Line deleted.");
                            isEdited = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid line number.");
                        }
                        break;

                    case "3":
                        Console.WriteLine();
                        Console.WriteLine("Current content:");
                        for (int i = 0; i < section.Count; i++)
                        {
                            Console.WriteLine($"{i}: {section[i]}");
                        }

                        Console.Write("Enter the line number to replace: ");
                        if (int.TryParse(Console.ReadLine(), out int replaceIndex) && replaceIndex >= 0 && replaceIndex < section.Count)
                        {
                            Console.Write("Enter new text: ");
                            string replaceText = Console.ReadLine();
                            section[replaceIndex] = replaceText;
                            Console.WriteLine("Line replaced.");
                            isEdited = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid line number.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Editing complete.");
                        Console.WriteLine();
                        doc.NotifyObservers($"Document '{doc.Title}' was edited by {collaborator.Name}.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please select again.");
                        break;
                }
            }
        }
    }
}
