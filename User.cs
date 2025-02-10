using SDP_Assignment;
using System;

public class User : UserComponent
{
    private DocumentCollection documentCollection = new DocumentCollection();

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
        document.ExecuteCommand(command);  
    }

    public void UndoLastCommand(Document document)
    {
        document.UndoLastCommand();  
    }

    public void RedoLastCommand(Document document)
    {
        document.RedoLastCommand();  
    }

    public void AddDocument(Document document)
    {
        documentCollection.AddDocument(document);
    }

    public IDocumentIterator GetDocumentIterator()
    {
        return documentCollection.CreateIterator();
    }

}
