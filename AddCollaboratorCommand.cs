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
            doc.getState().add(this.user);
        }

        public void Undo()
        {
            doc.NotifyObservers($"{user.Name} has been removed as collaborator.");
            doc.RemoveObserver(this.user);
            doc.Collaborators.Remove(this.user);
        }

        public void Redo()
        {
            doc.NotifyObservers($"{user.Name} has been added as collaborator.");
            Execute();
        }
    }
}
