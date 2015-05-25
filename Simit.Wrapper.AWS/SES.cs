namespace Simit.Wrapper.AWS
{
    #region Using Directive

    using Amazon;
    using Amazon.SimpleEmail;
    using Amazon.SimpleEmail.Model;
    using System;
    using System.Collections.Generic;

    #endregion Using Directive

    public class SES
    {
        #region Private Fields

        private AmazonSimpleEmailServiceClient serviceClient;
        private const string EmailAddress = "aws:ses:fromemail";
        private const string AccessKeyIDName = "aws:ses:accesskey";
        private const string SecretAccessKey = "aws:ses:secretkey";

        #endregion Private Fields

        #region Public Constructor

        public SES()
        {
            string[] requiredKeys = new string[] { EmailAddress, AccessKeyIDName, SecretAccessKey };

            foreach (string key in requiredKeys)
            {
                if (System.Configuration.ConfigurationManager.AppSettings[key] == null)
                {
                    throw new Exception(key + " not found in appSettings");
                }
            }
        }

        #endregion Public Constructor

        #region Private Properties

        private string SenderEmailAddress
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings[EmailAddress];
            }
        }

        private AmazonSimpleEmailServiceClient ServiceClient
        {
            get
            {
                if (this.serviceClient == null)
                    this.serviceClient = new AmazonSimpleEmailServiceClient(System.Configuration.ConfigurationManager.AppSettings[AccessKeyIDName], System.Configuration.ConfigurationManager.AppSettings[SecretAccessKey], RegionEndpoint.USEast1);

                return this.serviceClient;
            }
        }

        #endregion Private Properties

        public void SendEmail(string to, string subject, string htmlBody, List<string> bcc = null)
        {
            this.SendEmail(new List<string> { to }, subject, htmlBody, bcc);
        }

        public void SendEmail(List<string> to, string subject, string htmlBody, List<string> bcc = null)
        {
            Destination destination = new Destination();
            destination.ToAddresses = to;
            destination.BccAddresses = bcc;

            Content subjectContent = new Content { Charset = "UTF-8", Data = subject };
            Content bodyContent = new Content { Charset = "UTF-8", Data = htmlBody };

            SendEmailRequest request = new SendEmailRequest()
            {
                Destination = destination,
                Source = this.SenderEmailAddress,
                ReturnPath = this.SenderEmailAddress,
                Message = new Message(subjectContent, new Body() { Html = bodyContent })
            };

            this.ServiceClient.SendEmail(request);
        }
    }
}