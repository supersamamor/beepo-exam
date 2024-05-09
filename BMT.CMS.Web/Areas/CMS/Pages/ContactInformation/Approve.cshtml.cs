using BMT.CMS.Application.Features.CMS.Approval.Commands;
using BMT.CMS.Application.Features.CMS.Approval.Queries;
using BMT.CMS.Application.Features.CMS.ContactInformation.Queries;
using BMT.CMS.Web.Areas.CMS.Models;
using BMT.CMS.Web.Models;
using BMT.CMS.Core.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMT.CMS.Web.Areas.CMS.Pages.ContactInformation;

[Authorize(Policy = Permission.ContactInformation.Approve)]
public class ApproveModel : BasePageModel<ApproveModel>
{
    [BindProperty]
    public ContactInformationViewModel ContactInformation { get; set; } = new();
    [BindProperty]
    public string? ApprovalStatus { get; set; }
	[BindProperty]
	public string? ApprovalRemarks { get; set; }
    public async Task<IActionResult> OnGet(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        _ = (await Mediatr.Send(new GetApprovalStatusPerApproverByIdQuery(id, ApprovalModule.ContactInformation))).Select(l => ApprovalStatus = l);
        return await PageFrom(async () => await Mediatr.Send(new GetContactInformationByIdQuery(id)), ContactInformation);
    }

    public async Task<IActionResult> OnPost(string handler)
    {
        if (handler == "Approve")
        {
            return await Approve();
        }
        else if (handler == "Reject")
        {
            return await Reject();
        }
        return Page();
    }
    private async Task<IActionResult> Approve()
    {
        return await TryThenRedirectToPage(async () => await Mediatr.Send(new ApproveCommand(ContactInformation.Id, ApprovalRemarks, ApprovalModule.ContactInformation)), "Approve", true);
    }
    private async Task<IActionResult> Reject()
    {
        return await TryThenRedirectToPage(async () => await Mediatr.Send(new RejectCommand(ContactInformation.Id, ApprovalRemarks, ApprovalModule.ContactInformation)), "Approve", true);
    }
}
