using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    abstract class DocumentFactory
    {
        public abstract Document CreateDocument(string title, User owner);
    }
}
