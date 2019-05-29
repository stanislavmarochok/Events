using GoBuhat.Common;
using GoBuhat.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBuhat.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileView : ContentPage
    {
        List<EventView> eventList;
        private byte[] result;

        public string UserName { get; set; }
        public string UserId { get; set; }
        
        public ProfileView(string name, string id)
        {
            InitializeComponent();

            UserName = name;
            UserId = id;

            ShowLentaEvents();

            BindingContext = this;
        }

        private void ShowLentaEvents()
        {
            string name;
            string text;

            eventList = new List<EventView>();

            SendGetEventsToPHP();

        }

        private async void SendGetEventsToPHP()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/get_events_for_user.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("event_author_id", UserId);
            parameters.Add("user_id", UserId);

            string content = "";

            new Thread(() =>
            {
            Thread.CurrentThread.IsBackground = true;

            //client.UploadValuesCompleted += this.client_UploadDataCompleted;
            result = client.UploadValues(uri, parameters);

            content = CommonUIHelper.ConvertResult(result);

            while (!string.IsNullOrEmpty(content))
            {
                int newLine = content.IndexOf('\n');
                string name, text, id, author_name, authorId, join_status, datetime, publish_datetime;

                string line = content.Substring(0, newLine);

                publish_datetime = line.Substring(0, line.IndexOf(", Event datetime:"));
                line = line.Remove(0, line.IndexOf(", Event datetime:") + 2);

                datetime = line.Substring(0, line.IndexOf(", Join Status:"));
                line = line.Remove(0, line.IndexOf(", Join Status:") + 2);

                join_status = line.Substring(0, line.IndexOf(", Event Author Username:"));
                line = line.Remove(0, line.IndexOf(", Event Author Username:") + 2);

                author_name = line.Substring(0, line.IndexOf(", Event Author ID:"));
                line = line.Remove(0, line.IndexOf(", Event Author ID:") + 2);

                authorId = line.Substring(0, line.IndexOf(", Event ID:"));
                line = line.Remove(0, line.IndexOf(", Event ID:") + 2);

                id = line.Substring(0, line.IndexOf(", Event name:"));
                line = line.Remove(0, line.IndexOf(", Event name:") + 2);

                name = line.Substring(0, line.IndexOf(", Event text:"));
                line = line.Remove(0, line.IndexOf(", Event text:") + 2);

                text = line;

                author_name = author_name.Remove(0, 23);
                authorId = authorId.Remove(0, 17);
                id = id.Remove(0, 10);
                name = name.Remove(0, 12);
                text = text.Remove(0, 12);
                join_status = join_status.Remove(0, 13);
                publish_datetime = publish_datetime.Remove(0, 24);
                datetime = datetime.Remove(0, 16);

                eventList.Add(new EventView(author_name, authorId, id, name, text, join_status, datetime, publish_datetime));

                content = content.Remove(0, newLine + 1);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var item in eventList)
                {
                    eventsLayout.Children.Add(item);
                }
            });

            }).Start();

        }
    }
}