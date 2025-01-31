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
            Content = "Technical Report Header\n\nTechnical Report Footer";
        }
    }
}
