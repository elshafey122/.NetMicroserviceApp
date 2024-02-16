using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Mongo.EmailApi.Data;
using Mongo.EmailApi.Model;
using Mongo.EmailApi.Model.Dto;
using System;
using System.Text;

namespace Mongo.EmailApi.NewFolder
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDbContext> _dboptions;

        public EmailService(DbContextOptions<ApplicationDbContext> options)
        {
            _dboptions = options;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.cartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.cartDetails)
            {
                message.Append("<li>");
                message.Append(item.product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.cartHeader.Email); // send email in db and save it
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new EmailLogger
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new ApplicationDbContext(_dboptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
