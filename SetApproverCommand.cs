using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class SetApproverCommand : DocumentCommand
    {
        private Document doc;
        private User prevApprover;
        private User newApprover;

        public SetApproverCommand(Document doc, User newApprover)
        {
            this.doc = doc;
            this.newApprover = newApprover;
            this.prevApprover = doc.Approver;
        }

        public void Execute()
        {
            
        }

        public void Undo()
        {
            doc.SetApprover(prevApprover);
            if (prevApprover != null)
            {
                doc.NotifyObservers($"Approver reverted back to: {prevApprover.Name}.");
            }
            else
            {
                doc.NotifyObservers($"Approver removed for {doc.Title}");
            }
        }

        public void Redo()
        {
            Execute();
        }
    }
}
