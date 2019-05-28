using GoBuhat.Common;
using GoBuhat.Controls;
using GoBuhat.PopUp;
using GoBuhat.ViewModel;
using Rg.Plugins.Popup.Services;
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

namespace GoBuhat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lenta : ContentPage
    {
        List<EventView> eventList;
        private byte[] result;

        public string UserName { get; set; }
        public string UserId { get; set; }

        public Lenta()
        {
            InitializeComponent();
        }

        public Lenta(string name, string id)
        {
            InitializeComponent();

            UserName = name;
            UserId = id;

            ShowLentaEvents();
        }

        private void ShowLentaEvents()
        {
            eventList = new List<EventView>();

            SendGetEventsToPHP();

        }

        private async void SendGetEventsToPHP()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/get_events.php");

            NameValueCollection parameters = new NameValueCollection();

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
                    string name, text, id, author_name, authorId, join_status;

                    //Console.WriteLine("\n\nTEST:  ------>  " + content.Substring(0, newLine) + '\n');

                    string line = content.Substring(0, newLine);

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

                    eventList.Add(new EventView(author_name, authorId, id, name, text, join_status));

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

            private void Add_event_btn_Clicked(object sender, EventArgs e)
        {
            AddNewEvent();
        }

        private void AddNewEvent()
        {
            // TODO: add push up window with text input

            CreateNewEventDialog();
        }

        private void CreateNewEventDialog()
        {
            PopupNavigation.Instance.PushAsync(new AddEventPopUp(UserId));
        }
    }
}