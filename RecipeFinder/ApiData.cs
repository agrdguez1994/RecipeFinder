using RecipeFinder.Models;
using System.Net;
using System.Text.Json;

namespace RecipeFinder
{
    public class ApiData
    {
        //private static WebProxy proxy = new WebProxy
        //{
        //    Address = new Uri($""),
        //    BypassProxyOnLocal = false,
        //    UseDefaultCredentials = false/*,
        //    Credentials = new NetworkCredential(
        //            userName: "",
        //            password: "")*/
        //};
        private static HttpClientHandler httpClientHandler = new HttpClientHandler();
        /*{
            Proxy = proxy,
        };*/

        private static List<string> apis = new List<string> {
            "http://www.themealdb.com/api/json/v1/1/search.php?s=", //Search recipe
            "http://www.themealdb.com/api/json/v1/1/lookup.php?i="  //Get recipe by id
        };   

        public async static Task<Recipes> GetData(string searchValue,int api)
        {
            Recipes personObject = new Recipes();
            personObject.Meals = new List<Meal>();

            string apiResponse = "";
            using (var httpClient = new HttpClient(handler: httpClientHandler,false))
            {

                string url = apis[api]+searchValue;
                using (var response = await httpClient.GetAsync(url))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    personObject = JsonSerializer.Deserialize<Recipes>(apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


                }
                httpClient.Dispose();
            }
            return personObject;

        }
    }
}
