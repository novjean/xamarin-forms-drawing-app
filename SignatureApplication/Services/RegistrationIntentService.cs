/*
 *All System.Diagnostics needs to be removed. 
 *Placed here for debugging purpose
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignatureApplication.Models;

namespace SignatureApplication.Services
{
    public class RegistrationIntentService
    {
        private HttpClient _client = new HttpClient();

        //Implement if the device is already registered
        public async Task<DeviceInfo> checkDevice(string UUID){

            //Might have to look into a better implementation
            string Url = getUrl() + UUID;
            System.Diagnostics.Debug.WriteLine($"Url Called = {Url}");

            //async call to see if the url gets a response
            var response = await _client.GetAsync(Url);
            System.Diagnostics.Debug.WriteLine($"Response status code = {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"Response object = {response}");
            if(response.IsSuccessStatusCode){
                var content = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Content=  {content}");
                
                try{
                    return JsonConvert.DeserializeObject<DeviceInfo>(content);
                } catch(Exception e){
                    //Log the exception from this service
                    System.Diagnostics.Debug.WriteLine($"Exception in Registration Service: {e}");
                    return null;
                }
            }
            System.Diagnostics.Debug.WriteLine("Error: Did not receive a status code." +
                                               "Returning null");
            return null;
        }

        private string getUrl()
        {
            return "https://webcoreapi-central-test.azurewebsites.net/api/Setup/Key?deviceIdentifier=";
        }
    }
}
