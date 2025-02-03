using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public interface DocState
    {
        public void add(User collaborator);
        public void submit(User collaborator);
        public void approve();
        public void reject();
        public void pushBack(string comment);
        public void resubmit();
        public void edit(List<string> content, string newContent, User collaborator);
    }

}
