using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class PDFConverter : IFormatConverter
    {
        public Document Convert(Document document)
        {
            Console.WriteLine("Converting document to PDF format...");

            // Create a new file name with the format suffix
            string newFileName = $"{document.Title}_PDF.pdf";

            //Create a new document
            Document convertedDoc = new Document(newFileName, document.Owner);

            // Copy header, content, and footer
            List<string> fullContent = new List<string>();
            fullContent.AddRange(convertedDoc.GetHeader());
            fullContent.AddRange(convertedDoc.GetContent());
            fullContent.AddRange(convertedDoc.GetFooter());

            // Simulate creating a Word document
            using (StreamWriter writer = new StreamWriter($"{convertedDoc.Title}.pdf"))
            {
                foreach (var line in fullContent)
                {
                    writer.WriteLine(line);
                }
            }

            Console.WriteLine($"PDF document created: {convertedDoc.Title}.pdf");

            // Return the converted document
            return convertedDoc;
        }
    }
}
