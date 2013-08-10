// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SendGridLogger.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The sendgrid logger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Logger.SendGrid
{
    using System.Configuration;
    using System.Net.Mail;
    using System.Text;

    using STrackerServer.Logger.Core;

    /// <summary>
    /// The SendGrid logger.
    /// </summary>
    public class SendGridLogger : BaseLogger
    {
        /// <summary>
        /// The SMTP client.
        /// </summary>
        private readonly SmtpClient smtpClient;

        /// <summary>
        /// The STracker email.
        /// </summary>
        private readonly string strackerEmail = ConfigurationManager.AppSettings["STracker:Email"];

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridLogger"/> class.
        /// </summary>
        /// <param name="smtpClient">
        /// The SMTP client.
        /// </param>
        public SendGridLogger(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        } 

        /// <summary>
        /// Send debug log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Debug(string message)
        {
            this.SendMail("Debug", message);
        }

        /// <summary>
        /// Send info log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Info(string message)
        {
            this.SendMail("Info", message);
        }

        /// <summary>
        /// Send warn log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Warn(string message)
        {
            this.SendMail("Warn", message);
        }

        /// <summary>
        /// Send error log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Error(string message)
        {
            this.SendMail("Error", message);
        }

        /// <summary>
        /// The fatal log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Fatal(string message)
        {
            this.SendMail("Fatal", message);
        }

        /// <summary>
        /// The send mail.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        private void SendMail(string type, string message)
        {
            var mailMsg = new MailMessage();
            mailMsg.To.Add(new MailAddress(this.strackerEmail));
            mailMsg.Subject = "Log type: " + type;
            mailMsg.Body = message;
            mailMsg.BodyEncoding = Encoding.ASCII;
            this.smtpClient.Send(mailMsg);
        }
    }
}
