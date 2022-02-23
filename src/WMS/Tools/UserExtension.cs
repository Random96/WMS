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


        public static string GetAddr()
        {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create("http://www.eml-soft.ru/addr.php");
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        return responseFromServer;
                    }
                }
            }
        }
    }
}
