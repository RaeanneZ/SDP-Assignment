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
        private UserComponent collaborator;

        public AddCollaboratorCommand(Document doc, UserComponent collaborator) {
            this.doc = doc;
            this.collaborator = collaborator;   
        }

        public void Execute()
        {
            doc.getState().add(collaborator);
        }

        public void Undo()
        {
            doc.RemoveCollaborator(collaborator);
            doc.NotifyObservers($"{collaborator.Name} has been removed as collaborator.");
        }

        public void Redo()
        {
            Execute();
        }
    }
}
