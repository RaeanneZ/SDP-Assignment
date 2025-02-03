public class Collaborator : User
{
    public Collaborator(string name) : base(name) { }

    public override void Notify(string message)
    {
        Console.WriteLine("[Collaborator] Notification for " + Name + ": " + message);
    }
}