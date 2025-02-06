using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class DraftState : DocState
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

        public void reject()
        {
            Console.WriteLine("Cannot reject a draft document.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Cannot push back a draft document.");
        }

        public void resubmit()
        {
            Console.WriteLine("Cannot resubmit a draft document.");
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

                switch (choice)
                {
                    case "1": 
                        Console.Write("Enter text to add: ");
                        string newText = Console.ReadLine();
                        section.Add(newText);
                        Console.WriteLine("Line added.");
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
                        }
                        else
                        {
                            Console.WriteLine("Invalid line number.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Editing complete.");
                        doc.NotifyObservers($"Document '{doc.Title}' was edited by {collaborator.Name}.");
                        Console.WriteLine();
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please select again.");
                        break;
                }
            }
        }

    }

}
