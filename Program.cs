using System;
//test program
class Program
{
    static void Main()
    {
        Document doc = new Document("Project Plan", "Alice");
        Owner owner = new Owner("Alice");
        Collaborator collaborator = new Collaborator("Bob");
        Approver approver = new Approver("Charlie");

        doc.RegisterObserver(owner);
        doc.AddCollaborator(collaborator);
        doc.SetApprover(approver);

        doc.Edit("Initial draft completed.", owner);
        doc.SubmitApproval(owner);
        doc.SetState("Approved");
    }
}