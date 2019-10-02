using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public class BaseService
    {
        public static string DisplayErrorMessage(IdentityError error)
        {
            return $"{error.Code} : {error.Description}";
        }
    }
}
