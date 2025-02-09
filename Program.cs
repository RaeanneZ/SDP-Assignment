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
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            UserGroup newGroup = new UserGroup(groupName);
            groups[groupName] = newGroup;
            Console.WriteLine($"Group '{groupName}' created successfully.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        static void ManageGroups()
        {
            printGroups();

            Console.Write("Enter group name: ");
            string groupName = Console.ReadLine();

            if (!groups.ContainsKey(groupName))
            {
                Console.WriteLine("Group not found.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            UserGroup group = groups[groupName];

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Managing Group: {groupName}");
                Console.WriteLine("1. Add Member");
                Console.WriteLine("2. Remove Member");
                Console.WriteLine("3. View Members");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter username to add: ");
                        string addUsername = Console.ReadLine();
                        if (users.ContainsKey(addUsername))
                        {
                            group.Add(users[addUsername]);
                            Console.WriteLine($"User '{addUsername}' added to group '{groupName}'.");
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;

                    case "2":
                        Console.Write("Enter username to remove: ");
                        string removeUsername = Console.ReadLine();
                        if (users.ContainsKey(removeUsername))
                        {
                            group.Remove(users[removeUsername]);
                            Console.WriteLine($"User '{removeUsername}' removed from group '{groupName}'.");
                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;

                    case "3":
                        Console.WriteLine($"Members of '{groupName}':");
                        var members = group.GetUsers();
                        if (members.Count > 0)
                        {
                            foreach (var member in members)
                            {
                                Console.WriteLine($"- {member.Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No members in this group.");
                        }
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ViewDocumentCollaborators(Document document)
        {
            Console.Clear();
            document.ViewCollaborators();
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }

        static void ViewGroups()
        {
            Console.Clear();
            printGroups();

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        static void printGroups()
        {
            Console.WriteLine("=== List of Groups ===");

            if (groups.Count == 0)
            {
                Console.WriteLine("No groups have been created.");
            }
            else
            {
                foreach (var groupEntry in groups)
                {
                    string groupName = groupEntry.Key;
                    UserGroup group = groupEntry.Value;
                    List<UserComponent> members = group.GetUsers();

                    Console.Write($"{groupName} - ");
                    if (members.Count > 0)
                    {
                        Console.WriteLine(string.Join(", ", members.ConvertAll(m => m.Name)));
                    }
                    else
                    {
                        Console.WriteLine("No members");
                    }
                }
            }
        }

        static void ViewDocuments()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== View Documents ===");
                Console.WriteLine("1. List all documents owned by you");
                Console.WriteLine("2. List all documents accessible to you");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select Option: ");

                string decision = Console.ReadLine();
                Console.WriteLine();

                switch (decision)
                {
                    case "1":
                        DisplayOwnedDocuments(documents);
                        break;
                    case "2":
                        DisplayAccessibleDocuments(documents);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again.");
                        Console.ReadLine();
                        continue;

                }

            
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
                        }
                        else
                        {
                            DocumentActions(selectedDocument);
                        }
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
            

                // Iterate over a COPY of the list to avoid modification issues
                foreach (var doc in documents.ToList()) // <-- Add .ToList()
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
                Console.WriteLine("3. View Collaborators");
                Console.WriteLine("4. Set Approver");
                Console.WriteLine("5. Submit Document");
                Console.WriteLine("6. Convert Document");
                Console.WriteLine("7. View Version History");
                Console.WriteLine("8. Undo");
                Console.WriteLine("9. Redo");
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
                        ViewDocumentCollaborators(document);
                        break;
                    case "4":
                        SetApprover(document);
                        break;
                    case "5":
                        SubmitDocument(document);
                        break;
                    case "6":
                        ConvertDocument(document);
                        break;
                    case "7":
                        ShowVersionHistoryMenu(document);
                        break;
                    case "8":
                        document.UndoLastCommand();
                        break;
                    case "9":
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
            if (loggedInUser != document.Owner)
            {
                Console.WriteLine("Only the document owner can add collaborators.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter collaborator username or group name: ");
            string name = Console.ReadLine();

            UserComponent collaborator;
            if (users.ContainsKey(name))
            {
                collaborator = users[name];
            }
            else if (groups.ContainsKey(name))
            {
                collaborator = groups[name];
            }
            else
            {
                Console.WriteLine("User or Group not found.");
                Console.ReadLine();
                return;
            }

            document.AddCollaborator(collaborator);
        }

        static void SetApprover(Document document)
        {
            Console.Write("Enter approver username: ");
            var username = Console.ReadLine();

            if (!users.ContainsKey(username))
            {
                Console.WriteLine("User not found. Please enter a valid username.");
                Console.ReadLine ();
                return;
            }

            var user = users[username];

            document.SetApprover(user);
        }

        static void SubmitDocument(Document document)
        {
            if (loggedInUser != document.Owner)
            {
                Console.WriteLine("Only the document owner can submit the document for approval.");
                Console.ReadLine();
                return;
            }

            // Delegate to the state pattern
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
              
                // Set the converter
                document.SetFormatConverter(converter);

                // Perform the conversion
                Document convertedDocument = document.ConvertDocument();
                documents.Add(convertedDocument);

                Console.WriteLine("Document converted. Press Enter to continue.");
                Console.ReadLine();
            }
        }

        static void DisplayOwnedDocuments(List<Document> documents)
        {
            int x = 0;
            Console.WriteLine("=== Your documents ===");
            for (int i = 0; i < documents.Count; i++)
            {
                Document doc = documents[i];
                if (doc.Owner == loggedInUser)
                {
                    string docType = documents[i].GetType().Name.Replace("Document", "");
                    Console.WriteLine($"{i + 1}. [{docType}] {documents[i].Title} [State: {documents[i].CurrentStateName}]");
                    x++;
                }
                if (x == 0)
                {
                    Console.WriteLine("No documents owned!");
                }

            }
        }

        static void DisplayAccessibleDocuments(List<Document> documents)
        {
            int x = 0;
            Console.WriteLine("=== Your documents ===");

            foreach (var doc in documents)
            {
                // Check if the user is the owner, approver, or a direct collaborator
                if (doc.Owner == loggedInUser || doc.Approver == loggedInUser)
                {
                    string docType = doc.GetType().Name.Replace("Document", "");
                    Console.WriteLine($"{++x}. [{docType}] {doc.Title} [State: {doc.CurrentStateName}]");
                    continue;
                }

                // Check if the logged-in user is part of a group stored in collaborators
                bool isGroupMember = false;

                foreach (var collaborator in doc.Collaborators)
                {
                    if (collaborator is UserGroup group)
                    {
                        // Check if the user is part of the group
                        if (group.GetUsers().Contains(loggedInUser))
                        {
                            isGroupMember = true;
                            break;
                        }
                    }
                    else if (collaborator == loggedInUser)
                    {
                        // Check if the user is directly in the collaborators list
                        isGroupMember = true;
                        break;
                    }
                }

                if (isGroupMember)
                {
                    string docType = doc.GetType().Name.Replace("Document", "");
                    Console.WriteLine($"{++x}. [{docType}] {doc.Title} [State: {doc.CurrentStateName}]");
                }
            }

            if (x == 0)
            {
                Console.WriteLine("No documents accessible!");
            }
        }
    }
}
