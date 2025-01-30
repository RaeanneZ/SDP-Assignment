using System;
using System.Collections.Generic;

public class Document : Subject
{
    private List<Observer> observers = new List<Observer>();

    public string Title { get; private set; }
    public string Content { get; private set; }
    public string Owner { get; private set; }
    public User Approver { get; private set; }
    public List<User> Collaborators { get; private set; }

    public Document(string title, string owner)
    {
        Title = title;
        Owner = owner;
        Collaborators = new List<User>();
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
}