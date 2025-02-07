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
        public void submit();
        public void setApprover(User user);
        public void approve();
        public void reject();
        public void pushBack(string comment);
        public void edit(List<string> section, User collaborator, string action, string text = "", int lineNumber = -1);
    }

}
