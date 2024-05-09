using BMT.Common.Core.Base.Models;
using System.ComponentModel;

namespace BMT.CMS.Core.CMS;

public record ContactInformationState : BaseEntity
{
	public string FirstName { get; init; } = "";
	public string LastName { get; init; } = "";
	public string CompanyName { get; init; } = "";
	public string Mobile { get; init; } = "";
	public string Email { get; init; } = "";
	
	
	
}
