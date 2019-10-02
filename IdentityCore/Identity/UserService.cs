using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace Identity
{
    public class UserService : IUserService
    {
        public async Task<bool> IsAuthenticated(string username, string password)
        {
            var factory = new IdentityContextFactory(); ;

            //using (var db = factory.Create("Filename = MyDatabase.db"))
            //{
            //    var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == username && x.PasswordHash == password);

            //    if (user == null)
            //        return false;
            //}

            return true;
        }

        public Task<bool> IsAuthorized(string username)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserService
    {
        Task<bool> IsAuthenticated(string username, string password);
        Task<bool> IsAuthorized(string username);
    }
}


