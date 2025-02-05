using System.Reflection.Metadata;

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
            Console.WriteLine("==== Your Documents ====");

            foreach (var doc in documents)
            {
                if (doc.IsAssociatedWithUser(loggedInUser))
                {
                    Console.WriteLine($"- {doc.Title} [State: {doc.CurrentStateName}]");
                    DocumentActions(doc);
                }
            }

            Console.WriteLine("Press Enter to return to the menu.");
            Console.ReadLine();
        }

        static void DocumentActions(Document document)
        {
            Console.WriteLine($"Document: {document.Title}");
            while (true)
            {
                Console.WriteLine("1. Edit Document");
                Console.WriteLine("2. Add Collaborator");
                Console.WriteLine("3. Set Approver");
                Console.WriteLine("4. Submit Document");
                Console.WriteLine("5. Convert Document");
                Console.WriteLine("6. Undo");
                Console.WriteLine("7. Redo");
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
                        document.UndoLastCommand();
                        break;
                    case "7":
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
            Console.Clear();
            Console.WriteLine("==== Editing Document ====");
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

            Console.WriteLine("Which part would you like to edit?");
            Console.WriteLine("1. Header");
            Console.WriteLine("2. Content");
            Console.WriteLine("3. Footer");
            Console.WriteLine("4. Cancel");
            Console.Write("Enter choice: ");

            var choice = Console.ReadLine();
            Console.Write("Enter new text: ");
            var newText = Console.ReadLine();

            List<string> header = document.GetHeader();
            List<string> content = document.GetContent();
            List<string> footer = document.GetFooter();

            switch (choice)
            {
                case "1":
                    document.Edit(header, newText, loggedInUser);
                    break;
                case "2":
                    document.Edit(content, newText, loggedInUser);
                    break;
                case "3":
                    document.Edit(footer, newText, loggedInUser);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
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
            Console.WriteLine("Choose format to convert:");
            Console.WriteLine("1. PDF");
            Console.WriteLine("2. Word");
            Console.Write("Enter choice: ");
            var choice = Console.ReadLine();

            IFormatConverter converter = choice switch
            {
                "1" => new PDFConverter(),
                "2" => new WordConverter(),
                _ => null
            };

            if (converter == null)
            {
                Console.WriteLine("Invalid choice. Press Enter to return to the menu.");
                Console.ReadLine();
                return;
            }

            //converter.Convert(document.GetContent());
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }
    }
}