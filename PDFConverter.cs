using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class PDFConverter : FormatConverter
    {
        public void Convert(string content)
        {
            Console.WriteLine("Converting document to PDF format...");
            // Actual conversion logic to PDF format
            Console.WriteLine($"PDF Content: {content}");
        }
    }
}
