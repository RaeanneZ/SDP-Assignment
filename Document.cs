﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
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

            // Initialize states
            draftState = new DraftState(this);
            reviewState = new ReviewState(this);
            approvedState = new ApprovedState(this);
            rejectedState = new RejectedState(this);
            reviseState = new ReviseState(this);

            RegisterObserver(owner);
            state = draftState;
        }

        // State Pattern
        public DocState getState()
        {
            return state;
        }
        public void SetState(DocState newState)
        {
            state = newState;
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

        public void AddCollaborator(UserComponent collaborator)
        {
            if (IsOwnerOrCollaborator(collaborator))
            {
                Console.WriteLine($"{collaborator.Name} is already a collaborator.");
                return;
            }

            if (collaborator is User user && approver == user)
            {
                Console.WriteLine("Approver cannot be added as a collaborator!");
                return;
            }

            if (collaborator is UserGroup group)
            {
                collaborators.Add(group);
                RegisterObserver(group);

            }
            else
            {
                RegisterObserver(collaborator);
            }

            ExecuteCommand(new AddCollaboratorCommand(this, collaborator));
        }


        public void RemoveCollaborator(UserComponent collaborator) // UPDATED
        {
            if (!IsOwnerOrCollaborator(collaborator))
            {
                Console.WriteLine($"{collaborator.Name} is not a collaborator.");
                return;
                //AddVersion();
            }

            if (collaborator is UserGroup group)
            {
                // Remove the group from collaborators
                collaborators.Remove(group);
                RemoveObserver(group);

                // Remove all members of the group from collaborators
                foreach (var member in group.GetUsers())
                {
                    if (collaborators.Contains(member))
                    {
                        collaborators.Remove(member);
                        RemoveObserver(member);
                    }
                }
            }
            else
            {
                Collaborators.Remove(collaborator);
                RemoveObserver(collaborator);
            }
        }

        public void Edit(List<string> section, User user, string action, string text = "", int lineNumber = -1)
        {
            if (IsOwnerOrCollaborator(user))
            {
                ExecuteCommand(new EditDocumentCommand(this, section, user, action, text, lineNumber));
                AddVersion();
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }

        public void SetHeader(string newHeader, User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                header.Add(newHeader);
                AddVersion();
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
            if (user == null)
            {
                Console.WriteLine("Invalid approver.");
                return;
            }

            if (collaborators.Contains(user) || Owner == user)
            {
                Console.WriteLine("Approver cannot be a collaborator or owner.");
                return;
            }

            ExecuteCommand(new SetApproverCommand(this, user));    
        }

        public void ViewCollaborators()
        {
            Console.WriteLine($"=== Collaborators for Document: {Title} ===");
            foreach (var entry in collaborators)
            {
                Console.WriteLine($"{entry.Name}");
            }
        }

        public void EditContent(UserComponent user, string newContent)
        {
            NotifyObservers($"Document '{Title}' content updated by {user.Name}.");
        }

        public bool IsOwnerOrCollaborator(UserComponent userComponent)
        {
            if (userComponent is User user)
            {
                // Check if the user is the owner or directly added as a collaborator
                return owner == user || collaborators.Contains(user);
            }

            if (userComponent is UserGroup group)
            {
                // Check if the group is directly added as a collaborator
                if (collaborators.Contains(group))
                    return true;

                // Check if any member of the group is a collaborator
                foreach (var member in group.GetUsers())
                {
                    if (collaborators.Contains(member))
                        return true;
                }
            }

            return false;
        }


        public void Approve()
        {
            if (state != reviewState)
            {
                Console.WriteLine($"Document '{Title}'  must be under review to be approved.");
                return;
            }

            state.approve();
        }

        public void Reject(string reason)
        {
            if (state != reviewState)
            {
                Console.WriteLine($"Document '{Title}'  must be under review to be rejected.");
                return;
            }
            state.reject(reason);
        }

        public void PushBack(string comment)
        {
            if (state != reviewState)
            {
                Console.WriteLine("Document must be under review to be push back.");
                return;
            }
            state.pushBack(comment);
        }
        public Document ConvertDocument()
        {
            if (formatConverter == null)
            {
                Console.WriteLine("No format converter set.");
                return null;
            }
            Document convertedDocument = formatConverter.Convert(this);
            return convertedDocument;
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
            if (owner == user || approver == user)
            {
                return true;
            }

            foreach (var collaborator in collaborators)
            {
                if (collaborator is User collaboratorUser && collaboratorUser == user)
                {
                    return true;
                }

                if (collaborator is UserGroup group && group.GetUsers().Contains(user))
                {
                    return true;
                }
            }

            return false;
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
