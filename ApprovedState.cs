﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class ApprovedState : DocState
    {
        private Document doc;

        public ApprovedState(Document document)
        {
            doc = document;
        }

        public void add(User collaborator)
        {
            Console.WriteLine("Cannot add collaborators to an approved document.");
        }

        public void submit() 
        {
            Console.WriteLine("Document is already approved.");
        }

        public void setApprover(User collaborator)
        {
            Console.WriteLine("Document is already approved.");
        }

        public void approve()
        {
            Console.WriteLine("Document is already approved.");
        }

        public void reject()
        {
            Console.WriteLine("Cannot reject an approved document.");
        }

        public void pushBack(string comment)
        {
            Console.WriteLine("Cannot push back an approved document.");
        }

        public void resubmit()
        {
            Console.WriteLine("Cannot resubmit an approved document.");
        }

        public void edit(List<string> content, string newContent, User collaborator)
        {
            Console.WriteLine("Cannot edit an approved document.");
        }
    }

}
