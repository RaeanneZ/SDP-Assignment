using System;
using System.Collections.Generic;

public class Document : Subject
{
    private List<Observer> observers = new List<Observer>();
    private DocState docState;
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


        public DocState DraftState { get { return draftState; } }
        public DocState ReviewState { get { return reviewState; } }
        public DocState ApprovedState { get { return approvedState; } }
        public DocState RejectedState { get { return rejectedState; } }
        public DocState ReviseState { get { return reviseState; } }

        public bool Success { get; set; }

    public Document(string title, string owner)
    {
        Title = title;
        Owner = owner;
        content = "";

        Collaborators = new List<User>();
        
        draftState = new DraftState(this);
        reviewState = new ReviewState(this);
        approvedState = new ApprovedState(this);
        rejectedState = new RejectedState(this);
        reviseState = new ReviseState(this);

        state = draftState;
    }

    public void AddCollaborator(User user)
    {
        Collaborators.Add(user);
        RegisterObserver(user);
    }

    public void Edit(string content, User user)
    {
        if (Owner == user.Name || Collaborators.Contains(user))
        {
            Content = content;
            NotifyObservers($"Document '{Title}' was edited by {user.Name}.");
        }
    }

    public void SubmitApproval(User user)
    {
        if (user.Name == Owner)
        {
            NotifyObservers($"Document '{Title}' submitted for approval by {user.Name}.");
        }
    }

    public void SetApprover(User user)
    {
        Approver = user;
        RegisterObserver(user);
        NotifyObservers($"Approver assigned: {user.Name} for document '{Title}'.");
    }

    public void SetState(string state)
    {
        NotifyObservers($"Document '{Title}' state changed to: {state}");
    }

    public void RegisterObserver(Observer observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(string message)
    {
        foreach (var observer in observers)
        {
            observer.Notify(message);
        }
    }
    
    public bool IsOwnerOrCollaborator(User user)
        {
            return owner == user || collaborators.Contains(user);
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

