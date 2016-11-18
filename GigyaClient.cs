using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using GigyaMultipleLogins.Responses;
using Newtonsoft.Json.Linq;

namespace GigyaMultipleLogins
{
    public class GigyaClient
    {
        private readonly GigyaConfiguration _configuration;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(60);

        public GigyaClient(GigyaConfiguration configuration)
        {
            _configuration = configuration;
        }

        public InitRegistrationResponse InitRegistration()
        {
            var args = new Dictionary<string, object>();
            return ExecuteRequest<InitRegistrationResponse>("accounts.initRegistration", args);
        }

        public RegisterResponse Register(string email, string password, string regToken, object profile = null, object data = null)
        {
            var args = new Dictionary<string, object>
            {
                { "email", email },
                { "password", password },
                { "regToken", regToken }
            };
            return ExecuteRequest<RegisterResponse>("accounts.register", args);
        }

        public FinalizeRegistrationResponse FinalizeRegistration(string regToken)
        {
            var args = new Dictionary<string, object>
            {
                { "regToken", regToken }
            };
            return ExecuteRequest<FinalizeRegistrationResponse>("accounts.finalizeRegistration", args);
        }

        public LoginResponse Login(string email, string password)
        {
            var args = new Dictionary<string, object>
            {
                { "loginID", email },
                { "password", password }
            };
            return ExecuteRequest<LoginResponse>("accounts.login", args);
        }

        public DeleteAccountResponse DeleteAccount(string uid)
        {
            var args = new Dictionary<string, object>
            {
                { "uid", uid }
            };
            return ExecuteRequest<DeleteAccountResponse>("accounts.deleteAccount", args);
        }


        private TResponse ExecuteRequest<TResponse>(string method, Dictionary<string, object> args)
            where TResponse : GigyaResponseBase, new()
        {
            var response = new TResponse();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var gigyaResponse = GetJsonResponse(method, args);
                //validate response - skipped in this example app

                response.ApplyValues(gigyaResponse);
                return response;
            }
            finally
            {
                stopwatch.Stop();
                var elapsed = $"elapsed {stopwatch.Elapsed.TotalSeconds:N2}s";
                var message = $"{method} ({elapsed}): response=({response})";
                Console.WriteLine(message);
            }
        }

        private dynamic GetJsonResponse(string method, Dictionary<string, object> args)
        {
            var baseUri = new Uri(_configuration.ApiBaseUrl);
            args["apiKey"] = _configuration.ApiKey;
            args["secret"] = _configuration.ApiSecret;
            var uri = new Uri(baseUri, method + BuildQueryString(args));

            var request = WebRequest.Create(uri);
            request.Timeout = (int)_timeout.TotalMilliseconds;
            request.ContentType = "application/json";

            using (var response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return string.IsNullOrEmpty(json) ? null : JObject.Parse(json);
                }
            }
        }

        private string BuildQueryString(IDictionary<string, object> dict)
        {
            var list = dict.Select(pair => pair.Key + "=" + WebUtility.UrlEncode(pair.Value.ToString())).ToList();
            return "?" + string.Join("&", list);
        }
    }
}