using GoBuhat.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoBuhat.ViewModel
{
    class AddEventViewModel
    {

        byte[] result;

        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public string EventDateTime { get; set; }
        public string EventPublishDateTime { get; set; }
        public string EventName { get; set; }
        public string EventText { get; set; }
        public string EventAuthorId { get; set; }
        public ICommand SubmitCommand { protected set; get; }

        public AddEventViewModel(string event_author_id)
        {
            SubmitCommand = new Command(OnSubmit);

            EventAuthorId = event_author_id;
        }
        public void OnSubmit()
        {

            if (!CheckInput())
                return;

            SetDates();

            SendNewEventToPHP();

        }

        private void SetDates()
        {
            int year = EventDate.Year;
            int month = EventDate.Month;
            int day = EventDate.Day;
            string date = year.ToString() + "-" + month.ToString() + "-" + day.ToString() + " " + EventTime;
            EventDateTime = date;

            EventPublishDateTime = DateTime.Now.ToString("yyyy-MM-dd hh:ss:mm");
            Console.WriteLine("Date ------> " + EventPublishDateTime);
        }

        private bool CheckInput()
        {

            //TODO: add rules which will check EventName and EventText text fields


            return true;
        }


        private void SendNewEventToPHP()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("https://adwatcherapplication.000webhostapp.com/add_event.php");

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("event_publish_datetime", EventPublishDateTime);
            parameters.Add("event_datetime", EventDateTime);
            parameters.Add("event_name", EventName);
            parameters.Add("event_author_id", EventAuthorId);
            parameters.Add("event", EventText);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                result = client.UploadValues(uri, parameters);

                Console.WriteLine(CommonUIHelper.ConvertResult(result));

            }).Start();
        }
    }
}
