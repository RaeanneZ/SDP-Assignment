using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public class Document
    {
        private string title;
        private string content;
        private User owner;
        private User approver;
        private List<User> collaborators;
        private FormatConverter formatConverter;

        public Document(string t, string c, User o)
        {
            title = t;
            content = c;
            owner = o;
            collaborators = new List<User>();
        }

        public void SetFormatConverter(FormatConverter fc)
        {
            formatConverter = fc;
        }

        public void ConvertDocument()
        {
            if (formatConverter == null)
            {
                Console.WriteLine("No format converter set.");
                return;
            }
            formatConverter.Convert(content);
        }
    }
}
