using System;
using PCLStorage;
using Plugin.DeviceInfo;
using SignatureApplication.Services;
using SignaturePad.Forms;
using Xamarin.Forms;

namespace SignatureApplication
{
    public partial class MainPage : ContentPage
    {
        //Main Constructor
        public MainPage(bool registered)
        {
            InitializeComponent();
            //Called with registered=false by default, 
            //need a better implementation here
            if(!registered){
               verifyDevice();
            }
        }

        //Constructor for Rendering purpose in VS
        public MainPage(){
            InitializeComponent();
        }

        //Retreive the Device ID, MD5, and call RegistrationIntentService
        public async void verifyDevice()
        {
            //Generating the custom Unique identifier
            string deviceId=CrossDeviceInfo.Current.Id;
			//Encrypting the Device ID
			string md5 = EasyEncryption.MD5.ComputeMD5Hash(deviceId);
			System.Diagnostics.Debug.WriteLine($"Device Id: {deviceId}");

            //OS Specific Implementation (Optional)
			//switch(Device.RuntimePlatform){
			//    case Device.iOS: 
			//        deviceId = CrossDeviceInfo.Current.Id;
			//        break;
			//    case Device.Android:
			//        deviceId = CrossDeviceInfo.Current.GenerateAppId(false, null, null);
			//        break;
			//}

			//Call service to check if device is already registered
			RegistrationIntentService _regServ = new RegistrationIntentService();
            //If device is not registered
            if (!(_regServ.checkDevice(deviceId))){
                //Navigate to the failure Page
                await Navigation.PushAsync(new FailurePage(md5));
            }

        }

        private async void OnSendClicked(object sender, EventArgs e){

            //Accessing file system and creating file
            var folder = FileSystem.Current.LocalStorage;
            //var file = await folder.CreateFileAsync($"signature.png", 
            //CreationCollisionOption.GenerateUniqueName);
            var file = await folder.CreateFileAsync("signature.png", CreationCollisionOption.ReplaceExisting);
            //Creating the image of signature
			var settings = new ImageConstructionSettings
			{
				BackgroundColor = Color.White,
				StrokeColor = Color.Black,
			};


            try{
				using (var stream = await signaturePad.GetImageStreamAsync(SignatureImageFormat.Png, 
                                                                           settings))
				using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
				{
                    if(!stream.Equals(null))
					    await stream.CopyToAsync(fileStream);
				}

                //Display Thank you for signing. (Optional)
				await DisplayAlert("Thank you", 
                                   "Thanks for signing, " +
                                   "Your signature has been received.",
                                   "OK");
				System.Diagnostics.Debug.WriteLine($"File Path: {file.Path}");
                //Clear the signature after the file is saved.
                signaturePad.Clear();

                //Push to S3

                //Delete local device signature


            } catch(NullReferenceException){
                //Log Error here
                System.Diagnostics.Debug.WriteLine("Null Reference Exception caught");
                await DisplayAlert("Error", "Please sign.", "OK");
            } catch(Exception err){
                //Log error
                await DisplayAlert("Oops!", $"Something went wrong. Please make " +
                                   "sure to sign.", "OK");
            }
        }
    }
}
