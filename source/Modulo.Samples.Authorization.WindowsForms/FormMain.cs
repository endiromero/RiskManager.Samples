using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modulo.Samples.Authorization.WindowsForms
{
    public partial class FormMain : Form
    {
        public ChromiumWebBrowser browser;

        public FormMain()
        {
            InitializeComponent();

            Cef.Initialize(new CefSettings()
            {
                IgnoreCertificateErrors = true
            });
            browser = new ChromiumWebBrowser("about:blank");
            browser.FrameLoadStart += Browser_FrameLoadStart;
            tableLayoutPanel1.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private async void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (e.Url.StartsWith("http://localhost/app"))
            {
                MessageBox.Show(e.Url);

                var url = "https://build.dev.modulo.com/RM_EN_FULL";
                var clientId = textBoxClientId.Text;
                var clientSecret = textBoxClientSecret.Text;
                var callback = callbackUrlTextBox.Text;

                var uri = new Uri(e.Url);
                var parameters = ParseQueryString(uri);
                var code = parameters["code"];

                var client = new HttpClient();
                var dictionary = new Dictionary<string, string>();
                dictionary.Add("grant_type", "authorization_code");
                dictionary.Add("code", code);
                dictionary.Add("client_id", clientId);
                dictionary.Add("client_secret", clientSecret);
                dictionary.Add("redirect_uri", callback);
                var content = new FormUrlEncodedContent(dictionary);
                var tokenResponse = await client.PostAsync(url + "/APIIntegration/Token", content);
                var token = await tokenResponse.Content.ReadAsAsync<TokenResponseDto>();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth2", token.access_token);

                var meResponse = await client.GetAsync(url + "/api/info/me");
                var me = await meResponse.Content.ReadAsAsync<InfoMeDto>();

                textBoxMyLogin.Invoke(new Action(() =>
                {
                    textBoxMyLogin.Text = me.Name;
                }));
            }
        }

        public class InfoMeDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
            public string Email { get; set; }
        }

        public class TokenResponseDto
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var url = "https://build.dev.modulo.com/RM_EN_FULL";
            var clientId = textBoxClientId.Text;
            var callback = callbackUrlTextBox.Text;
            var uri = new Uri(url + string.Format("/APIIntegration/AuthorizeFeatures?client_id={0}&response_type=code&redirect_uri={1}", clientId, callback));
            browser.Load(uri.ToString());
        }

        private static readonly Regex _regex = new Regex(@"[?|&]([\w\.]+)=([^?|^&]+)");

        public static IReadOnlyDictionary<string, string> ParseQueryString(Uri uri)
        {
            var match = _regex.Match(uri.PathAndQuery);
            var paramaters = new Dictionary<string, string>();
            while (match.Success)
            {
                paramaters.Add(match.Groups[1].Value, match.Groups[2].Value);
                match = match.NextMatch();
            }
            return paramaters;
        }
    }
}
