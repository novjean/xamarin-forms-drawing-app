using System;
using System.Collections.Generic;
using SignatureApplication.Services;
using Xamarin.Forms;

namespace SignatureApplication
{
    public partial class FailurePage : ContentPage
    {

        private string uuid;
        private RegistrationIntentService _regServ = new RegistrationIntentService();

        public FailurePage()
        {
            InitializeComponent();
        }

        public FailurePage(string uuid)
        {
            NavigationPage.SetHasBackButton(this, false);
            this.uuid = uuid;

            InitializeComponent();

            //Instead of UUID, do we have some ID which is smaller and easier for the support team
            displayMessage.Text = "This device is not active on our network." +
                "Please contact us and provide the device id: "+ uuid;
        }

		void onCheckAgainClicked(object sender, System.EventArgs e)
		{
            
            Navigation.PopAsync();
		}

    }
}
