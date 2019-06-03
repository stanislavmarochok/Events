using GoBuhat.Common;
using GoBuhat.Pages;
using GoBuhat.PopUp;
using GoBuhat.Utils;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBuhat.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventView : ContentView
    {
        private bool joinStatus;
        public bool JoinStatus
        {
            get
            {
                btn_join.Source = (joinStatus) ? "liked.png" : "not_liked.png";

                return joinStatus;
            }
            set
            {
                joinStatus = value;

                btn_join.Source = (joinStatus) ? "liked.png" : "not_liked.png";
            }
        }
        private string eventID;
        public string EventID
        {
            get
            {
                return eventID;
            }
            set
            {
                eventID = value;
            }
        }
        private string eventName;
        public string EventName
        {
            get
            {
                //return "Name: " + eventName;
                return eventName;
            }
            set
            {
                eventName = value;
            }
        }
        private string eventText;
        public string EventText
        {
            get
            {
                //return "Text: " + eventText;
                return eventText;
            }
            set
            {
                eventText = value;
            }
        }
        private string eventAuthorName;
        public string EventAuthorName
        {
            get
            {
                return eventAuthorName;
            }
            set
            {
                eventAuthorName = value;
            }
        }
        private string eventAuthorId;
        public string EventAuthorId
        {
            get
            {
                return eventAuthorId;
            }
            set
            {
                eventAuthorId = value;
            }
        }
        private string eventDatetime;
        public string EventDatetime
        {
            get
            {
                return eventDatetime;
            }
            set
            {
                eventDatetime = value;
            }
        }
        private string eventPublishDatetime;
        public string EventPublishDatetime
        {
            get
            {
                return eventPublishDatetime;
            }
            set
            {
                eventPublishDatetime = value;
            }
        }

        byte[] result;
        List<UserView> users;

        Label noUsers;

        public EventView (
            string author_name, 
            string author_id, 
            string id, 
            string name, 
            string text, 
            string join_status,
            string datetime,
            string publish_datetime )
		{
			InitializeComponent ();

            EventAuthorName = author_name;
            EventAuthorId = author_id;
            EventID = id;
            EventName = name;
            EventText = text;
            EventDatetime = datetime;
            EventPublishDatetime = publish_datetime;

            JoinStatus = (join_status.Equals("true")) ? true : false;

            users = new List<UserView>();

            noUsers = new Label()
            {
                Text = "Noone likes this event now :("
            };

            BindingContext = this;
        }

        private void Btn_join_Clicked(object sender, EventArgs e)
        {
            JoinStatus = (JoinStatus) ? false : true;

            if (JoinStatus)
            {
                JoinEvent();
            }
            else
            {
                UnjoinEvent();
            }
        }

        private void UnjoinEvent()
        {
            byte[] result;

            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/unjoin_event.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("user_id", Settings.UserID);
            parameters.Add("event_id", EventID);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                result = client.UploadValues(uri, parameters);

                string res = CommonUIHelper.ConvertResult(result);

            }).Start();
        }

        private void JoinEvent()
        {
            byte[] result;

            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/join_event.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("user_id", Settings.UserID);
            parameters.Add("event_id", EventID);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                
                result = client.UploadValues(uri, parameters);

                string res = CommonUIHelper.ConvertResult(result);

            }).Start();
        }

        private void Btn_showAll_Clicked(object sender, EventArgs e)
        {
            ShowAllUsersWhoLiked();
        }

        private void ShowAllUsersWhoLiked()
        {

            if (UsersList.Children.Count != 0)
            {
                foreach (var item in UsersList.Children.ToList())
                    UsersList.Children.Remove(item);

                foreach (var item in users.ToList())
                    users.Remove(item);

                SetColor(false);

                return;
            }

            SetColor(true);
            AddUsers();
        }

        private void SetColor(bool active)
        {
            if (active)
            {
                btn_showAll.BackgroundColor = Color.Aqua;
                UsersList.BackgroundColor = Color.Aqua;
            }else
            {
                btn_showAll.BackgroundColor = Color.White;
                UsersList.BackgroundColor = Color.White;
            }
        }

        private void AddUsers()
        {
            // TODO: get users who liked this post

            SendGetUsersToPHP();
        }

        private void SendGetUsersToPHP()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/get_users_for_event.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("event_id", EventID);

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
                        UsersList.Children.Add(noUsers);

                        return;
                    }

                    foreach (var user in users)
                    {
                        UsersList.Children.Add(user);
                    }
                });

            }).Start();
        }

        private async void OnEventHeaderTapped(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new ProfileView(EventAuthorName, EventAuthorId));

            await Navigation.PushAsync(new ProfileView(EventAuthorName, EventAuthorId));
        }
    }
}
