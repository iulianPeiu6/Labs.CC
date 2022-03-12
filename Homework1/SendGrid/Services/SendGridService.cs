using Microsoft.Extensions.Options;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;
using Newtonsoft.Json;
using SendGrid.Options;
using SendGrid.Services.Abstracts;
using System.Net.Mail;
using System.Text;

namespace SendGrid.Services
{
    public class SendGridService : SendGridServiceBase, ISendGridService
    {
        public SendGridService(IOptions<SendGridConfiguration> config, IRequestLogRepository requestLogs) : base(config, requestLogs)
        {
        }

        public async Task<bool> SendAsync(Mail mail)
        {
            Validate(mail);

            var request = new HttpRequestMessage(HttpMethod.Post, config.WebApiEndpoint);
            request.Headers.Add("Authorization", $"Bearer {config.ApiKey}");
            request.Content = BuildRequestContentForSend(mail);

            var success = await MakeRequest(request);

            return success;
        }

        private void Validate(Mail mail)
        {
            if (string.IsNullOrWhiteSpace(mail.SenderEmailAddress))
            {
                mail.SenderEmailAddress = config.SenderEmailAddress;
            }

            if (string.IsNullOrWhiteSpace(mail.Subject))
            {
                mail.Subject = config.BaseSubject;
            }

            if (!IsValidEmailAddress(mail.ReceiverEmailAddress))
            {
                throw new ArgumentException($"Receiver Email Address: '{mail.ReceiverEmailAddress}' is invalid!");
            }
        }

        private HttpContent? BuildRequestContentForSend(Mail mail)
        {

            var contentObject = new
            {
                personalizations = new[]
                {
                    new
                    {
                        to = new[]
                        {
                            new { email = mail.ReceiverEmailAddress }
                        }
                    }
                },
                from = new { email = mail.SenderEmailAddress },
                subject = mail.Subject,
                content = new[]
                {
                    new { type = "text/html", value = mail.Body }
                }
            };

            string content = JsonConvert.SerializeObject(contentObject);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        private static bool IsValidEmailAddress(string email)
        {
            try
            {
                var emailAddress = new MailAddress(email);
                return emailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
