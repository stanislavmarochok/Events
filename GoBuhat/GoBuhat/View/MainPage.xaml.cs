using GoBuhat.Interfaces;
using GoBuhat.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoBuhat
{
    public partial class MainPage : MasterDetailPage
    {
        public string Username { get; set; }
        public string Id { get; set; }

        public List<MasterPageItem> menuList { get; set; }

        public MainPage(string username, string id)
        {
            InitializeComponent();

            this.Username = username;
            this.Id = id;

            SetMasterPage();

        }

        private void SetMasterPage()
        {
            menuList = new List<MasterPageItem>();

            // Adding menu items to menuList and you can define title ,page and icon
            menuList.Add(new MasterPageItem() { Title = "My Page", Icon = "mypage.png", TargetType = typeof(ProfileView) });
            menuList.Add(new MasterPageItem() { Title = "All Events", Icon = "allevents.png", TargetType = typeof(LentaView) });

            //AddCategories();

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage(new LentaView(Username, Id));

            username_master_lbl.Text = Username + "\n" + "id: " + Id;
        }


        // It is just a testing function, delete it in future
        private void AddCategories()
        {
            foreach (Category category in (Category[])Enum.GetValues(typeof(Category)))
            {
                menuList.Add(new MasterPageItem() { Title = category.ToString(), Icon = "allevents.png", TargetType = typeof(LentaView) });
            }
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            if (page.Name.Equals("LentaView"))
            {
                Detail = new NavigationPage(new LentaView(Username, Id));
            }else
            {
                if (page.Name.Equals("ProfileView"))
                    Detail = new NavigationPage(new ProfileView(Username, Id));
                else
                    Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            }

            IsPresented = false;
        }

        private void log_out_btn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}
