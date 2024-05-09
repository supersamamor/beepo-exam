using BMT.CMS.Application.Features.CMS.ContactInformation.Queries;
using BMT.CMS.Web.Areas.CMS.Models;
using BMT.CMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMT.CMS.Web.Areas.CMS.Pages.ContactInformation;

[Authorize(Policy = Permission.ContactInformation.View)]
public class DetailsModel : BasePageModel<DetailsModel>
{
    public ContactInformationViewModel ContactInformation { get; set; } = new();
	[BindProperty]
    public string? RemoveSubDetailId { get; set; }
    [BindProperty]
    public string? AsyncAction { get; set; }
    public async Task<IActionResult> OnGet(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        return await PageFrom(async () => await Mediatr.Send(new GetContactInformationByIdQuery(id)), ContactInformation);
    }
}
