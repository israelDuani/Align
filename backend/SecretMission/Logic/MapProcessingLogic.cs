using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecretMission.DB;
using SecretMission.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SecretMission.Logic
{
    public static class MapProcessingLogic
    {
        private static HttpClient client = new HttpClient();
        private static string ApiKey = "AIzaSyBsFOHyeNh0kJNI01Q9K9vzCoMkTB3YxF8";

        public static async Task<AgentMission> GetClosestMission(IDBCommunicator dbCommunicator,string addressToComapre)
        {
            // Get all missions from db
            var missionsList = await dbCommunicator.GetAgentMissionListAsync();
            // Extract the addresses from the missions objects
            string addressesString = await GetAddressStringFromMissions(missionsList);
            // Create the Url Request to google distance matrix
            Uri apiRequest = await BuildApiRequest(addressToComapre, addressesString);
            // Call the api and extract the relevant data
            List<JToken> apiResult = await GetApiResult(apiRequest);
            // Comapre all the distances and find the index of the minimum distance 
            int closestMissionIndex = await getMinDistanceIndex(apiResult);
            // No results
            if(closestMissionIndex == -1)
            {
                throw new KeyNotFoundException("could not find any locations using distance matrix api");
            }
        
            return missionsList[closestMissionIndex];
        }

        public static async Task<string> GetAddressStringFromMissions(List<AgentMission> missionsList)
        {
            return await Task.Run(() =>
            {
                StringBuilder destinations = new StringBuilder(); ;
                foreach (AgentMission mission in missionsList)
                {
                    destinations.Append(mission.address);
                    destinations.Append(" ,");
                    destinations.Append(mission.country);
                    destinations.Append("|");
                }
                return destinations.ToString();
            });
        }

        public static async Task<Uri> BuildApiRequest(string locationAddress, string addressesString)
        {
            return await Task.Run(() =>
            {
                var uriBuilder = new UriBuilder("https://maps.googleapis.com/maps/api/distancematrix/json");
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                parameters["origins"] = locationAddress;
                parameters["destinations"] = addressesString;
                parameters["key"] = ApiKey;
                uriBuilder.Query = parameters.ToString();
                Uri finalUrl = uriBuilder.Uri;
                return finalUrl;
            });
        }

        public static async Task<List<JToken>> GetApiResult(Uri apiRequest)
        {
            HttpResponseMessage response = await client.GetAsync(apiRequest);
            if (response.IsSuccessStatusCode)
            {
                string product = await response.Content.ReadAsStringAsync();
                var x = JsonConvert.DeserializeObject<JObject>(product);
                var distanceArr = x["rows"][0]["elements"].ToList();
                return distanceArr;
            }
            return new List<JToken>();
        }

        public static async Task<int> getMinDistanceIndex(List<JToken> distanceArr)
        {
            int minDistance = int.MaxValue;
            int minDistanceIndex = -1;
            int currIndex = 0;

            await Task.Run(() =>
            {
                // run on every result from google and find the minimum distance
                foreach (JToken item in distanceArr)
                {
                    string status = item["status"].ToString();
                    // skip if google is could not calculate the distance to the current address
                    if (status != "ZERO_RESULTS")
                    {
                        int currDistance = (int)item["distance"]["value"];
                        if (currDistance < minDistance)
                        {
                            minDistance = currDistance;
                            minDistanceIndex = currIndex;
                        }
                    }
                    currIndex++;
                }
            });
            return minDistanceIndex;
        }
    }
}
