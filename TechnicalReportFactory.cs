using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class TechnicalReportFactory : DocumentFactory
    {
        public override Document CreateDocument(string title, User owner)
        {
            Console.WriteLine("Creating a Technical Report.");
            return new TechnicalReportDocument(title, owner);
        }
    }
}
