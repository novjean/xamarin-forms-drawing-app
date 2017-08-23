using System;
using SignatureApplication.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDevice))]
namespace SignatureApplication.Droid
{
	public class AndroidDevice : IDevice
	{
		public string GetIdentifier()
		{
			return Settings.Secure.GetString(Forms.Context.ContentResolver, Settings.Secure.AndroidId);
		}
	}
}
