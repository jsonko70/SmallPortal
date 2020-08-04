using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Logging;
using SmallPortal.Data;
using Microsoft.AspNetCore.Authorization;

namespace SmallPortal.Controllers
{
    public class ContactController : Controller
    {
        //string username = "";
        //string pasword = "";
        //string emailPrivate = "";

        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        public string PostedMessage { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            return Redirect("/contact");
        }

        public void OnGet(int id)
        {
            ViewData["PostedMessage"] = "Your message has been sent";
            PostedMessage = "Your message has been sent";
        }
        public void SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("John Doe", "test@tester.com"));
            message.To.Add(new MailboxAddress("Team", "team@smallportal.com"));
            message.Subject = "Email from Small Portal App";
            message.Body = new TextPart("plain")
            {
                Text = "hello world from app"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("username", "password");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}