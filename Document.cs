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

        // Collaborators list now stores UserComponent (User or UserGroup)
        private List<UserComponent> collaborators = new List<UserComponent>();
        

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

        public List<UserComponent> Collaborators
        {
            get { return collaborators; }
        }
        public Document(string title, User owner)
        {
            this.title = title;
            this.owner = owner;
            content = new List<string>();
            header = new List<string>();
            footer = new List<string>();

            collaborators = new List<UserComponent>(); // UPDATED

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
            AddVersion();
            Console.WriteLine("Finished editing. Version has been added to version history.");
        }

        public void AddCollaborator(UserComponent userComponent) // UPDATED
        {
            if (IsOwnerOrCollaborator(userComponent)) // UPDATED
            {
                Console.WriteLine($"{userComponent.Name} is already a collaborator.");
                return;
            }

            if (userComponent is User user && approver == user) // Ensures approver is not added
            {
                Console.WriteLine("Approver cannot be added as a collaborator!");
                return;
            }

            Collaborators.Add(userComponent);
            RegisterObserver(userComponent); // Works for both User and UserGroup
            NotifyObservers($"Collaborator '{userComponent.Name}' added to document '{Title}'.");
        }

        public void RemoveCollaborator(UserComponent userComponent) // UPDATED
        {
            if (!Collaborators.Contains(userComponent))
            {
                Console.WriteLine($"{userComponent.Name} is not a collaborator.");
                return;
            }

            Collaborators.Remove(userComponent);
            RemoveObserver(userComponent);
            NotifyObservers($"Collaborator '{userComponent.Name}' removed from document '{Title}'.");
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

        public bool IsOwnerOrCollaborator(UserComponent userComponent) // UPDATED
        {
            if (userComponent is User user)
            {
                return owner == user || Collaborators.Contains(user);
            }

            if (userComponent is UserGroup group)
            {
                return Collaborators.Contains(group) || group.GetUsers().Exists(member => IsOwnerOrCollaborator(member));
            }

            return false;
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
        
        
        // Observer Pattern -------------------------------------------------------------------------------------------
        public void RegisterObserver(Observer observer) // UPDATED
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void RemoveObserver(Observer observer) // UPDATED
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(string message) // UPDATED
        {
            foreach (UserComponent observer in observers)
            {
                observer.Notify(message); // Works for both User and UserGroup
            }
        }

        // Command Pattern --------------------------------------------------------------------------------------------
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
