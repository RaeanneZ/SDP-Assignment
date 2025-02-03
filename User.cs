using System;

public class User : Observer
{
    private string name;

    public string Name { get; private set; }

    public User(string name)
    {
        this.name = name;
    }

    public virtual void Notify(string message)
    {
        Console.WriteLine($"Notification for {Name}: {message}");
    }
}
