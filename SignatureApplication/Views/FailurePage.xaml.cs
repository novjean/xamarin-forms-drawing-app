using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SignatureApplication
{
    public partial class FailurePage : ContentPage
    {

        private string deviceMoreInfo;

        public FailurePage()
        {
            InitializeComponent();
        }

        public FailurePage(string deviceMoreInfo)
        {
            NavigationPage.SetHasBackButton(this, false);
            this.deviceMoreInfo = deviceMoreInfo;

            InitializeComponent();
            displayMessage.Text = "This device is not active on our network.Please contact us and provide the device id: "+ deviceMoreInfo;

        }

		void onCheckAgainClicked(object sender, System.EventArgs e)
		{
            //Navigation.PushModalAsync(new MainPage(true));
            Navigation.PopAsync();
		}

    }
}
