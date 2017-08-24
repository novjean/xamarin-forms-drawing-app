using System;
using PCLStorage;
using Plugin.DeviceInfo;
using SignatureApplication.Models;
using SignatureApplication.Services;
using SignaturePad.Forms;
using Xamarin.Forms;

//Might need to change the name 
namespace SignatureApplication
{
    public partial class MainPage : ContentPage
    {
        //Instance of the registration service
        private RegistrationIntentService _regServ = new RegistrationIntentService();
        
        //Main Constructor
        public MainPage(bool registered)
        {
            InitializeComponent();
            //Called with registered=false by default, 
            //Better implementation required
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
            string uuid = EasyEncryption.MD5.ComputeMD5Hash(deviceId);
            //Temporarily Hardcoded a working uuid
            //string uuid = "aa9ea3b5f371b0a4adfb5c68f24b95fa";
            //System.Diagnostics.Debug.WriteLine($"UUID: {uuid}");

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
            DeviceInfo device = null;
            device = await _regServ.checkDevice(uuid);

            //If device is not registered
            //if (!(_regServ.checkDevice(deviceId))){
            try{
				if (device.Equals(null))
				{
					//Navigate to the failure Page
					await Navigation.PushAsync(new FailurePage(uuid));
                } else{
					System.Diagnostics.Debug.WriteLine($"Device Key = {device.key}");
					System.Diagnostics.Debug.WriteLine("Registered device. Congrats");
                }

            } catch(NullReferenceException){
                System.Diagnostics.Debug.WriteLine("Null Exception caught");
				//Navigate to the failure Page
				await Navigation.PushAsync(new FailurePage(uuid));
            }

        }

        //Method for when the signature is completed
        private async void OnSendClicked(object sender, EventArgs e){

            //Accessing file system and creating file
            var folder = FileSystem.Current.LocalStorage;

            //Creating file without limitng the number of files locally
            //var file = await folder.CreateFileAsync($"signature.png", 
            //CreationCollisionOption.GenerateUniqueName);
            //Better implementation. Only one sign file locally being replaced
            //Whats the file name prefix suffix?
            var file = await folder.CreateFileAsync("signature.png", 
                                                    CreationCollisionOption.ReplaceExisting);
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
                    //Trying to stop the file from being created as blank (Needs work)
                    if(!stream.Equals(null))
					    await stream.CopyToAsync(fileStream);
				}

                //Display Thank you for signing. (Optional)
				await DisplayAlert("Thank you", 
                                   "Thanks for signing, " +
                                   "Your signature has been received.",
                                   "OK");
				System.Diagnostics.Debug.WriteLine($"File Path: {file.Path}");
                //Clear the signature from pad after the file is saved.
                signaturePad.Clear();

                //Push to S3
                //AWS VS Toolkit. Needs research.
                //AWS Credentials - Account Number, Secret Key and one more
                //Bucket(s) name(s)
                //File Name to be stored in
                //Where in DB is it updated ?


                //Delete local device signature
                //Is this optional ?

            } catch(NullReferenceException){
                //Log Error here
                System.Diagnostics.Debug.WriteLine("Null Reference Exception caught");
                await DisplayAlert("Error", "Please sign.", "OK");
            } catch(Exception){
                //Log error
                await DisplayAlert("Oops!", $"Something went wrong. Please make " +
                                   "sure to sign.", "OK");
            }
        }
    }
}
