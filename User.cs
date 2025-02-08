using System;

public class User : Observer
{
    private string name;

    public string Name { 
        get { return name; } 
    }

    public User(string name)
    {
        this.name = name;
    }

    public virtual void Notify(string message)
    {
        Console.WriteLine($"Notification for {Name}: {message}");
    }
}
