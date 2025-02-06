using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class TechnicalReportDocument : Document
    {
        public TechnicalReportDocument(string title, User owner) : base(title, owner)
        {
            SetHeader("Technical Report Header", owner);
            SetContent("This is the body of the technical report.", owner);
            SetFooter("Technical Report Footer", owner);
        }
    }
}
