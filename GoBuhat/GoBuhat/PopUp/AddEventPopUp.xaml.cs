using GoBuhat.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBuhat.PopUp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddEventPopUp : PopupPage
	{
		public AddEventPopUp (string event_author_id)
		{
			InitializeComponent ();

            this.BindingContext = new AddEventViewModel(event_author_id);
		}

        private void btn_addEvent_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}