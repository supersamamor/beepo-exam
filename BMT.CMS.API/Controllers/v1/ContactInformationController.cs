using BMT.Common.Utility.Models;
using BMT.CMS.Application.Features.CMS.ContactInformation.Commands;
using BMT.CMS.Application.Features.CMS.ContactInformation.Queries;
using BMT.CMS.Core.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BMT.Common.API.Controllers;
using Asp.Versioning;

namespace BMT.CMS.API.Controllers.v1;

[ApiVersion("1.0")]
public class ContactInformationController : BaseApiController<ContactInformationController>
{
    [Authorize(Policy = Permission.ContactInformation.View)]
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<ContactInformationState>>> GetAsync([FromQuery] GetContactInformationQuery query) =>
        Ok(await Mediator.Send(query));

    [Authorize(Policy = Permission.ContactInformation.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactInformationState>> GetAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new GetContactInformationByIdQuery(id)));

    [Authorize(Policy = Permission.ContactInformation.Create)]
    [HttpPost]
    public async Task<ActionResult<ContactInformationState>> PostAsync([FromBody] ContactInformationViewModel request) =>
        await ToActionResult(async () => await Mediator.Send(Mapper.Map<AddContactInformationCommand>(request)));

    [Authorize(Policy = Permission.ContactInformation.Edit)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ContactInformationState>> PutAsync(string id, [FromBody] ContactInformationViewModel request)
    {
        var command = Mapper.Map<EditContactInformationCommand>(request);
        return await ToActionResult(async () => await Mediator.Send(command with { Id = id }));
    }

    [Authorize(Policy = Permission.ContactInformation.Delete)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ContactInformationState>> DeleteAsync(string id) =>
        await ToActionResult(async () => await Mediator.Send(new DeleteContactInformationCommand { Id = id }));
}

public record ContactInformationViewModel
{
    [Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string FirstName { get;set; } = "";
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string LastName { get;set; } = "";
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string CompanyName { get;set; } = "";
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Mobile { get;set; } = "";
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string Email { get;set; } = "";
	   
}
