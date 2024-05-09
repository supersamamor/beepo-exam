using BMT.Common.Web.Utility.Extensions;
using BMT.CMS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using BMT.Common.Web.Utility.Annotations;

namespace BMT.CMS.Web.Areas.CMS.Models;

public record ContactInformationViewModel : BaseViewModel
{	
	[Display(Name = "First Name")]
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string FirstName { get; init; } = "";
	[Display(Name = "Last Name")]
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string LastName { get; init; } = "";
	[Display(Name = "Company Name")]
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	public string CompanyName { get; init; } = "";
	[Display(Name = "Mobile")]
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
	[Phone]
	public string Mobile { get; init; } = "";
	[Display(Name = "Email")]
	[Required]
	[StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
    [EmailAddress]
    public string Email { get; init; } = "";
	
	public DateTime LastModifiedDate { get; set; }
		
	
}
