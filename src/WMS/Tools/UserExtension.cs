using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ru.EmlSoft.WMS.Controllers;
using ru.EmlSoft.WMS.Data.Abstract.Identity;
using System.Net;
using System.Security.Claims;

namespace WMS.Tools
{
    public static class UserExtension
    {
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


        public static async Task<string> GetAddrAsync()
        {
            HttpClient client = new HttpClient();

            try
            {
                // Create a request for the URL. 		
                using (var response = await client.GetAsync("http://www.eml-soft.ru/addr.php"))
                {

                    response.EnsureSuccessStatusCode();

                    // Get the response.
                    var responseBody = await response.Content.ReadAsStringAsync();

                    return responseBody;
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
