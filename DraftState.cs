using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class DraftState : DocState
    {
        private Document doc;

        public DraftState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            doc.Collaborators.Add(collaborator);
        }

        public void submit()
        {
            doc.SetState(doc.ReviewState);
        }
        public void approve() => Console.WriteLine("Cannot approve a draft document.");
        public void reject() => Console.WriteLine("Cannot reject a draft document.");
        public void pushBack(string comment) => Console.WriteLine("Cannot push back a draft document.");
        public void resubmit() => Console.WriteLine("Cannot resubmit a draft document.");
        public void edit(List<string> content, string newContent, User collaborator)
        {
            content.Add(newContent);
        }
    }

}
