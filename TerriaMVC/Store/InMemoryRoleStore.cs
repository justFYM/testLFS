using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TerriaMVC.Store
{
    public class InMemoryRoleStore : IRoleStore<IdentityRole>
    {
        private readonly List<IdentityRole> _roles = new List<IdentityRole>();

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            _roles.Add(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            _roles.Remove(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_roles.FirstOrDefault(r => r.Id == roleId));
        }

        public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_roles.FirstOrDefault(r => r.NormalizedName == normalizedRoleName));
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // No es necesario hacer nada ya que trabajamos con la lista en memoria
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            // No es necesario implementar nada aquí para el repositorio en memoria
        }
    }
}
