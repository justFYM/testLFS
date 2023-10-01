using Microsoft.AspNetCore.Identity;

namespace TerriaMVC.Store
{
    public class InMemoryUserRepository
    {
        private List<IdentityUser> users = new List<IdentityUser>();
        private List<IdentityRole> roles = new List<IdentityRole>();

        public void AddUser(IdentityUser user)
        {
            users.Add(user);
        }

        public void AddRole(IdentityRole role)
        {
            roles.Add(role);
        }

        public IdentityUser FindUserById(string userId)
        {
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public IdentityUser FindUserByName(string userName)
        {
            return users.FirstOrDefault(u => u.UserName == userName);
        }

        public IdentityRole FindRoleById(string roleId)
        {
            return roles.FirstOrDefault(r => r.Id == roleId);
        }

        public IdentityRole FindRoleByName(string roleName)
        {
            return roles.FirstOrDefault(r => r.Name == roleName);
        }
    }
}
