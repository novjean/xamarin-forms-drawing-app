using System;
namespace SignatureApplication.Services
{
    public class RegistrationIntentService
    {
        public string deviceId { get; set; }

        public RegistrationIntentService()
        {
        }

        //Implement if the device is already registered
        public bool checkDevice(string GUID){
            
            return false;
        }
    }
}
