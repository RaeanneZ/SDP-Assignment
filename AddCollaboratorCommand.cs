using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class AddCollaboratorCommand : DocumentCommand
    {
        private Document doc;
        private User user;
        public AddCollaboratorCommand(Document doc, User user) {
            this.doc = doc;
            this.user = user;   
        }

        public void Execute()
        {
            doc.AddCollaborator(this.user);
        }

        public void Undo()
        {
            doc.Collaborators.Remove(this.user);
            doc.NotifyObservers($"{user.Name} has been removed as collaborator.");
        }

        public void Redo()
        {
            Execute();
        }
    }
}
