using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Data;
using GoBuhat.Common;
using System.Net;
using System.Collections.Specialized;
using System.Threading;
using GoBuhat.Utils;

namespace GoBuhat.ViewModel
{
    class LoginViewModel
    {
        public Action DisplayInvalidLoginPrompt;

        public string Id { get; set; }

        private byte[] result;

        private string username;
        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }
        private bool _isSaveSwitchToggled = false;
        public bool IsSaveSwitchToggled
        {
            get
            {
                return _isSaveSwitchToggled;
            }
            set
            {
                _isSaveSwitchToggled = value;
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);

            if (!string.IsNullOrEmpty(Settings.UserName))
                IsSaveSwitchToggled = true;
        }
        public void OnSubmit()
        {
            //if (!UserExist())
            //{
            //    CommonUIHelper.CreateToast("User does not exist");
            //    return;
            //}

            SendAuthorizationToPHP();
        }

        private void SendAuthorizationToPHP()
        {
            // TODO: send request to "https://adwatcherapplication.000webhostapp.com/login.php"
            
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/login.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("user_name", UserName);
            parameters.Add("user_password", Password);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                //client.UploadValuesCompleted += this.client_UploadDataCompleted;
                result = client.UploadValues(uri, parameters);

                string res = CommonUIHelper.ConvertResult(result);

                if (!UserExist(res))
                {
                    //CommonUIHelper.CreateToast("User does not exist");

                    Console.WriteLine("User doesn't exist");
                    return;
                }

                SaveUser();

                //Console.WriteLine(CommonUIHelper.ConvertResult(result) + " Name: " + UserName + "; Password: " + Password);

                OpenMainPage();
            }).Start();
        }

        private void SaveUser()
        {
            //if (IsSaveSwitchToggled)
            //{
                Settings.UserName = UserName;
                Settings.UserPassword = Password;
                Settings.UserID = Id;
            //}
            //else
            //{
            //    Settings.UserName = null;
            //    Settings.UserPassword = null;
            //}
        }

        private void OpenMainPage()
        {
            App.Current.MainPage = new MainPage(UserName, Id);
        }
        
        private bool UserExist(string res)
        {

            // TODO: check user exists in database
            // Divide res string into parts "response:" and "status"

            string response_template = "response: ";
            string id_template = "id: ";

            int comma_index = res.IndexOf(',');

            string response_status = res.Substring(res.IndexOf(response_template) + response_template.Length, comma_index - response_template.Length);

            string id = res.Substring(res.IndexOf(id_template) + id_template.Length);

            Id = id;

            if (response_status.Equals("ok"))
            {
                return true;
            }
            else // if (response_status.Equals("failed"))
            {
                return false;
            }

        }
    }
}
