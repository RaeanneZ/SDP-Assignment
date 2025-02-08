using System;
using System.Reflection.PortableExecutable;

namespace SDP_Assignment
{
    public class DocumentVersion
    {
        public string Header { get; private set; }
        public string Content { get; private set; }
        public string Footer { get; private set; }
        public DateTime Timestamp { get; private set; }

        public DocumentVersion(string header, string content, string footer)
        {
            Header = header;
            Content = content;
            Footer = footer;
            Timestamp = DateTime.Now;
        }
    }
}