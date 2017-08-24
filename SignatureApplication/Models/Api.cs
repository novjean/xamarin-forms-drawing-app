using System;
namespace SignatureApplication.Models
{
    public class Api
    {
        public string api { get; }
        public Api()
        {
            this.api = "https://webcoreapi-central-test.azurewebsites.net/api/";
        }
    }
}
