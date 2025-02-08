using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SDP_Assignment
{
    public class Document : Subject
    {
        private List<Observer> observers = new List<Observer>();
        private IFormatConverter formatConverter;
        private string title;
        private User owner;
        private User approver;

        // Document Content
        private List<string> header;
        private List<string> content;
        private List<string> footer;

        // Document states
        private DocState state;
        private readonly DocState draftState;
        private readonly DocState reviewState;
        private readonly DocState approvedState;
        private readonly DocState rejectedState;
        private readonly DocState reviseState;

        // Commands
        private Stack<DocumentCommand> commandHistory = new Stack<DocumentCommand>();
        private Stack<DocumentCommand> redoStack = new Stack<DocumentCommand>(); 

        public List<User> Collaborators { get; private set; }

        private DocumentVersionHistory versionHistory = new DocumentVersionHistory();

        public string Title
        {
            get { return title; }
        }

        public User Owner
        {
            get { return owner; }
        }

        public User Approver
        {
            get { return approver; }
            set { approver = value; }
        }

        public DocState DraftState
        {
            get { return draftState; }
        }

        public DocState ReviewState
        {
            get { return reviewState; }
        }

        public DocState ApprovedState
        {
            get { return approvedState; }
        }

        public DocState RejectedState
        {
            get { return rejectedState; }
        }

        public DocState ReviseState
        {
            get { return reviseState; }
        }

        public DocState getState()
        {
            return state;
        }

        public List<string> GetHeader()
        {
            return header;
        }
        public List<string> GetContent()
        {
            return content;
        }
        public List<string> GetFooter()
        {
            return footer;
        }

        public Document(string title, User owner)
        {
            this.title = title;
            this.owner = owner;
            content = new List<string>();
            header = new List<string>();
            footer = new List<string>();

            Collaborators = new List<User>();

            // Initialize states
            draftState = new DraftState(this);
            reviewState = new ReviewState(this);
            approvedState = new ApprovedState(this);
            rejectedState = new RejectedState(this);
            reviseState = new ReviseState(this);

            state = draftState;
        }

        private void AddVersion()
        {
            string fullContent = string.Join("\n", content);
            string fullHeader = string.Join("\n", header);
            string fullFooter = string.Join("\n", footer);

            versionHistory.AddVersion(new DocumentVersion(fullHeader, fullContent, fullFooter));
        }

        public void FinishEditing()
        {
            //finish editing, add a new version to the version history
            AddVersion();

            Console.WriteLine("Finished editing. Version has been added to version history.");
        }

        public void SetHeader(string newHeader, User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                header.Add(newHeader);
                AddVersion();
                NotifyObservers($"Document '{Title}' header updated by {user.Name}.");
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }

        public void SetContent(string newContent, User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                content.Add(newContent);
                AddVersion();
                NotifyObservers($"Document '{Title}' content updated by {user.Name}.");
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }

        public void SetFooter(string newFooter, User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                footer.Add(newFooter);
                AddVersion();
                NotifyObservers($"Document '{Title}' footer updated by {user.Name}.");
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }

        public void ShowVersionHistory()
        {
            IDocumentVersionIterator iterator = versionHistory.CreateIterator();
            Console.WriteLine($"Version History for {Title}:");

            Console.WriteLine("Current Versions:");
            foreach (var version in versionHistory.GetVersions())
            {
                Console.WriteLine($"- Version at {version.Timestamp}");
            }

            int index = 1;
            while (iterator.HasNext())
            {
                DocumentVersion version = iterator.Next();
                Console.WriteLine($"{index}. Version at {version.Timestamp}:");
                Console.WriteLine($"   Header: {version.Header}");
                Console.WriteLine($"   Content: {version.Content}");
                Console.WriteLine($"   Footer: {version.Footer}");
                index++;
            }
            Console.WriteLine("0. Back");
        }

        // This is for setting and getting the header, content and footer
        public void AddCollaborator(User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                Console.WriteLine("User is already a collaborator.");
                Console.WriteLine();
                return;
            }
            
            if (approver == user) 
            {
                Console.WriteLine("Approver cannot be added as collaborator!");
                Console.WriteLine();
                return;
            }

            ExecuteCommand(new AddCollaboratorCommand(this, user));
            RegisterObserver(user);
            return;
        }

        public void Edit(List<string> section, User user, string action, string text = "", int lineNumber = -1)
        {
            if (IsOwnerOrCollaborator(user))
            {
                ExecuteCommand(new EditDocumentCommand(this, section, user, action, text, lineNumber));
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }

        public void SubmitForApproval(User user)
        {
            if (approver == null)
            {
                Console.WriteLine("Please set an approver first!");
                Console.WriteLine();
                return;
            }

            ExecuteCommand(new SubmitCommand(this, ReviewState));
        }

        public void SetApprover(User user)
        {
            if (state == reviewState || state == reviseState)
            {
                Console.WriteLine("Error: New approver cannot be set at this time.");
                Console.WriteLine();
                return;
            }

            if (IsOwnerOrCollaborator(user))
            {
                Console.WriteLine("Error: Approver cannot be the owner or a collaborator.");
                Console.WriteLine();
                return;
            }

            ExecuteCommand(new SetApproverCommand(this, user));
            RegisterObserver(user);
        }

        public void SetState(DocState newState)
        {
            this.state = newState;
            NotifyObservers("Document '" + Title + "' state changed to: " + newState.GetType().Name);
        }

        public void RegisterObserver(Observer observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void RemoveObserver(Observer observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (Observer observer in observers)
            {
                observer.Notify(message);
            }
        }

        public bool IsOwnerOrCollaborator(User user)
        {
            return owner == user || Collaborators.Contains(user);
        }

        public void Approve()
        {
            if (approver == null)
            {
                Console.WriteLine("No approver assigned.");
                return;
            }
            if (state != reviewState)
            {
                Console.WriteLine("Document must be under review to be approved.");
                return;
            }
            state.approve();
        }

        public void Reject(string reason)
        {
            if (approver == null)
            {
                Console.WriteLine("No approver assigned.");
                return;
            }
            if (state != reviewState)
            {
                Console.WriteLine("Document must be under review to be rejected.");
                return;
            }
            state.reject(reason);
        }

        public void PushBack(string comment)
        {
            if (state == reviewState)
            {
                state.pushBack(comment);
            }
            else
            {
                Console.WriteLine("Document must be in review state to be pushed back.");
            }
        }

        public void ConvertDocument()
        {
            if (formatConverter == null)
            {
                Console.WriteLine("No format converter set.");
                return;
            }
            //formatConverter.Convert(content);
        }

        public void SetFormatConverter(IFormatConverter converter)
        {
            formatConverter = converter;
        }

        public string CurrentStateName
        {
            get { return state.GetType().Name; }
        }

        public bool IsAssociatedWithUser(User user)
        {
            return owner == user || Collaborators.Contains(user) || approver == user;
        }

        public void ExecuteCommand(DocumentCommand command)
        {
            command.Execute();
            commandHistory.Push(command);
        }

        public void UndoLastCommand()
        {
            if (commandHistory.Count > 0)
            {
                DocumentCommand command = commandHistory.Pop();
                command.Undo();
                redoStack.Push(command);
            }
            else
            {
                Console.WriteLine("No command to undo!");
            }
            Console.WriteLine();
        }

        public void RedoLastCommand()
        {
            if (redoStack.Count > 0)
            {
                DocumentCommand command = redoStack.Pop();
                command.Redo();
                commandHistory.Push(command);
            }
            else
            {
                Console.WriteLine("No command to redo!");
            }
            Console.WriteLine();
        }

        public List<DocumentVersion> GetVersions()
        {

            return versionHistory.GetVersions();
        }

    }
}



