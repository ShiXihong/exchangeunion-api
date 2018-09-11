using API.WebServices.Models;
using API.WebServices.Utils;
using MailChimp.Net;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Services
{
    public class SendEmail
    {
        private readonly EmailConfig _emailConfig;
        private readonly MailChimpConfig _mailChimpConfig;
        private readonly string _categroy;

        public SendEmail(IOptions<EmailConfigList> emailConfigList, IOptions<MailChimpConfig> mailChimpConfig, string categroy)
        {
            if (categroy == EnumUtils.Category.ExchangeUnion.ToString())
            {
                _emailConfig = emailConfigList.Value.XUCEmailConfig;
            }
            else if (categroy == EnumUtils.Category.DigitalFinanceGroup.ToString())
            {
                _emailConfig = emailConfigList.Value.DFGEmailConfig;
            }

            _categroy = categroy;
            _mailChimpConfig = mailChimpConfig.Value;
        }

        public Boolean SendEmailSync(string email, string subject, string message, string textpart = "p")
        {
            textpart = textpart == "p" ? "plain" : "html";
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.Form));
                string[] emailArray = email.Split(',');
                foreach (var item in emailArray)
                {
                    emailMessage.To.Add(new MailboxAddress(item));
                }
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(textpart) { Text = message };
                using (var client = new SmtpClient())
                {
                    client.Connect(_emailConfig.Host, 465, true);
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<EmailResult> SendEmailAsync(string email, string subject, string message)
        {
            EmailResult result = new EmailResult();
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.Form));
                emailMessage.To.Add(new MailboxAddress("mail", email));
                emailMessage.Bcc.Add(new MailboxAddress("mail", _emailConfig.Form));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailConfig.Host, 587, SecureSocketOptions.StartTls).ConfigureAwait(false);
                    //await client.ConnectAsync(_emailConfig.Host, 587, false).ConfigureAwait(false);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    //client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
                result.Success = true;
                result.Message = "send success";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.ToString();
                return result;
            }
        }

        public async Task<bool> AddMemberToMailChimp(string email)
        {
            try
            {
                DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
                int timeStamp = Convert.ToInt32((DateTime.Now - dateStart).TotalSeconds);

                IMailChimpManager manager = new MailChimpManager(_mailChimpConfig.APIKey);
                // Use the Status property if updating an existing member
                var member = new Member { EmailAddress = email, StatusIfNew = Status.Subscribed };     
                string listId = "", FName = "";
                if(_categroy == EnumUtils.Category.ExchangeUnion.ToString())
                {
                    listId = _mailChimpConfig.XUCListID;
                    FName = "Exchange Union";
                }
                else if (_categroy == EnumUtils.Category.DigitalFinanceGroup.ToString())
                {
                    listId = _mailChimpConfig.DFGListID;
                    FName = "Digital Finance Group";
                }

                member.MergeFields.Add("FNAME", FName);
                member.MergeFields.Add("LNAME", timeStamp);
                await manager.Members.AddOrUpdateAsync(listId, member);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    
}
