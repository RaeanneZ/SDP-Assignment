using System;
using System.Collections.Generic;

namespace SDP_Assignment
{
    public class Document : Subject
    {
        private List<Observer> observers = new List<Observer>();
        private IFormatConverter formatConverter;
        private string title;
        private string content;
        private User owner;
        private User approver;

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

        public string Content
        {
            get { return content; }
            set { content = value; }
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
    }
}


