using System;

namespace GigyaMultipleLogins
{
    public class Program
    {
        public static void Main()
        {
            var client = CreateGigyaClient();

            var user = new {Email = Guid.NewGuid().ToString("N") + "@example.com", Password = "p@s$w00rd!"};

            //register
            var regToken = client.InitRegistration().RegistrationToken;
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            var uid = client.Register(user.Email, user.Password, regToken).UserId;
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            client.FinalizeRegistration(regToken);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

            //try to login with wrong password
            var attempts = 10;
            for (int i = 0; i < attempts; i++)
            {
                client.Login(user.Email, "wrong password");
            }

            //delete the user
            client.DeleteAccount(uid);
        }

        private static GigyaClient CreateGigyaClient()
        {
            var configuration = new GigyaConfiguration
            {
                ApiBaseUrl = "https://accounts.us1.gigya.com/",
                ApiKey = "...",
                ApiSecret = "..."
            };

            return new GigyaClient(configuration);
        }
    }
}
