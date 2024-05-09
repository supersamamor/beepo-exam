using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BMT.CMS.Web.Areas.Identity.Pages.Account;
[AllowAnonymous]
public class NotActiveModel : PageModel
{
    public void OnGet()
    {
    }
}
