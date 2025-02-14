using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    class UserGroup : UserComponent
    {
        private List<UserComponent> members = new List<UserComponent>();

        public UserGroup(string groupName)
        {
            Name = groupName;
        }

        public override void Add(UserComponent user)
        {
            if (!members.Contains(user))
            {
                members.Add(user);
                user.Notify($"You have been added to the group '{Name}'.");
            }
        }

        public override void Remove(UserComponent user)
        {
            members.Remove(user);
        }

        public override List<UserComponent> GetUsers()
        {
            return members;
        }

        public override void Notify(string message)
        {
            Console.WriteLine($"[Group: {Name}] Notification: {message}");
            foreach (var member in members)
            {
                member.Notify(message);
            }
        }
    }
}
