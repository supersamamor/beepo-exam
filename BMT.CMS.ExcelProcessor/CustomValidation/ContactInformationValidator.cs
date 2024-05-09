using BMT.CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BMT.CMS.Core.CMS;
using BMT.CMS.ExcelProcessor.Models;
using BMT.CMS.ExcelProcessor.Helper;


namespace BMT.CMS.ExcelProcessor.CustomValidation
{
    public static class ContactInformationValidator
    {
        public static async Task<Dictionary<string, object?>>  ValidatePerRecordAsync(ApplicationContext context, Dictionary<string, object?> rowValue)
        {
			string errorValidation = "";
            var firstName = rowValue[nameof(ContactInformationState.FirstName)]?.ToString();
			if (!string.IsNullOrEmpty(firstName))
			{
				var firstNameMaxLength = 255;
				if (firstName.Length > firstNameMaxLength)
				{
					errorValidation += $"First Name should be less than {firstNameMaxLength} characters.;";
				}
			}
			var lastName = rowValue[nameof(ContactInformationState.LastName)]?.ToString();
			if (!string.IsNullOrEmpty(lastName))
			{
				var lastNameMaxLength = 255;
				if (lastName.Length > lastNameMaxLength)
				{
					errorValidation += $"Last Name should be less than {lastNameMaxLength} characters.;";
				}
			}
			var companyName = rowValue[nameof(ContactInformationState.CompanyName)]?.ToString();
			if (!string.IsNullOrEmpty(companyName))
			{
				var companyNameMaxLength = 255;
				if (companyName.Length > companyNameMaxLength)
				{
					errorValidation += $"Company Name should be less than {companyNameMaxLength} characters.;";
				}
			}
			var mobile = rowValue[nameof(ContactInformationState.Mobile)]?.ToString();
			if (!string.IsNullOrEmpty(mobile))
			{
				var mobileMaxLength = 255;
				if (mobile.Length > mobileMaxLength)
				{
					errorValidation += $"Mobile should be less than {mobileMaxLength} characters.;";
				}
			}
			var email = rowValue[nameof(ContactInformationState.Email)]?.ToString();
			if (!string.IsNullOrEmpty(email))
			{
				var emailMaxLength = 255;
				if (email.Length > emailMaxLength)
				{
					errorValidation += $"Email should be less than {emailMaxLength} characters.;";
				}
			}
			
			if (!string.IsNullOrEmpty(errorValidation))
			{
				throw new Exception(errorValidation);
			}
            return rowValue;
        }
			
		public static Dictionary<string, HashSet<int>> DuplicateValidation(List<ExcelRecord> records)
		{
			List<string> listOfKeys = new()
			{
																												
			};
			return listOfKeys.Count > 0 ? DictionaryHelper.FindDuplicateRowNumbersPerKey(records, listOfKeys) : new Dictionary<string, HashSet<int>>();
		}
    }
}
