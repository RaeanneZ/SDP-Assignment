using System;
using System.Collections.Generic;

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
        private string header;
        private string content;
        private string footer;

        // Document states
        private DocState state;
        private readonly DocState draftState;
        private readonly DocState reviewState;
        private readonly DocState approvedState;
        private readonly DocState rejectedState;
        private readonly DocState reviseState;

        public List<User> Collaborators { get; private set; }

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

        public string GetHeader()
        {
            return header; 
        }
        public string GetContent()
        {
            return content; 
        }
        public string GetFooter()
        {
            return footer;
        }

        public Document(string title, User owner)
        {
            this.title = title;
            this.owner = owner;
            content = "";

            Collaborators = new List<User>();

            // Initialize states
            draftState = new DraftState(this);
            reviewState = new ReviewState(this);
            approvedState = new ApprovedState(this);
            rejectedState = new RejectedState(this);
            reviseState = new ReviseState(this);

            state = draftState;
        }

    // This is for setting and getting the header, content and footer


        public void AddCollaborator(User user)
        {
            if (user == owner)
            {
                Console.WriteLine("Owner is already a collaborator.");
                return;
            }

            if (!Collaborators.Contains(user))
            {
                Collaborators.Add(user);
                RegisterObserver(user);
                NotifyObservers(user.Name + " added as a collaborator to document '" + Title + "'.");
            }
        }

        public void Edit(string newContent, User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                content = newContent;
                NotifyObservers("Document '" + Title + "' was edited by " + user.Name + ".");
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }

        public void SubmitForApproval(User user)
        {
            if (user != owner)
            {
                Console.WriteLine("Only the owner can submit for approval.");
                return;
            }

            NotifyObservers("Document '" + Title + "' submitted for approval by " + user.Name + ".");
            state.submit(user);
        }

        public void SetApprover(User user)
        {
            if (user == owner || Collaborators.Contains(user))
            {
                Console.WriteLine("Error: Approver cannot be the owner or a collaborator.");
                return;
            }

            approver = user;
            RegisterObserver(user);
            NotifyObservers("Approver assigned: " + user.Name + " for document '" + Title + "'.");
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

        public void Reject()
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
            state.reject();
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

        public void Resubmit()
        {
            if (state == reviseState || state == rejectedState)
            {
                state.resubmit();
            }
            else
            {
                Console.WriteLine("Only rejected or revised documents can be resubmitted.");
            }
        }

        public void ConvertDocument()
        {
            if (formatConverter == null)
            {
                Console.WriteLine("No format converter set.");
                return;
            }
            formatConverter.Convert(content);
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


        // Get and set the content
        public void SetHeader(string newHeader, User user)
        {
            if (IsOwnerOrCollaborator(user))
            {
                header = newHeader;
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
                content = newContent;
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
                footer = newFooter;
                NotifyObservers($"Document '{Title}' footer updated by {user.Name}.");
            }
            else
            {
                Console.WriteLine("Only owner or collaborators can edit.");
            }
        }
    }
}



