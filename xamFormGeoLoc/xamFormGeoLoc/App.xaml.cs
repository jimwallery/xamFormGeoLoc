using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamFormGeoLoc.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace xamFormGeoLoc
{
    public partial class App : Application
    {

        public static double ScreenHeight;
        public static double ScreenWidth;

        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new RouteStopPage())
            {
                BarBackgroundColor = Color.FromHex("#3498DB"),
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
