using Microsoft.Security.Application;
using Modulo.AuthorizedApplicationExample.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Security;
using WebApiContrib.Formatting.Html;

namespace Modulo.AuthorizedApplicationExample.Areas.Login
{
    public class LoginController : ApiController
    {
        ModuloRiskManagerConfig Config;
        ToAbsolutePath ToAbsolute;

        public LoginController(ModuloRiskManagerConfig config, ToAbsolutePath toAbsolute)
        {
            Config = config;
            ToAbsolute = toAbsolute;
        }

        [HttpGet, Route("~/Login")]
        public HttpResponseMessage Index()
        {
            var client_id = Config.Id;
            var callback = Encoder.UrlPathEncode(ToAbsolute("~/oauthcode"));
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(Config.BaseUri.ToString() + string.Format("/APIIntegration/AuthorizeFeatures?client_id={0}&response_type=code&redirect_uri={1}", client_id, callback));
            return response;
        }

        [HttpGet, Route("~/oauthcode")]
        public async Task<HttpResponseMessage> OAuthCode(string code)
        {
            var callback = Encoder.UrlPathEncode(ToAbsolute("~/"));

            var client = new HttpClient();
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("grant_type", "authorization_code");
            dictionary.Add("code", code);
            dictionary.Add("client_id", Config.Id);
            dictionary.Add("client_secret", Config.Key);
            dictionary.Add("redirect_uri", callback);
            var content = new FormUrlEncodedContent(dictionary);
            var tokenResponse = await client.PostAsync(Config.BaseUri.ToString() + "/APIIntegration/Token", content);
            var token = await tokenResponse.Content.ReadAsAsync<TokenResponseDto>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth2", token.access_token);

            var meResponse = await client.GetAsync(Config.BaseUri.ToString() + "/api/info/me");
            var me = await meResponse.Content.ReadAsAsync<InfoMeDto>();

            var tokenMe = new AccessTokenAndMe()
            {
                AccessToken = token,
                Me = me
            };

            var cookieToken = JsonConvert.SerializeObject(tokenMe);
            var bytes = System.Text.Encoding.UTF8.GetBytes(cookieToken);
            bytes = MachineKey.Protect(bytes, "COOKIE-TOKEN");
            cookieToken = Convert.ToBase64String(bytes);
            var cookie = new CookieHeaderValue("AUTH", cookieToken);            
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddMinutes(20);
            //cookie.Secure = true; //ALWAYS USE THIS IN PRODUCTION!!

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(ToAbsolute("~"));
            response.Headers.AddCookies(new[] { cookie });
            return response;
        }
    }

    public class InfoMeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }

    public class AccessTokenAndMe
    {
        public TokenResponseDto AccessToken { get; set; }
        public InfoMeDto Me { get; set; }
    }
}