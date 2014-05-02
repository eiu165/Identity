using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Memory.Entity
{
    public class IdentityUser : IUser
    {

        public IdentityUser()
        {
        }

        public IdentityUser(string userName)
        {
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

    }

}
