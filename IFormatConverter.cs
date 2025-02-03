using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public interface IFormatConverter
    {
        Document Convert(Document document);
    }
}
