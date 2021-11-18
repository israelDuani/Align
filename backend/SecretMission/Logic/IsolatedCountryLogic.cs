using SecretMission.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMission.Logic
{
    public static class IsolatedCountryLogic
    {
        // Scan all the missions and remove the missions the didnt occurred yet
        public static async Task<List<AgentMission>> RemoveFutureMission(List<AgentMission> missionsList)
        {
            List<AgentMission> result = await Task.Run(() =>
            {
                DateTime parsedDate;
                List<AgentMission> result = new List<AgentMission>(missionsList);

                foreach (var mission in missionsList)
                {
                    parsedDate = DateTime.Parse(mission.date);
                    if (DateTime.Compare(parsedDate, DateTime.Now) >= 0)
                    {
                        result.Remove(mission);
                    }
                }
                return result;
            });

            return result;
        }

        // scan all the mission and remove any mission that its agent is appear on other mission
        public static async Task<List<AgentMission>> RemoveDuplicateAgents(List<AgentMission> missionsList)
        {
            List<AgentMission> result = await Task.Run(() =>
            {
                // find all duplicates agents
                var duplicateItems = missionsList.GroupBy(mission => mission.agent).SelectMany(group => group.Skip(1));

                // remove all duplicates agents
                foreach (var item in duplicateItems)
                {
                    missionsList.RemoveAll(mission => mission.agent == item.agent);
                }
                return missionsList;
            });

            return result;
        }

        // Get the country with most mission (after the filter of duplicate and time filter)
        public static async Task<string> FindMostIsolatedCountry(List<AgentMission> missionsListNoDuplicate)
        {
            string MostIsolatedCountry = "none";
            int NumOfMostMissions = 0;
            string currentCountry = "none";
            int currentCounter = 0;
            string result = await Task.Run(() =>
            {
                List<AgentMission> sortedMissions = missionsListNoDuplicate.OrderBy(mission => mission.country).ToList();
                foreach (AgentMission mission in sortedMissions)
                {
                    // country is different from the previos one.
                    if (mission.country != currentCountry)
                    {
                        // check if the last country is the max one
                        if (currentCounter > NumOfMostMissions)
                        {
                            MostIsolatedCountry = currentCountry;
                            NumOfMostMissions = currentCounter;
                        }
                        // reset counter for the new country to be scanned
                        currentCountry = mission.country;
                        currentCounter = 0;
                    }
                    currentCounter++;
                }
                return MostIsolatedCountry;
            });
            return result;
        }
    }
}
