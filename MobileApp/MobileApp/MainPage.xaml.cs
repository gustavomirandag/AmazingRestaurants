using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        const string userName = "yuri@al.infnet.edu.br";
        const string password = "@dsInf123";
        const string apiBaseUri = "http://localhost:55104";
        //const string apiBaseUri = "https://amazingrestaurants-restaurantsmicroservice-api.azurewebsites.net";
        const string apiGetValues = "/api/restaurants";


        private static async Task<string> GetAPIToken(string userName, string password, string apiBaseUri)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                });

                //send request
                var responseMessage = client.PostAsync("/Token", formContent).Result;

                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);
                return jObject.GetValue("access_token").ToString();
            }
        }

        static async Task<string> GetRequest(string token, string apiBaseUri, string requestPath)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //make request
                HttpResponseMessage response = client.GetAsync(requestPath).Result;
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //Get the token
            var token = GetAPIToken(userName, password, apiBaseUri).Result;
            Console.WriteLine("Token: {0}", token);

            //Make the call
            var response = GetRequest(token, apiBaseUri, apiGetValues).Result;
            var restaurants = JsonConvert.DeserializeObject<IEnumerable<string>>(response);

            foreach (var restaurant in restaurants)
            {
                var label = new Label
                {
                    Text = restaurant
                };

                slRestaurantsList.Children.Add(label);
            }
        }
    }
}
