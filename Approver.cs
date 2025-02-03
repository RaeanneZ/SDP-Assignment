public class Approver : User
{
    public Approver(string name) : base(name) { }

    public override void Notify(string message)
    {
        Console.WriteLine("[Approver] Notification for " + Name + ": " + message);
    }
}