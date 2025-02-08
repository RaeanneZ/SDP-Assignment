using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    public abstract class UserComponent : Observer
    {
        private string name;
        public string Name
        {
            get;
            protected set;
        }

        public abstract void Notify(string message);
        public virtual void Add(UserComponent user) { throw new NotImplementedException(); }
        public virtual void Remove(UserComponent user) { throw new NotImplementedException(); }
        public virtual List<UserComponent> GetUsers() { throw new NotImplementedException(); }
    }
}
