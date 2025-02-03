using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class GrantProposalDocument : Document
    {
        public GrantProposalDocument(string title, User owner) : base(title, owner)
        {
            SetHeader("Grant Proposal Header", owner);
            SetContent("This is the body of the grant proposal.", owner);
            SetFooter("Grant Proposal Footer", owner);
        }
    }
}
