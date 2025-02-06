using SDP_Assignment;

public class EditDocumentCommand : DocumentCommand
{
    private Document doc;
    private User user;
    private List<string> section;
    private List<string> oldContent;
    private string action;
    private string text;
    private int lineNumber;

    public EditDocumentCommand(Document doc, List<string> section, User user, string action, string text = "", int lineNumber = -1)
    {
        this.doc = doc;
        this.section = section;
        this.user = user;
        this.action = action;
        this.text = text;
        this.lineNumber = lineNumber;
    }

    public void Execute()
    {
        this.oldContent = new List<string>(section);
        doc.getState().edit(section, user, action, text, lineNumber);
    }

    public void Undo()
    {
        if (oldContent != null)
        {
            section.Clear();
            section.AddRange(oldContent);
        }
    }

    public void Redo()
    {
        Execute();
    }
}
