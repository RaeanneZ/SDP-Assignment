using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class Document
    {
        private DocumentState docState;
        private List<User> collaborators = new List<User>();
        private List<IObserver> observerList = new List<IObserver>();
        private IFormatConverter formatConverter;
        private string title;
        private string content;
        private User owner;
        private User approver;

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
            docState = new DraftState();
        }

        // This is for the document to add more components
        public void AddComponent(string component)
        {
            Content += $"\n\n{component}";
        }

        public void SetState(DocumentState state)
        {
            docState = state;
            NotifyObservers($"State changed to {state.GetType().Name}.");
        }

        public void AddCollaborator(User user)
        {
            if (Owner == user)
            {
                Console.WriteLine("Owner cannot add themselves as a collaborator.");
                return;
            }
            collaborators.Add(user);
            NotifyObservers($"{user.Name} added as a collaborator.");
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

        public void Edit(string content, User user)
        {
            docState.Edit(this, content, user);
        }
        public void Submit(User user)
        {
            docState.Submit(this, user);
        }

        public string CurrentStateName => docState.GetType().Name;

        public bool IsAssociatedWithUser(User user)
        {
            return owner == user || collaborators.Contains(user) || approver == user;
        }
    }
}
