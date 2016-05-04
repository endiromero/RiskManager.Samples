# RiskManager.Samples
Samples using the Modulo Risk Manager platform.

## Authenticated Access to Risk Manager

1 - Redirect the user to the Risk Manager authorization page.

https://github.com/modulogrc/RiskManager.Samples/blob/master/source/Modulo.AuthorizedApplicationExample/Controllers/LoginController.cs#L27

    [HttpGet, Route("~/Login")]
    public HttpResponseMessage Index()
    {
        var client_id = Config.Id;
        var callback = Encoder.UrlPathEncode(ToAbsolute("~/oauthcode"));
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);
        response.Headers.Location = new Uri(Config.BaseUri.ToString() + string.Format("/APIIntegration/AuthorizeFeatures?client_id={0}&response_type=code&redirect_uri={1}", client_id, callback));
        return response;
    }

2 - Process the OAuth code
3 - Call Risk Manager to get the access token
4 - Call Risk Manager to get user information
5 - Save the user information and the access token in the cookie using the MachineKey cryptography

https://github.com/modulogrc/RiskManager.Samples/blob/master/source/Modulo.AuthorizedApplicationExample/Controllers/LoginController.cs#L37

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
