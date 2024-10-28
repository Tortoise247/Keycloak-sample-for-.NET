using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace KeycloakAuthSample.Pages
{
    public class SignoutModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SignoutModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            // Keycloakのサインアウトエンドポイントを呼び出す
            var client = _httpClientFactory.CreateClient();
            var keycloakSignoutUrl = "http://localhost:8080/realms/myrealm/protocol/openid-connect/logout";

            var request = new HttpRequestMessage(HttpMethod.Post, keycloakSignoutUrl);
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "myclient"),
                new KeyValuePair<string, string>("client_secret", "CdLV18B6zFBvUWCtWGEAkzwlTkYmwI88"),
                new KeyValuePair<string, string>("refresh_token", await HttpContext.GetTokenAsync("refresh_token"))
            });

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // エラーハンドリング
                return StatusCode((int)response.StatusCode, "Keycloakサインアウトエンドポイントへのリクエストが失敗しました。");
            }

            // セッションをクリア
            HttpContext.Session.Clear();

            // 認証クッキーを削除
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            return RedirectToPage("/SignoutCallback");
        }
    }
}