// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StickyNotesDemo
{
    /// <summary>
    ///     Interaction logic for EmailDialog.xaml
    /// </summary>
    public class EmailDialog : Window
    {
        private TextBox _body;
        private TextBox _from;
        private PasswordBox _password;
        private TextBox _subj;
        private TextBox _to;

        public EmailDialog(string message)
        {
            WindowStyle = WindowStyle.ToolWindow;
            Height = Width = 600;
            Title = "Email";
            Background = Window2.ChangeBackgroundColor(Colors.BlanchedAlmond);


            if (File.Exists("MailServerInfo"))
            {
                CreateMailUi(message);
                ShowDialog();
            }
            else
            {
                var msd = new MailSettingsDialog();
                msd.Show();
            }
        }

        private void CreateMailUi(string message)
        {
            var sp = new StackPanel {Background = Brushes.Transparent};

            var g = new Grid {Background = Brushes.Transparent};
            var rdef1 = new RowDefinition();
            var rdef2 = new RowDefinition();
            var rdef3 = new RowDefinition();
            var rdef4 = new RowDefinition();
            var rdef5 = new RowDefinition();
            g.RowDefinitions.Add(rdef1);
            g.RowDefinitions.Add(rdef2);
            g.RowDefinitions.Add(rdef3);
            g.RowDefinitions.Add(rdef4);
            g.RowDefinitions.Add(rdef5);

            var cd1 = new ColumnDefinition();
            var gdl = new GridLength(80);
            cd1.Width = gdl;
            var cd2 = new ColumnDefinition();
            g.ColumnDefinitions.Add(cd1);
            g.ColumnDefinitions.Add(cd2);

            var l00 = new Label
            {
                Background = Brushes.Transparent,
                Content = "Login:"
            };
            var login = new TextBox();
            var email = GetEmail();
            var index = email.IndexOf('@');
            if (index < 0)
            {
                login.Text = "Pls correct incorrect email in Mail settings";
            }
            else
            {
                var loginText = email.Substring(0, index);
                login.Text = loginText;
            }

            Grid.SetRow(l00, 0);
            Grid.SetColumn(l00, 0);
            Grid.SetRow(login, 0);
            Grid.SetColumn(login, 1);

            var l0 = new Label
            {
                Background = Brushes.Transparent,
                Content = "Password:"
            };
            _password = new PasswordBox {PasswordChar = '*'};


            Grid.SetRow(l0, 1);
            Grid.SetColumn(l0, 0);
            Grid.SetRow(_password, 1);
            Grid.SetColumn(_password, 1);

            var l1 = new Label
            {
                Background = Brushes.Transparent,
                Content = "To:"
            };
            _to = new TextBox();

            Grid.SetRow(l1, 2);
            Grid.SetColumn(l1, 0);
            Grid.SetRow(_to, 2);
            Grid.SetColumn(_to, 1);

            var l2 = new Label
            {
                Background = Brushes.Transparent,
                Content = "From:"
            };
            _from = new TextBox {Text = email};

            Grid.SetRow(l2, 3);
            Grid.SetColumn(l2, 0);
            Grid.SetRow(_from, 3);
            Grid.SetColumn(_from, 1);

            var l5 = new Label
            {
                Background = Brushes.Transparent,
                Content = "Subj:"
            };
            _subj = new TextBox();

            Grid.SetRow(l5, 4);
            Grid.SetColumn(l5, 0);
            Grid.SetRow(_subj, 4);
            Grid.SetColumn(_subj, 1);

            g.Children.Add(l00);
            g.Children.Add(login);
            g.Children.Add(l0);
            g.Children.Add(_password);
            g.Children.Add(l1);
            g.Children.Add(_to);
            g.Children.Add(l2);
            g.Children.Add(_from);
            g.Children.Add(l5);
            g.Children.Add(_subj);

            _body = new TextBox
            {
                Text = message,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 250,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            var ok = new Button
            {
                Content = "Ok",
                HorizontalAlignment = HorizontalAlignment.Right
            };
            ok.Click += ok_Click;

            sp.Children.Add(g);
            sp.Children.Add(_body);
            sp.Children.Add(ok);

            Height = 500;
            Content = sp;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            var fs = new FileStream("MailServerInfo", FileMode.Open);
            var reader = new StreamReader(fs);
            var server = reader.ReadLine();
            var account = reader.ReadLine();
            var index = account.IndexOf('@');
            index = (index < 0) ? 0 : index;
            account = account.Substring(0, index);
            var portString = reader.ReadLine();
            var port = (portString != string.Empty) ? int.Parse(portString) : 25;

            var sslCheck = reader.ReadLine().Contains("true") ? true : false;

            var toEmail = _to.Text;
            var fromEmail = _from.Text;
            var password = _password.Password;
            var subject = _subj.Text;
            var bodyMessage = _body.Text;

            reader.Close();
            fs.Close();

            var email = new Email(server, account, port, sslCheck, toEmail, fromEmail, password, bodyMessage, this,
                subject);
        }

        private string GetEmail()
        {
            var fs = new FileStream("MailServerInfo", FileMode.Open);
            var reader = new StreamReader(fs);
            reader.ReadLine();
            var str = reader.ReadLine();
            reader.Close();
            fs.Close();
            return str;
        }
    }
}