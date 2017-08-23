using System;
namespace SignatureApplication.Models
{
    public class Device
    {
        public string deviceIdentifier { get; set; }

        public Device(string deviceIdentifier)
        {
            this.deviceIdentifier = deviceIdentifier;
        }
    }
}
