using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class EditDocumentCommand : DocumentCommand
    {
        private Document doc;
        private User user;
        private List<String> section;
        private List<string> oldContent;

        public EditDocumentCommand(Document doc, List<string> section, User user)
        {
            this.doc = doc;
            this.section = section;
            this.user = user;
        }

        public void Execute()
        {
            this.oldContent = new List<string>(section);
            doc.getState().edit(section, user);
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
}
