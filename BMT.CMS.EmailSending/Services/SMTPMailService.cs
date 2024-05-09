using BMT.Common.Services.Shared.Interfaces;
using BMT.Common.Services.Shared.Models.Mail;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace BMT.CMS.EmailSending.Services
{
    public class SMTPEmailService(IOptions<MailSettings> settings, IConfiguration Configuration) : IMailService
	{
		private readonly MailSettings _settings = settings.Value;
		public async Task SendAsync(MailRequest request, CancellationToken cancellationToken = default)
		{  
			var timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			timeoutCancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(_settings.TimeOutMinute));
			if (Configuration.GetValue<bool>("IsTest"))
			{
				if (!string.IsNullOrEmpty(_settings.TestEmailRecipient))
				{
					string[] testEmails = _settings.TestEmailRecipient!.Split(',');
					request.To = testEmails[0];
				}
				else
				{
					request.To = "";
				}            
				request.Ccs = null;
				request.Bcc = null;
				request.Subject += " - Test";
			}
			var decryptedPassword = DecryptPassword(_settings.SMTPEmailPassword!, _settings.SMTPEmail!);
			var mailMessage = new MailMessage(_settings.SMTPEmail!, request.To, request.Subject, request.Body)
			{
				IsBodyHtml = true,
				BodyEncoding = System.Text.Encoding.UTF8,
				SubjectEncoding = System.Text.Encoding.UTF8
			};
			if (request.Attachments != null && request.Attachments.Count > 0)
			{
				foreach (var item in request.Attachments)
				{
					if (File.Exists(item))
					{
						mailMessage.Attachments.Add(new Attachment(item));
					}
				}
			}
			// Add Cc recipients
			if (request.Ccs != null)
			{
				foreach (string cc in request.Ccs)
				{
					mailMessage.CC.Add(cc);
				}
			}
			// Add Bcc recipients
			if (request.Bcc != null)
			{
				foreach (string bcc in request.Bcc)
				{
					mailMessage.Bcc.Add(bcc);
				}
			}
			try
			{
				using var client = new SmtpClient();
				client.Host = _settings.SMTPHost!;
				client.Port = _settings.SMTPPort;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				if (!string.IsNullOrEmpty(_settings.SMTPEmailPassword))
				{
					client.EnableSsl = true;
					client.Credentials = new System.Net.NetworkCredential(_settings.SMTPEmail!, decryptedPassword);
				}
				else
				{
					client.EnableSsl = false;
				}
				await client.SendMailAsync(mailMessage, timeoutCancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{
				if (timeoutCancellationTokenSource.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
				{
					throw new TimeoutException($"The email sending operation timed out after {_settings.TimeOutMinute} minute(s).");
				}
				throw; // If the cancellationToken parameter was cancelled, rethrow the original OperationCanceledException
			}
		}  
		private static string EncryptPassword(string password, string emailAsKey)
		{
			if (string.IsNullOrEmpty(password)) { return ""; }
			byte[] encryptedBytes;
			var key = DeriveKeyFromEmail(emailAsKey);
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = new byte[16]; // Initialization Vector
				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
				byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
				encryptedBytes = encryptor.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);
			}
			return Convert.ToBase64String(encryptedBytes);
		}
		private static string DecryptPassword(string encryptedPassword, string emailAsKey)
		{

			if (string.IsNullOrEmpty(encryptedPassword)) { return ""; }
			byte[] decryptedBytes;
			var key = DeriveKeyFromEmail(emailAsKey);
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = new byte[16]; // Initialization Vector
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
				byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
				decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
			}
			return Encoding.UTF8.GetString(decryptedBytes);
		} 
		private static byte[] DeriveKeyFromEmail(string email)
		{
			byte[] emailBytes = Encoding.UTF8.GetBytes(email);
			return SHA256.HashData(emailBytes);
		}
	}
}
