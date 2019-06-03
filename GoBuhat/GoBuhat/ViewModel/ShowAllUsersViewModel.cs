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
    class ShowAllUsersViewModel
    {

        byte[] result;

        public string EventId { get; set; }
        public ICommand SubmitCommand { protected set; get; }

        public ShowAllUsersViewModel(string event_id)
        {
            SubmitCommand = new Command(OnSubmit);

            EventId = event_id;
        }
        public void OnSubmit()
        {

            if (!CheckInput())
                return;

            //SendNewEventToPHP();

        }

        private bool CheckInput()
        {

            //TODO: add rules which will check EventName and EventText text fields


            return true;
        }
    }
}
