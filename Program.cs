namespace SDP_Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a document
            User owner = new User("Anna");
            Document document = new Document("Technical Report", "This is the content of the document.", owner);

            // Set format converter to Word
            document.SetFormatConverter(new WordConverter());
            document.ConvertDocument();

            // Change format converter to PDF
            document.SetFormatConverter(new PDFConverter());
            document.ConvertDocument();
        }
    }
}
