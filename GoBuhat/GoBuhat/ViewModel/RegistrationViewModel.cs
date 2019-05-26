using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Data;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Collections.Specialized;
using GoBuhat.Common;
using System.Threading;

namespace GoBuhat.ViewModel
{
    class RegistrationViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public Action OpenMainPage;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private byte[] result;

        private string username;
        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmPassword"));
            }
        }

        public ICommand SubmitCommand { protected set; get; }
        public RegistrationViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public void OnSubmit()
        {

            if (!ControlInput())
            {
                CommonUIHelper.CreateToast("Check something");
                return;
            }

            SendNewUserToPHP();
        }

        private bool ControlInput()
        {
            // TODO: create conditions for user_name, user_email and password compares to password confirm and its minimal length

            return true;
        }

        private void SendNewUserToPHP()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/register.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("user_name", UserName);
            parameters.Add("user_email", Email);
            parameters.Add("user_password", Password);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                //client.UploadValuesCompleted += this.client_UploadDataCompleted;
                result = client.UploadValues(uri, parameters);

                Console.WriteLine(CommonUIHelper.ConvertResult(result));

                OpenMainPage();
            }).Start();
        }
    }
}