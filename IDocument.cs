using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public interface IDocument
    {
        void SetHeader(string newHeader, User user);
        void SetContent(string newContent, User user);
        void SetFooter(string newFooter, User user);

    }
}
