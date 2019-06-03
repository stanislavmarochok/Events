using GoBuhat.Common;
using GoBuhat.Controls;
using GoBuhat.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBuhat.PopUp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShowAllUsersOfEvent : PopupPage
    {
        byte[] result;
        public string EventId { get; set; }

        List<UserView> users;
        public ShowAllUsersOfEvent(string event_id)
		{
			InitializeComponent ();

            EventId = event_id;

            AddUsers();

            //this.BindingContext = new ShowAllUsersViewModel(event_id);
        }

        private void AddUsers()
        {
            // TODO: get users who liked this post
            users = new List<UserView>();

            SendGetUsersToPHP();
        }

        private void SendGetUsersToPHP()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/get_users_for_event.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("event_id", EventId);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                result = client.UploadValues(uri, parameters);

                string content = CommonUIHelper.ConvertResult(result);

                while (!string.IsNullOrEmpty(content))
                {
                    int newLine = content.IndexOf('\n');
                    string user_name, user_id;

                    string line = content.Substring(0, newLine);

                    user_name = line.Substring(0, line.IndexOf(", User Id:"));
                    line = line.Remove(0, line.IndexOf(", User Id:") + 2);

                    user_id = line;

                    user_name = user_name.Remove(0, 11);
                    user_id = user_id.Remove(0, 9);

                    users.Add(new UserView(user_id, user_name));

                    content = content.Remove(0, newLine + 1);
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (users.Count == 0)
                    {
                        UsersList.Children.Add(new Label()
                        {
                            Text = "Noone likes this event now :("
                        });

                        return;
                    }

                    foreach (var user in users)
                    {
                        UsersList.Children.Add(user);
                    }
                });

            }).Start();
        }
    }
}