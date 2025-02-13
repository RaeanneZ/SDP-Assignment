using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    internal class SignatureDecorator : DocumentDecorator
    {
        public SignatureDecorator(Document document) : base(document) { }

        public override void SetFooter(string text, User user)
        {
            string newFooter = "Signature: " + text;
            document.SetFooter(newFooter, user);  // Append signature

            Console.WriteLine("Added Signature for " + text + " into document.");
        }
    }
}
