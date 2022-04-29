using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CountryLists
{
    public partial class App : Application
    {
        public App()
        {
            //InitializeComponent();

            MainPage = new NavigationPage(new Page());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
