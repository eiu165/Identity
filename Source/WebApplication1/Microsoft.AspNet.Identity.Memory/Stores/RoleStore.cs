// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleStore.cs" company="Private (Caleb Kiage)">
//   Copyright (c) 2013
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AspNet.Identity.Memory.Stores
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNet.Identity.Memory.Entities;

    using Microsoft.AspNet.Identity;
     

    public class RoleStore<TRole> : IRoleStore<TRole>
        where TRole : IdentityRole
    {
        #region Fields
         

        private bool disposed;

        #endregion

        #region Constructors and Destructors

        public RoleStore( )
        { 
        }

        #endregion

        #region Public Methods and Operators

        public virtual async Task CreateAsync(TRole role)
        {
            this.ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.roleRepository.Save(role);
            await Task.FromResult(0);
        }

        public virtual Task DeleteAsync(TRole role)
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(this.roleRepository.FindOne(roleId));
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            this.ThrowIfDisposed();
            return
                Task.FromResult(
                    this.roleRepository.FindAll().FirstOrDefault(u => u.Name.ToUpper() == roleName.ToUpper()));
        }

        public virtual async Task UpdateAsync(TRole role)
        {
            this.ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.roleRepository.Save(role);
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