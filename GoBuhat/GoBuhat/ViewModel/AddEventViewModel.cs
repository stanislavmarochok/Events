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

            SendNewEventToPHP();

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
