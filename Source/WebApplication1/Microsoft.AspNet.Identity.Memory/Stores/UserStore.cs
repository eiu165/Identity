// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserStore.cs" company="Private (Caleb Kiage)">
//   Copyright (c) 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AspNet.Identity.Memory.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AspNet.Identity.Memory.Entities;

    using Microsoft.AspNet.Identity;
     

    /// <summary>
    /// The user store.
    /// </summary>
    /// <typeparam name="TUser">
    /// The type of user.
    /// </typeparam>
    public class UserStore<TUser> : IUserLoginStore<TUser>, 
                                    IUserClaimStore<TUser>, 
                                    IUserRoleStore<TUser>, 
                                    IUserPasswordStore<TUser>, 
                                    IUserSecurityStampStore<TUser>
        where TUser : IdentityUser
    {
        #region Fields
         
        private bool disposed;

        #endregion

        #region Constructors and Destructors

        public UserStore( )
        { 
        }

        #endregion

        #region Public Properties

        public bool AutoSaveChanges { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual Task AddClaimAsync(TUser user, Claim claim)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            user.AddClaim(new IdentityUserClaim { User = user, ClaimType = claim.Type, ClaimValue = claim.Value });
            return Task.FromResult(0);
        }

        public virtual Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            user.AddLogin(
                new IdentityUserLogin
                    {
                        User = user, 
                        ProviderKey = login.ProviderKey, 
                        LoginProvider = login.LoginProvider
                    });
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException(IdentityResources.ValueCannotBeNullOrWhiteSpace, "role");
            }

            IdentityRole identityRole =
                this.roleRepository.FindAll().SingleOrDefault(r => r.Name.ToUpper() == role.ToUpper());
            if (identityRole == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, IdentityResources.RoleNotFound, new[] { (object)role }));
            }

            user.AddRole(identityRole);
            return Task.FromResult(0);
        }

        public virtual async Task CreateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.userRepository.Save(user);
            await Task.FromResult(0);
        }

        public virtual Task DeleteAsync(TUser user)
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual async Task<TUser> FindAsync(UserLoginInfo login)
        {
            this.ThrowIfDisposed();
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            IdentityUser entity =
                this.loginRepository.FindAll()
                    .Where(l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey)
                    .Select(l => l.User)
                    .FirstOrDefault();
            return await Task.FromResult(entity as TUser);
        }

        public virtual Task<TUser> FindByIdAsync(string userId)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(this.userRepository.FindOne(userId));
        }

        public virtual async Task<TUser> FindByNameAsync(string userName)
        {
            this.ThrowIfDisposed();
            return
                await
                Task.FromResult(
                    this.userRepository.FindAll().FirstOrDefault(u => u.UserName.ToUpper() == userName.ToUpper()));
        }

        public virtual Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<Claim> result = new List<Claim>();
            foreach (IdentityUserClaim identityUserClaim in user.Claims)
            {
                result.Add(new Claim(identityUserClaim.ClaimType, identityUserClaim.ClaimValue));
            }

            return Task.FromResult(result);
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<UserLoginInfo> result = new List<UserLoginInfo>();
            foreach (IdentityUserLogin identityUserLogin in user.Logins)
            {
                result.Add(new UserLoginInfo(identityUserLogin.LoginProvider, identityUserLogin.ProviderKey));
            }

            return Task.FromResult(result);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult((IList<string>)user.Roles.Select(u => u.Name).ToList());
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException(IdentityResources.ValueCannotBeNullOrWhiteSpace, "role");
            }

            return Task.FromResult(user.Roles.Any(r => r.Name.ToUpper() == role.ToUpper()));
        }

        public virtual Task RemoveClaimAsync(TUser user, Claim claim)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            foreach (IdentityUserClaim entity in user.Claims.Where(
                uc =>
                    {
                        if (uc.ClaimValue == claim.Value)
                        {
                            return uc.ClaimType == claim.Type;
                        }

                        return false;
                    }).ToList())
            {
                user.Claims.Remove(entity);
                this.claimRepository.Delete(entity);
            }

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException(IdentityResources.ValueCannotBeNullOrWhiteSpace, "role");
            }

            IdentityRole entity = user.Roles.FirstOrDefault(r => r.Name.ToUpper() == role.ToUpper());
            if (entity != null)
            {
                user.Roles.Remove(entity);
            }

            return Task.FromResult(0);
        }

        public virtual Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            IdentityUserLogin entity = user.Logins.Where(
                l =>
                    {
                        if (l.LoginProvider == login.LoginProvider && Equals(l.User, user))
                        {
                            return l.ProviderKey == login.ProviderKey;
                        }

                        return false;
                    }).SingleOrDefault();
            if (entity == null)
            {
                return Task.FromResult(0);
            }

            user.Logins.Remove(entity);
            this.loginRepository.Delete(entity);
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public virtual async Task UpdateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.userRepository.Save(user);
            await Task.FromResult(0);
        }

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            this.disposed = true;
        }
        
        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        #endregion
    }
}