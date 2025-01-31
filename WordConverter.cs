using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class WordConverter : FormatConverter
    {
        public void Convert(string content)
        {
            Console.WriteLine("Converting document to Word format...");
            // Actual conversion logic to Word format
            Console.WriteLine($"Word Content: {content}");
        }
    }
}
