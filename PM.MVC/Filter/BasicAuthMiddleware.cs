using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Principal;

namespace PM.MVC.Filter
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BasicAuthenticationOptions _options;
        public BasicAuthMiddleware(RequestDelegate next, BasicAuthenticationOptions options)
        {
            this._next = next;
            this._options = options ?? throw new ArgumentException("User info can't be null");
        }
        public async Task Invoke(HttpContext context)
        {
            if (CheckIsValidRequest(context, out string username))
            {
                var identity = new GenericIdentity(username);
                var principle = new GenericPrincipal(identity, null);
                context.User = principle;
                await this._next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }
        private bool CheckIsValidRequest(HttpContext context, out string username)
        {
            var basicAuthHeader = GetBasicAuthenticationHeaderValue(context);
            username = basicAuthHeader.UserName;
            return basicAuthHeader.IsValidBasicAuthenticationHeaderValue &&
                   basicAuthHeader.UserName == this._options.Name &&
                   basicAuthHeader.Password == this._options.Password;
        }
        private BasicAuthenticationHeaderValue GetBasicAuthenticationHeaderValue(HttpContext context)
        {
            var basicAuthenticationHeader = context.Request.Headers["Authorization"]
                .FirstOrDefault(header => header.StartsWith("Basic", StringComparison.OrdinalIgnoreCase));
            var decodedHeader = new BasicAuthenticationHeaderValue(basicAuthenticationHeader);
            return decodedHeader;
        }
    }
    public class BasicAuthenticationOptions
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class BasicAuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string authenticationHeaderValue)
        {
            if (!string.IsNullOrWhiteSpace(authenticationHeaderValue))
            {
                this._authenticationHeaderValue = authenticationHeaderValue;
                if (TryDecodeHeaderValue())
                {
                    ReadAuthenticationHeaderValue();
                }
            }
        }
        private readonly string _authenticationHeaderValue;
        private string[] _splitDecodedCredentials;
        public bool IsValidBasicAuthenticationHeaderValue { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        private bool TryDecodeHeaderValue()
        {
            const int headerSchemeLength = 6;
            if (this._authenticationHeaderValue.Length <= headerSchemeLength)
            {
                return false;
            }
            var encodedCredentials = this._authenticationHeaderValue.Substring(headerSchemeLength);
            try
            {
                var decodedCredentials = Convert.FromBase64String(encodedCredentials);
                this._splitDecodedCredentials = System.Text.Encoding.ASCII.GetString(decodedCredentials).Split(':');
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private void ReadAuthenticationHeaderValue()
        {
            IsValidBasicAuthenticationHeaderValue = this._splitDecodedCredentials.Length == 2
                                                   && !string.IsNullOrWhiteSpace(this._splitDecodedCredentials[0])
                                                   && !string.IsNullOrWhiteSpace(this._splitDecodedCredentials[1]);
            if (IsValidBasicAuthenticationHeaderValue)
            {
                UserName = this._splitDecodedCredentials[0];
                Password = this._splitDecodedCredentials[1];
            }
        }
    }
}

