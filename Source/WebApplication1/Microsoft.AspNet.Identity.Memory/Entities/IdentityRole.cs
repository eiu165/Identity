// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityRole.cs" company="Private (Caleb Kiage)">
//   Copyright (c) 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AspNet.Identity.Memory.Entities
{
    using Microsoft.AspNet.Identity;
    using Iesi.Collections.Generic;
    using System.Collections.Generic;
     

    public class IdentityRole :  IRole 
    {
        #region Constructors and Destructors

        public IdentityRole()
        {
            this.Users = new HashedSet<IdentityUser>();
        }

        public IdentityRole(string roleName)
            : this()
        {
            this.Name = roleName;
        }

        #endregion

        #region Public Properties
         
        public string Name { get; set; }

        public ISet<IdentityUser> Users { get; protected set; }

        public long Version { get; protected set; }

        #endregion

        #region Public Methods and Operators

        public void AddUser(IdentityUser user)
        {
            user.Roles.Add(this);
            this.Users.Add(user);
        }

        public void RemoveUser(IdentityUser user)
        {
            user.Roles.Remove(this);
            this.Users.Remove(user);
        }

        #endregion

        public string Id
        {
            get { return "sdfsaf"; }
        }
    }
}