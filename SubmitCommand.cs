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
        private DocState newState;

        public SubmitCommand(Document doc, DocState newState)
        {
            this.doc = doc;
            this.newState = newState;
        }

        public void Execute()
        {
            prevState = doc.getState();
            doc.getState().submit();
            doc.NotifyObservers("Document '" + doc.Title + "' submitted for approval.");
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
