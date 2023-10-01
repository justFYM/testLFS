namespace TerriaMVC.Store;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public class InMemoryUserStore : IUserStore<IdentityUser>,
                                  IUserRoleStore<IdentityUser>,
                                  IUserPasswordStore<IdentityUser>,
                                  IUserEmailStore<IdentityUser>
{
    private readonly List<IdentityUser> users = new List<IdentityUser>();
    private readonly List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

    // Implementación de IUserStore

    public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        users.Add(user);
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        users.Remove(user);
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(users.FirstOrDefault(u => u.Id == userId));
    }

    public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return Task.FromResult(users.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName));
    }

    public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    // Implementación de IUserRoleStore (puedes agregar otras interfaces requeridas)

    public Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        var role = new IdentityUserRole<string> { UserId = user.Id, RoleId = roleName };
        userRoles.Add(role);
        return Task.CompletedTask;
    }

    public Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        var roleNames = userRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToList();
        return Task.FromResult<IList<string>>(roleNames);
    }

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {

    }

    public Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    // Implementación de otras interfaces...

    // Debes implementar otras interfaces como IUserPasswordStore, IUserEmailStore, etc.
    // Puedes encontrar los métodos requeridos en la documentación de Identity.
    // La implementación variará dependiendo de tus necesidades y requerimientos.
    // Por simplicidad, no los he incluido todos en este ejemplo.

    // ...
}

