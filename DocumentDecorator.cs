using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public abstract class DocumentDecorator : DocumentBase
    {
        protected DocumentBase document;

        public DocumentDecorator(DocumentBase document)
        {
            this.document = document;
        }

        public override void SetHeader(string newHeader, User user)
        {
            document.SetHeader(newHeader, user);
        }

        public override void SetContent(string newContent, User user)
        {
            document.SetContent(newContent, user);
        }

        public override void SetFooter(string newFooter, User user)
        {
            document.SetFooter(newFooter, user);
        }
    }
}
