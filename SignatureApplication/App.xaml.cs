using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.FirebasePushNotification;
using SignatureApplication.Views;
using Xamarin.Forms;

namespace SignatureApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(false));

			//FCM Integration
			CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
			{
				System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
				//DisplayAlert("Token", $"Token: {p.Token}", "OK");

			};

           

			//CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Received Notification in App");

            //    MainPage = new SigningPage();
            //    //DisplayAlert("Received", "Token Received", "OK");

            //};

			
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
