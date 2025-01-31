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
            Content = "Grant Proposal Header\n\nGrant Proposal Footer";
        }
    }
}
