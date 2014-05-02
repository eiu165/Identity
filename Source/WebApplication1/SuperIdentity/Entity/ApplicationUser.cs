using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperIdentity.Entity
{
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser()
        {

            this.Id = Guid.NewGuid().ToString();

        }

        public ApplicationUser(string userName) : this()
        { 
            UserName = userName; 
        }

        public virtual string Id { get; set; }


        public virtual string Email { get; set; }


        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string UserName { get; set; }

         
    }

}
