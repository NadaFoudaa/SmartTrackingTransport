using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Services.Services.IEmailService
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _config;
		public EmailService(IConfiguration config)
		{
			_config = config;
		}

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress("No Reply", _config["Email:From"]));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;

			// Email body
			email.Body = new TextPart("html")
			{
				Text = body
			};

			using var smtp = new MailKit.Net.Smtp.SmtpClient();
			try
			{
				// Connect to the SMTP server
				await smtp.ConnectAsync(_config["Email:SmtpServer"], int.Parse(_config["Email:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

				// Authenticate with the server
				await smtp.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);

				// Send the email
				await smtp.SendAsync(email);
			}
			catch (Exception ex)
			{
				// Log or handle the exception
				throw new Exception($"Failed to send email: {ex.Message}", ex);
			}
			finally
			{
				// Disconnect from the SMTP server
				await smtp.DisconnectAsync(true);
			}
		}
	}
}