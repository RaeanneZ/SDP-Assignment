using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class Document
    {
        private DocState docState;
        private List<User> collaborators = new List<User>();
        private List<IObserver> observerList = new List<IObserver>();
        private IFormatConverter formatConverter;
        private string title;
        private string content;
        private User owner;
        private User approver;

        // doc state
        private DocState draftState;
        private DocState reviewState;
        private DocState approvedState;
        private DocState rejectedState;
        private DocState reviseState;

        private DocState state;

        private bool success = false;

        public DocState DraftState { get { return draftState; } }
        public DocState ReviewState { get { return reviewState; } }
        public DocState ApprovedState { get { return approvedState; } }
        public DocState RejectedState { get { return rejectedState; } }
        public DocState ReviseState { get { return reviseState; } }

        public bool Success { get; set; }

        public Document()
        {
            draftState = new DraftState(this);
            reviewState = new ReviewState(this);
            approvedState = new ApprovedState(this);
            rejectedState = new RejectedState(this);
            reviseState = new ReviseState(this);

            state = draftState;
        }

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

        public Document(string title, User owner)
        {
            this.title = title;
            this.owner = owner;
            content = "";

            draftState = new DraftState(this);
            reviewState = new ReviewState(this);
            approvedState = new ApprovedState(this);
            rejectedState = new RejectedState(this);
            reviseState = new ReviseState(this);

            state = draftState;
        }

        // This is for the document to add more components
        public void AddComponent(string component)
        {
            Content += $"\n\n{component}";
        }


        public void SetState(DocState state)
        {
            this.state = state;
        }

        public void Submit(User user)
        {
            if (!collaborators.Contains(user))
            {
                state.submit(user);
            }
            else
            {
                Console.WriteLine("Approver cannot be a collaborator, please select a new one.");
            }
        }
        public void Approve()
        {
            state.approve();
        }
        public void Reject()
        {
            state.reject();
        }
        public void PushBack(string comment)
        {
            state.pushBack(comment);
        }
        public void Resubmit()
        {
            state.resubmit();
        }
        public void Edit(User collaborator)
        {
            state.edit(collaborator);
        }

        public void AddCollaborator(User collaborator)
        {
            if (!collaborators.Contains(collaborator) || collaborator != owner)
            {
                collaborators.Add(collaborator);
            }
            else
            {
                Console.WriteLine("User is already in collaborators!");
            }
        }

        public void SetApprover(User user)
        {
            if (collaborators.Contains(user) || Owner == user)
            {
                Console.WriteLine("Approver cannot be the owner or a collaborator.");
                return;
            }
            approver = user;
            NotifyObservers($"{user.Name} assigned as the approver.");
        }

        public bool IsOwnerOrCollaborator(User user)
        {
            return owner == user || collaborators.Contains(user);
        }

        public void AddObserver(IObserver observer)
        {
            observerList.Add(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (var observer in observerList)
            {
                observer.Notify(message);
            }
        }

        public string CurrentStateName => docState.GetType().Name;

        public bool IsAssociatedWithUser(User user)
        {
            return owner == user || collaborators.Contains(user) || approver == user;
        }

        public void SetFormatConverter(FormatConverter fc)
        {
            formatConverter = fc;
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
    }
}
