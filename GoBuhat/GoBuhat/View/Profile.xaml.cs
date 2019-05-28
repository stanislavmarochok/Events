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
	public partial class Profile : ContentPage
    {
        List<EventView> eventList;
        private byte[] result;

        public string UserName { get; set; }
        public string UserId { get; set; }
        
        public Profile(string name, string id)
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
                string name, text, id, authorId, join_status;

                //Console.WriteLine("\n\nTEST:  ------>  " + content.Substring(0, newLine) + '\n');

                string line = content.Substring(0, newLine);

                //join_status = line.Substring(0, line.IndexOf(", Event Author ID:"));
                //line = line.Remove(0, line.IndexOf(", Event Author ID:") + 2);

                authorId = line.Substring(0, line.IndexOf(", Event ID:"));
                line = line.Remove(0, line.IndexOf(", Event ID:") + 2);

                id = line.Substring(0, line.IndexOf(", Event name:"));
                line = line.Remove(0, line.IndexOf(", Event name:") + 2);

                name = line.Substring(0, line.IndexOf(", Event text:"));
                line = line.Remove(0, line.IndexOf(", Event text:") + 2);

                text = line;

                authorId = authorId.Remove(0, 17);
                id = id.Remove(0, 10);
                name = name.Remove(0, 12);
                text = text.Remove(0, 12);

                eventList.Add(new EventView("STEVE", authorId, id, name, text, "true"));

                content = content.Remove(0, newLine + 1);
            }

            eventList.Reverse();

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