using System;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace SDP_Assignment
{
    class Program
    {
        static Dictionary<string, User> users = new Dictionary<string, User>();
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
                Console.WriteLine("3. Logout");
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
            var document = factory.CreateDocument(title, loggedInUser); // Ensure loggedInUser is not null
            documents.Add(document);
            Console.WriteLine("Document created successfully. Press Enter to return to the menu.");
            Console.ReadLine();
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
                    string docType = documents[i].GetType().Name.Replace("Document", ""); // extracts "Technical Report" or "Grant Proposal"
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
                        if (selectedDocument.Approver == loggedInUser)
                        {
                            ApproverActions(selectedDocument);
                            return;
                        }
                        else
                        {
                            Console.WriteLine($"- {selectedDocument.Title} [State: {selectedDocument.CurrentStateName}]");
                            DocumentActions(selectedDocument);
                            return;
                        }
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

        static void ApproverActions(Document document)
        {
            Console.Clear();
            DisplayDocument(document);

            Console.WriteLine("Select action: ");
            Console.WriteLine("1. Approve");
            Console.WriteLine("2. Push back");
            Console.WriteLine("3. Reject");
            Console.WriteLine("4. Cancel");
            Console.Write("Enter choice: ");
            var x = Console.ReadLine();

            switch (x)
            {
                case "1":
                    document.Approve();
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    break;
                case "2":
                    Console.Write("Enter a comment to push back with: ");
                    string comment = Console.ReadLine();
                    document.PushBack(comment);
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine("Enter reason for rejection: ");
                    string reason = Console.ReadLine();
                    document.Reject(reason);
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void DocumentActions(Document document)
        {
            Console.Clear();
            string docType = document.GetType().Name.Replace("Document", "");

            Console.WriteLine($"==== {docType}: {document.Title} ====");

            while (true)
            {
                Console.WriteLine("1. Edit Document");
                Console.WriteLine("2. Add Collaborator");
                Console.WriteLine("3. Set Approver");
                Console.WriteLine("4. Submit Document");
                Console.WriteLine("5. Convert Document");
                Console.WriteLine("6. View Version History");
                Console.WriteLine("7. Undo");
                Console.WriteLine("8. Redo");
                Console.WriteLine("0. Back");
                Console.Write("Choose an action: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        EditDocument(document);
                        break;
                    case "2":
                        AddCollaborator(document);
                        break;
                    case "3":
                        SetApprover(document);
                        break;
                    case "4":
                        SubmitDocument(document);
                        break;
                    case "5":
                        ConvertDocument(document);
                        break;
                    case "6":
                        ShowVersionHistoryMenu(document);
                        break;
                    case "7":
                        document.UndoLastCommand();
                        break;
                    case "8":
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

        static void EditDocument(Document document)
        {
            if (document.getState() == document.ReviewState)
            {
                Console.WriteLine("Document cannot be edited while in review state!");
                Console.WriteLine();
                return;
            }
            if (document.getState() == document.ApprovedState)
            {
                Console.WriteLine("Document cannot be edited after being approved!");
                Console.WriteLine();
                return;
            }
            Console.Clear();
            DisplayDocument(document);


            Console.WriteLine("Which part would you like to edit?");
            Console.WriteLine("1. Header");
            Console.WriteLine("2. Content");
            Console.WriteLine("3. Footer");
            Console.WriteLine("4. Cancel");
            Console.Write("Enter choice: ");

            var choice = Console.ReadLine();

            List<string> header = document.GetHeader();
            List<string> content = document.GetContent();
            List<string> footer = document.GetFooter();

            switch (choice)
            {
                case "1":
                    DisplayEditMenu(document, header, loggedInUser);
                    break;
                case "2":
                    DisplayEditMenu(document, content, loggedInUser);
                    break;
                case "3":
                    DisplayEditMenu(document, footer, loggedInUser);
                    break;
                case "4":
                    Console.WriteLine();
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }

        static void ShowVersionHistoryMenu(Document document)
        {
            Console.Clear();
            Console.WriteLine($"==== Version History: {document.Title} ====");

            var versions = document.GetVersions();
            if (versions.Count == 0)
            {
                Console.WriteLine("No versions available.");
                Console.WriteLine("Press Enter to return.");
                Console.ReadLine();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Document: {document.Title}");
                Console.WriteLine("Select a version to view:");

                for (int i = 0; i < versions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Version at {versions[i].Timestamp}");
                }
                Console.WriteLine("0. Back");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 0)
                        return;

                    if (choice > 0 && choice <= versions.Count)
                    {
                        var version = versions[choice - 1];
                        Console.Clear();
                        Console.WriteLine($"==== Viewing Version: {version.Timestamp} ====");
                        Console.WriteLine("Header:");
                        foreach (var line in version.Header) Console.WriteLine(line);
                        Console.WriteLine("\nContent:");
                        foreach (var line in version.Content) Console.WriteLine(line);
                        Console.WriteLine("\nFooter:");
                        foreach (var line in version.Footer) Console.WriteLine(line);
                        Console.WriteLine("\nPress Enter to go back.");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
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

        static void DisplayDocument(Document document)
        {
            Console.WriteLine($"Title: {document.Title}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Header:");
            
            foreach (string i in document.GetHeader())
            {
                Console.WriteLine(i);
            }
                
            Console.WriteLine("--------------------------------------------------"); 
            Console.WriteLine("Content:");
                
            foreach (string i in document.GetContent())
            {
                Console.WriteLine(i);
            }
               
            Console.WriteLine("--------------------------------------------------"); 
            Console.WriteLine("Footer:");
                
            foreach (string i in document.GetFooter())    
            {     
                Console.WriteLine(i); 
            }
                
            Console.WriteLine("--------------------------------------------------");
        }

        static void DisplayEditMenu(Document document, List<string> section, User user)
        {
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
                        document.Edit(section, user, "add", newText);
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
                                document.Edit(section, user, "remove", lineNumber: deleteIndex);
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
                                document.Edit(section, user, "replace", replaceText, replaceIndex);
                            }
                            else
                            {
                                Console.WriteLine("Invalid line number.");
                            }
                            break;

                        case "4":
                            Console.WriteLine("Editing complete.");
                            document.FinishEditing();
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please select again.");
                            break;
                    }
                }
            }

        static void AddCollaborator(Document document)
        {
            Console.Write("Enter collaborator username: ");
            var username = Console.ReadLine();

            if (users.ContainsKey(username))
            {
                document.AddCollaborator(users[username]);
                Console.WriteLine("Press Enter to continue.");
            }
            else
            {
                Console.WriteLine("User not found. Press Enter to try again.");
            }
            Console.ReadLine();
        }

        static void SetApprover(Document document)
        {
            Console.Write("Enter approver username: ");
            var username = Console.ReadLine();

            if (!users.ContainsKey(username))
            {
                Console.WriteLine("User not found. Please enter a valid username.");
                Console.ReadLine();
                return;
            }

            var user = users[username];
            document.SetApprover(user);
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }

        static void SubmitDocument(Document document)
        {
            document.SubmitForApproval(loggedInUser);
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }

        static void ConvertDocument(Document document)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"==== Convert Document: {document.Title} ====");
                Console.WriteLine("Choose format to convert:");
                Console.WriteLine("1. PDF");
                Console.WriteLine("2. Word");
                Console.WriteLine("3. Cancel");
                Console.Write("Enter choice: ");

                var choice = Console.ReadLine();

                if (choice == "3" || string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine("Conversion canceled. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                IFormatConverter converter = choice switch
                {
                    "1" => new PDFConverter(),
                    "2" => new WordConverter(),
                    _ => null
                };

                if (converter == null)
                {
                    Console.WriteLine("Invalid choice. Press Enter to try again.");
                    Console.ReadLine();
                    continue;
                }

                //converter.Convert(document.GetContent());
                Console.WriteLine("Document converted successfully. Press Enter to continue.");
                Console.ReadLine();
                return;
            }
        }

    }
}