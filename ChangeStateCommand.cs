using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class ChangeStateCommand : DocumentCommand
    {
        private Document doc;
        private DocState prevState;
        private DocState newState;

        public ChangeStateCommand(Document doc, DocState newState)
        {
            this.doc = doc;
            this.newState = newState;
        }

        public void Execute()
        {
            prevState = doc.getState();
            doc.SetState(newState);
        }

        public void Undo()
        {
            doc.SetState(prevState);
        }

        public void Redo() {
            Execute();
        }
    }
}
