using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public abstract class DocumentBase
    {
        public abstract void SetHeader(string newHeader, User user);
        public abstract void SetContent(string newContent, User user);
        public abstract void SetFooter(string newFooter, User user);

    }
}
