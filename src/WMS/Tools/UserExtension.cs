using Microsoft.AspNetCore.Identity;
using ru.emlsoft.WMS.Data.Abstract.Identity;
using System.Security.Claims;

namespace ru.emlsoft.WMS.Tools
{
    public static class UserExtension
    {


        public static async Task<string> GetAddressAsync()
        {
            using var client = new HttpClient();
            try
            {
                // Create a request for the URL. 		
                using var response = await client.GetAsync("http://www.eml-soft.ru/addr.php");

                response.EnsureSuccessStatusCode();

                // Get the response.
                var responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        public static async Task<User> GetUserAsync(this IUserStore userStore, SignInManager<User> signInManager, CancellationToken cancellationToken = default)
        {
            var user = signInManager.Context.User;
            if (user != null)
            {
                // get current db user
                if (int.TryParse(user.FindFirst(ClaimTypes.Sid)?.Value, out int sid))
                {
                    var dbUser = await userStore.GetUserByIdAsync(sid, cancellationToken);

                    return dbUser;
                }
            }

            return new User();
        }

        public static User GetUser(this IUserStore userStore, SignInManager<User> signInManager)
        {
            var user = signInManager.Context.User;
            if (user != null)
            {
                // get current db user
                if (int.TryParse(user.FindFirst(ClaimTypes.Sid)?.Value, out int sid))
                {
                    var dbUser = userStore.GetUserById(sid);

                    return dbUser;
                }
            }

            return new User();
        }
    }
}
