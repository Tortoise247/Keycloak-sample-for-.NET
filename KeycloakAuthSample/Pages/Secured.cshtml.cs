using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeycloakAuthSample.Pages
{
    [Authorize]
    public class SecuredModel : PageModel
    {
        public string Message { get; private set; }

        public void OnGet()
        {
            Message = "認証できた！";
        }
    }
}