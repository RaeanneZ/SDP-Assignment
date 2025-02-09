using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class SubmitCommand : DocumentCommand
    {
        private Document doc;
        private DocState prevState;

        public SubmitCommand(Document doc)
        {
            this.doc = doc;
        }

        public void Execute()
        {
            prevState = doc.getState();
            doc.getState().submit();
        }

        public void Undo()
        {
            doc.SetState(prevState);
            doc.NotifyObservers("Document '" + doc.Title + "' unsubmitted for approval.");
        }

        public void Redo() {
            Execute();
        }
    }
}
