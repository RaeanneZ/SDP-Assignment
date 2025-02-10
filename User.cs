using SDP_Assignment;
using System.Xml.Linq;

public class User : UserComponent
{
    public Dictionary<Document, Stack<DocumentCommand>> commandHistory = new Dictionary<Document, Stack<DocumentCommand>>();
    public Dictionary<Document, Stack<DocumentCommand>> redoStacks = new Dictionary<Document, Stack<DocumentCommand>>();

    public User(string name)
    {
        Name = name;
    }

    public override void Notify(string message)
    {
        Console.WriteLine($"Notification for {Name}: {message}");
    }

    public void ExecuteCommand(Document document, DocumentCommand command)
    {
        if (!commandHistory.ContainsKey(document))
        {
            commandHistory[document] = new Stack<DocumentCommand>();
            redoStacks[document] = new Stack<DocumentCommand>();
        }

        command.Execute();
        commandHistory[document].Push(command);
        redoStacks[document].Clear(); 
    }

    public void UndoLastCommand(Document document)
    {
        if (commandHistory.ContainsKey(document) && commandHistory[document].Count > 0)
        {
            DocumentCommand command = commandHistory[document].Pop();
            command.Undo();
            redoStacks[document].Push(command);
        }
        else
        {
            Console.WriteLine($"{Name} has no actions to undo for this document!");
        }
    }

    public void RedoLastCommand(Document document)
    {
        if (redoStacks.ContainsKey(document) && redoStacks[document].Count > 0)
        {
            DocumentCommand command = redoStacks[document].Pop();
            command.Redo();
            commandHistory[document].Push(command);
        }
        else
        {
            Console.WriteLine($"{Name} has no actions to redo for this document!");
        }
    }
}
