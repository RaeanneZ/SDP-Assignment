using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

namespace SDP_Assignment
{
    public class Document : DocumentBase, Subject
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
        public bool IsPushedBack;

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

            RegisterObserver(owner);
            state = draftState;
            owner.AddDocument(this);
            IsPushedBack = false;
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

        public override void SetHeader(string newHeader, User user)
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

        public override void SetContent(string newContent, User user)
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

        public override void SetFooter(string newFooter, User user)
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

        public void ViewCollaborators()
        {
            Console.WriteLine($"=== Collaborators for Document: {Title} ===");
            foreach (var entry in collaborators)
            {
                Console.WriteLine($"{entry.Name}");
            }
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

        public List<DocumentVersion> GetVersions()
        {
            return versionHistory.GetVersions();
        }
    }
}
