// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityUser.cs" company="Private (Caleb Kiage)">
//   Copyright (c) 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AspNet.Identity.Memory.Entities
{   
    using Microsoft.AspNet.Identity; 

    public class IdentityUser :  IUser 
    {
        public string Id
        {
            get { return "aaaa"; }
        }

        public string UserName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}