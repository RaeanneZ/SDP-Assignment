using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class WordConverter : IFormatConverter
    {
        public Document Convert(Document document)
        {
            Console.WriteLine("Converting document to Word format...");

            // Create a new file name with the format suffix
            string newFileName = $"{document.Title}_Word";

            //Create a new document
            Document convertedDoc = new Document(newFileName, document.Owner);

            // Copy header, content, and footer
            // list<string> to string
            convertedDoc.SetHeader(string.Join(Environment.NewLine, document.GetHeader()), document.Owner);
            convertedDoc.SetContent(string.Join(Environment.NewLine, document.GetContent()), document.Owner);
            convertedDoc.SetFooter(string.Join(Environment.NewLine, document.GetFooter()), document.Owner);

            Console.WriteLine($"Word document created: {convertedDoc.Title}.docx");

            // Return the converted document
            return convertedDoc;
        }
    }
}
