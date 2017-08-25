using System;
using System.Collections.Generic;
using PCLStorage;
using SignaturePad.Forms;
using Xamarin.Forms;

namespace SignatureApplication.Views
{
    public partial class SigningPage : ContentPage
    {
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        public SigningPage()
        {
            InitializeComponent();
        }

		//Method for when the signature is completed
		private async void OnSendClicked(object sender, EventArgs e)
		{

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


			try
			{
				using (var stream = await signaturePad.GetImageStreamAsync(SignatureImageFormat.Png,
																		   settings))
				using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
				{
					//Trying to stop the file from being created as blank (Needs work)
					if (!stream.Equals(null))
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


				//Delete local device file to save storage on device
				//Is this optional ?

			}
			catch (NullReferenceException)
			{
				//Log Error here
				System.Diagnostics.Debug.WriteLine("Null Reference Exception caught");
				await DisplayAlert("Error", "Please sign.", "OK");
			}
			catch (Exception)
			{
				//Log error
				await DisplayAlert("Oops!", $"Something went wrong. Please make " +
								   "sure to sign.", "OK");
			}
		}
    }
}
