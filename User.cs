using SDP_Assignment;
using System;

public class User : UserComponent
{ 
    public User(string name)
    {
        Name = name;
    }

    public override void Notify(string message)
    {
        Console.WriteLine($"Notification for {Name}: {message}");
    }
}
