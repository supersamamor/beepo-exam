using BMT.Common.Web.Utility.Extensions;
using BMT.CMS.Core.Identity;
using BMT.CMS.Web.Areas.Admin.Models;
using BMT.CMS.Web.Areas.Admin.Queries.Roles;
using BMT.CMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BMT.CMS.Web.Areas.Admin.Pages.Roles;

[Authorize(Policy = Permission.Roles.View)]
public class ViewModel : BasePageModel<ViewModel>
{
    readonly RoleManager<ApplicationRole> _roleManager;

    public ViewModel(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public RoleViewModel Role { get; set; } = new();
    public IList<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();

    public async Task<IActionResult> OnGet(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        return await Mediatr.Send(new GetRoleByIdQuery(id))
                            .ToActionResult(async role =>
                            {
                                Role = Mapper.Map<RoleViewModel>(role);
                                var roleClaims = (await _roleManager.GetClaimsAsync(role)).Map(c => c.Value);
                                Permissions = Permission.GenerateAllPermissions().Map(p => new PermissionViewModel
                                {
                                    Permission = p,
                                    Enabled = roleClaims.Any(c => c == p)
                                }).ToList();
                                return Page();
                            }, none: null);
    }
}
