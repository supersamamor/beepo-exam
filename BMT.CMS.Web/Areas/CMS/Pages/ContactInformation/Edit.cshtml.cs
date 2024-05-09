using BMT.CMS.Application.Features.CMS.ContactInformation.Commands;
using BMT.CMS.Application.Features.CMS.ContactInformation.Queries;
using BMT.CMS.Web.Areas.CMS.Models;
using BMT.CMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMT.CMS.Web.Areas.CMS.Pages.ContactInformation;

[Authorize(Policy = Permission.ContactInformation.Edit)]
public class EditModel : BasePageModel<EditModel>
{
    [BindProperty]
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

    public async Task<IActionResult> OnPost()
    {		
        if (!ModelState.IsValid)
        {
            return Page();
        }
		
        return await TryThenRedirectToPage(async () => await Mediatr.Send(Mapper.Map<EditContactInformationCommand>(ContactInformation)), "Details", true);
    }	
	public IActionResult OnPostChangeFormValue()
    {
        ModelState.Clear();
		
        return Partial("_InputFieldsPartial", ContactInformation);
    }
	
}
