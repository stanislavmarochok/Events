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
	public partial class UserView : ContentView
    {
        private string userID;
        public string UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        public UserView( string userId, string userName )
		{
            InitializeComponent();

            UserID = userId;
            UserName = userName;

            BindingContext = this;
        }

        private async void OnEventHeaderTapped(object sender, EventArgs e)
        {
            //await PopupNavigation.Instance.PopAsync();
            await Navigation.PushAsync(new ProfileView(UserName, UserID));
        }
    }
}
