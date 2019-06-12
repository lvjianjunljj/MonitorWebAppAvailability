using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System;

namespace WebAndLoadTestProject
{
    public class JWTTokenWebTestPlugin : WebTestPlugin
    {
        [DisplayName("TenantId")]
        [Description("TenantId")]
        public string TenantId { get; set; }

        [DisplayName("ClientId")]
        [Description("ClientId")]
        public string ClientId { get; set; }

        [DisplayName("ClientSecret")]
        [Description("ClientSecret")]
        public string ClientSecret { get; set; }

        [DisplayName("Resource")]
        [Description("Resource")]
        public string Resource { get; set; }

        private string Token { get; set; }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            //if (string.IsNullOrEmpty(this.ContextParameterName))
            //{
            //    throw new ArgumentNullException();
            //}
            //Token = GetAccessTokenAsync(ClientId, AadInstance, Tenant, ClientSecret).Result;
            Token = GetDataCopToken(this.TenantId, this.ClientId, this.ClientSecret, this.Resource).Result;
            //e.WebTest.Context[ContextParameterName] = Token;
            //httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        }

        public override void PreRequestDataBinding(object sender, PreRequestDataBindingEventArgs e)
        {
            e.Request.Headers.Add("Authorization", "Bearer " + Token);
        }

        //private object GetAdToken(string clientId, string ClientSecret, string aadInstance, string tenant, string toDoResourceId)
        //{

        //    var authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        //    var authContext = new AuthenticationContext(authority, new TokenCache());
        //    var credential = new ClientCredential(clientId, ClientSecret);
        //    var result = authContext.AcquireTokenAsync(toDoResourceId, credential);
        //    return result.Result.AccessToken;
        //}

        //public static async Task<string> GetAccessTokenAsync(string resource, string aadInstance, string tenant, string ClientSecret)
        //{
        //    //
        //    // a context pointing to our Application in our Tenant in our Azure AD (validates authority and caches)
        //    //
        //    var authContext = new AuthenticationContext(string.Concat(aadInstance, tenant));

        //    //
        //    // authenticate with AAD either via Certificate, or API key
        //    //
        //    Task<AuthenticationResult> authTask;

        //    {
        //        // use API key
        //        var apiCred = new ClientCredential(resource, ClientSecret);
        //        authTask = authContext.AcquireTokenAsync(resource, apiCred);
        //    }

        //    //
        //    // do the auth handshake
        //    //
        //    var result = await authTask.ConfigureAwait(true);
        //    return result.AccessToken;
        //}

        /// <summary>
        /// Get the request headers for DataCop
        /// </summary>
        /// <returns>The request headers for DataCop</returns>
        private static async Task<string> GetDataCopToken(string tenantId, string clientId, string clientSecret, string resource)
        {
            var authenticationContext = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}/", TokenCache.DefaultShared);
            ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
            var authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCred);
            return authenticationResult.AccessToken;
        }
    }
}