using GoBuhat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoBuhat.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");

            UserName.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };
        }

        private void sw_save_Toggled(object sender, ToggledEventArgs e)
        {

            SaveThisUser();

        }

        private void SaveThisUser()
        {

            // TODO: save this user on device as text or something else
        }

        private void Reg_btn_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegistrationPage();
        }
    }
}