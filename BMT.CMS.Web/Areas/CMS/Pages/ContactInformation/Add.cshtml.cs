using BMT.CMS.Application.Features.CMS.ContactInformation.Commands;
using BMT.CMS.Web.Areas.CMS.Models;
using BMT.CMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMT.CMS.Web.Areas.CMS.Pages.ContactInformation;

[Authorize(Policy = Permission.ContactInformation.Create)]
public class AddModel : BasePageModel<AddModel>
{
    [BindProperty]
    public ContactInformationViewModel ContactInformation { get; set; } = new();
    [BindProperty]
    public string? RemoveSubDetailId { get; set; }
    [BindProperty]
    public string? AsyncAction { get; set; }
    public IActionResult OnGet()
    {
		
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
		
        if (!ModelState.IsValid)
        {
            return Page();
        }
		
        return await TryThenRedirectToPage(async () => await Mediatr.Send(Mapper.Map<AddContactInformationCommand>(ContactInformation)), "Details", true);
    }	
	public IActionResult OnPostChangeFormValue()
    {
        ModelState.Clear();
		
        return Partial("_InputFieldsPartial", ContactInformation);
    }
	
}
