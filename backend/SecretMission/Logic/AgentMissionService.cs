using SecretMission.DB;
using SecretMission.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMission.Logic
{
    public static class AgentMissionService
    {
        public static async Task<string> GetMostIsolatedCountry(IDBCommunicator communicator)
        {
            var missionsList = await communicator.GetAgentMissionListAsync();
            var withoutFutureMissions = await IsolatedCountryLogic.RemoveFutureMission(missionsList);
            var noDuplicatesList = await IsolatedCountryLogic.RemoveDuplicateAgents(withoutFutureMissions);
            string result = await IsolatedCountryLogic.FindMostIsolatedCountry(noDuplicatesList);
            return result;
        }
        

        public static async Task<AgentMission> GetClosestMission(IDBCommunicator communicator,string LocationAddress)
        {
            return await MapProcessingLogic.GetClosestMission(communicator, LocationAddress);
        }

        
    }
}
