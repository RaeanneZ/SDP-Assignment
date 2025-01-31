using System;

public class User : Observer
{
    public string Name { get; private set; }

    public User(string name)
    {
        Name = name;
    }

    public virtual void Notify(string message)
    {
        Console.WriteLine($"Notification for {Name}: {message}");
    }
}
