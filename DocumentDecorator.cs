using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public abstract class DocumentDecorator : IDocument
    {
        protected IDocument document;

        public DocumentDecorator(IDocument document)
        {
            this.document = document;
        }

        public virtual void SetHeader(string newHeader, User user)
        {
            document.SetHeader(newHeader, user);
        }

        public virtual void SetContent(string newContent, User user)
        {
            document.SetContent(newContent, user);
        }

        public virtual void SetFooter(string newFooter, User user)
        {
            document.SetFooter(newFooter, user);
        }
    }
}
