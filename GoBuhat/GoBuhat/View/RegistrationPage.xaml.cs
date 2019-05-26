using GoBuhat.Common;
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
	public partial class RegistrationPage : ContentPage
	{
		public RegistrationPage ()
		{
			InitializeComponent ();

            var vm = new RegistrationViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            vm.OpenMainPage += () => { OpenMainPage(); };

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };
        }
        
        private static void OpenMainPage()
        {
            App.Current.MainPage = new MainPage("nothing", "0");
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pass_strong = "";

            switch (PasswordAdvisor.CheckStrength(Password.Text))
            {
                case PasswordScore.Blank:
                    pass_strong = "Blank";
                    break;

                case PasswordScore.VeryWeak:
                    pass_strong = "Very Weak";
                    break;

                case PasswordScore.Weak:
                    pass_strong = "Weak";
                    break;

                case PasswordScore.Medium:
                    pass_strong = "Medium";
                    break;

                case PasswordScore.Strong:
                    pass_strong = "Strong";
                    break;

                case PasswordScore.VeryStrong:
                    pass_strong = "Very Strong";
                    break;
            }

            pass_st_lbl.Text = "Password strong: " + pass_strong;
        }
    }
}