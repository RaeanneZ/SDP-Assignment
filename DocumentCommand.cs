using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public interface DocumentCommand
    {
        void Execute();
        void Undo();
        void Redo();
    }
}
