using System;
using System.Collections.Generic;

namespace SDP_Assignment
{
    class Program
    {
        static Dictionary<string, User> users = new Dictionary<string, User>(); // Stores individual users
        static Dictionary<string, UserGroup> groups = new Dictionary<string, UserGroup>(); // Stores user groups
        static List<Document> documents = new List<Document>();
        static User loggedInUser;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Document Workflow System ====");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void Login()
        {
            Console.Clear();
            Console.Write("Enter your username: ");
            var username = Console.ReadLine();

            if (users.ContainsKey(username))
            {
                loggedInUser = users[username];
                Console.WriteLine($"Welcome back, {loggedInUser.Name}! Press Enter to continue.");
                Console.ReadLine();
                UserMenu();
            }
            else
            {
                Console.WriteLine("User not found. Press Enter to return to the main menu.");
                Console.ReadLine();
            }
        }

        static void Register()
        {
            Console.Clear();
            Console.Write("Enter a username: ");
            var username = Console.ReadLine();

            if (users.ContainsKey(username))
            {
                Console.WriteLine("Username already exists. Press Enter to return to the main menu.");
                Console.ReadLine();
            }
            else
            {
                Console.Write("Enter your full name: ");
                var name = Console.ReadLine();

                var user = new User(name);
                users[username] = user;
                Console.WriteLine("User registered successfully. Press Enter to return to the main menu.");
                Console.ReadLine();
            }
        }

        static void UserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== User Menu ====");
                Console.WriteLine("1. Create Document");
                Console.WriteLine("2. View Documents");
                Console.WriteLine("3. Create Group");
                Console.WriteLine("4. Manage Groups");
                Console.WriteLine("5. Logout");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateDocument();
                        break;
                    case "2":
                        ViewDocuments();
                        break;
                    case "3":
                        CreateGroup();
                        break;
                    case "4":
                        ManageGroups();
                        break;
                    case "5":
                        Console.WriteLine("Logging out. Press Enter to continue.");
                        Console.ReadLine();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void CreateDocument()
        {
            Console.Clear();
            Console.WriteLine("Select Document Type:");
            Console.WriteLine("1. Technical Report");
            Console.WriteLine("2. Grant Proposal");
            Console.Write("Enter choice: ");
            var choice = Console.ReadLine();

            DocumentFactory factory = choice switch
            {
                "1" => new TechnicalReportFactory(),
                "2" => new GrantProposalFactory(),
                _ => null
            };

            if (factory == null)
            {
                Console.WriteLine("Invalid choice. Press Enter to return to the menu.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter the document title: ");
            var title = Console.ReadLine();
            var document = factory.CreateDocument(title, loggedInUser);
            documents.Add(document);
            Console.WriteLine("Document created successfully. Press Enter to return to the menu.");
            Console.ReadLine();
        }

        static void CreateGroup()
        {
            Console.Write("Enter group name: ");
            string groupName = Console.ReadLine();

            if (groups.ContainsKey(groupName))
            {
                Console.WriteLine("Group already exists!");
                return;
            }

            UserGroup newGroup = new UserGroup(groupName);
            groups[groupName] = newGroup;
            Console.WriteLine($"Group '{groupName}' created successfully.");
        }

        static void ManageGroups()
        {
            Console.Write("Enter group name: ");
            string groupName = Console.ReadLine();

            if (!groups.ContainsKey(groupName))
            {
                Console.WriteLine("Group not found.");
                return;
            }

            UserGroup group = groups[groupName];

            Console.WriteLine("1. Add Member");
            Console.WriteLine("2. Remove Member");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            if (!users.ContainsKey(username))
            {
                Console.WriteLine("User not found.");
                return;
            }

            User user = users[username];

            switch (choice)
            {
                case "1":
                    group.Add(user);
                    Console.WriteLine("User added to group.");
                    break;
                case "2":
                    group.Remove(user);
                    Console.WriteLine("User removed from group.");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void ViewDocuments()
        {
            Console.Clear();
            if (documents.Count == 0)
            {
                Console.WriteLine("No documents available.");
                Console.WriteLine("Press Enter to return to the menu.");
                Console.ReadLine();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Your Documents ====");

                for (int i = 0; i < documents.Count; i++)
                {
                    Document doc = documents[i];
                    string docType = documents[i].GetType().Name.Replace("Document", "");
                    Console.WriteLine($"{i + 1}. [{docType}] {documents[i].Title} [State: {documents[i].CurrentStateName}]");
                }

                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select a document (Enter number): ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                        return;

                    if (choice > 0 && choice <= documents.Count)
                    {
                        Document selectedDocument = documents[choice - 1];
                        DocumentActions(selectedDocument);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Press Enter to try again.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Press Enter to try again.");
                    Console.ReadLine();
                }
            }
        }

        static void DocumentActions(Document document)
        {
            Console.Clear();
            string docType = document.GetType().Name.Replace("Document", "");

            Console.WriteLine($"==== {docType}: {document.Title} ====");

            while (true)
            {
                Console.WriteLine("1. Add Collaborator");
                Console.WriteLine("2. Submit Document");
                Console.WriteLine("3. Undo");
                Console.WriteLine("4. Redo");
                Console.WriteLine("0. Back");
                Console.Write("Choose an action: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddCollaborator(document);
                        break;
                    case "2":
                        document.SubmitForApproval(loggedInUser);
                        break;
                    case "3":
                        document.UndoLastCommand();
                        break;
                    case "4":
                        document.RedoLastCommand();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddCollaborator(Document document)
        {
            Console.Write("Enter collaborator username or group name: ");
            string name = Console.ReadLine();

            if (users.ContainsKey(name))
            {
                document.AddCollaborator(users[name]); // Add User
            }
            else if (groups.ContainsKey(name))
            {
                document.AddCollaborator(groups[name]); // Add UserGroup
            }
            else
            {
                Console.WriteLine("User or Group not found.");
            }
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }
    }
}
