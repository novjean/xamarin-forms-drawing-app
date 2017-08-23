using System;
namespace SignatureApplication.Models
{
    public class ApiToken
    {
        public string deviceToken { get; set; }
        public string expires { get; set; }
        public string host { get; set; }

        public ApiToken()
        {
        }
    }
}
