// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StickyNotesDemo
{
    internal class Email
    {
        private readonly SmtpClient _client;
        private readonly EmailDialog _dia;
        private readonly MailMessage _message;

        public Email(string server, string login, int port, bool sslCheck, string toAddr, string fromAddr,
            string password,
            string bodyMessage, EmailDialog emailDia, string subject)
        {
            _dia = emailDia;
            try
            {
                var nw = new NetworkCredential(login, password);
                // Command line argument must be the SMTP host.
                _client = new SmtpClient(server)
                {
                    Credentials = nw,
                    EnableSsl = sslCheck ? true : false
                };
                // Specify the e-mail sender.
                // Create a mailing address that includes a UTF8 character
                // in the display name.

                var from = new MailAddress(fromAddr, "", Encoding.UTF8);
                _client.Port = port;
                // Set destinations for the e-mail message.
                var to = new MailAddress(toAddr);
                // Specify the message content.
                _message = new MailMessage(from, to)
                {
                    Body = bodyMessage,
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8
                };


                // Set the method that is called back when the send operation ends.
                _client.SendCompleted += SendCompletedCallback;
                // The userState can be any object that allows your callback 
                // method to identify this send operation.
                // For this example, the userToken is a string constant.
                var userState = "Sticky note message";
                _client.SendAsync(_message, userState);
            }
            catch (Exception e)
            {
                CreateDialog(e.Message);
            }
        }

        public bool MailSent { get; private set; }

        public void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            var token = (string) e.UserState;


            if (e.Error != null)
            {
                _client.SendAsyncCancel();
                CreateDialog(e.Error.ToString());
                MailSent = false;
            }
            else
            {
                CreateDialog("Message Sent");
                MailSent = true;
                _dia.Close();
            }
            _message.Dispose();
        }

        private static void CreateDialog(string msg)
        {
            var w = new Window();
            var sp = new StackPanel {Background = Brushes.Transparent};
            var tb = new TextBlock
            {
                Background = sp.Background,
                TextWrapping = TextWrapping.Wrap,
                Text = msg
            };
            sp.Children.Add(tb);

            w.Content = sp;
            w.WindowStyle = WindowStyle.ToolWindow;
            w.Background = Window2.ChangeBackgroundColor(Colors.Wheat);
            w.Height = 250;
            w.Width = 600;
            w.ShowDialog();
        }
    }
}