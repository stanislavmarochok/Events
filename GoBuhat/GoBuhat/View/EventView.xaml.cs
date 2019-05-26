using GoBuhat.Common;
using GoBuhat.Utils;
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
        private string eventAuthorId;
        public string EventAuthorId
        {
            get
            {
                return "Author ID: " + eventAuthorId;
            }
            set
            {
                eventAuthorId = value;
            }
        }

        //public EventView(string author_id, string id, string name, string text)
        //{
        //    InitializeComponent();

        //    EventAuthorId = author_id;
        //    EventID = id;
        //    EventName = name;
        //    EventText = text;

        //    ChangeJoinStatus();

        //    BindingContext = this;
        //}

        public EventView (string author_id, string id, string name, string text, string join_status)
		{
			InitializeComponent ();

            EventAuthorId = author_id;
            EventID = id;
            EventName = name;
            EventText = text;

            JoinStatus = (join_status.Equals("true")) ? true : false;

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

        }
    }
}
