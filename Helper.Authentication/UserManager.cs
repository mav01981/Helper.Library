using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity
{
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager(IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }


        public async Task<bool> CheckUserPassword(string username, string password)
        {
            var token = new CancellationToken();

            var user = await this.Store.FindByNameAsync(username, new CancellationToken());

            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            if (user == null)
                return false;

            var result = this.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success;
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
            var token = new CancellationToken();

            user.PasswordHash = this.PasswordHasher.HashPassword(user, user.PasswordHash);

            var result = await this.Store.CreateAsync(user, token);

            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            return user;
        }
    }
}
