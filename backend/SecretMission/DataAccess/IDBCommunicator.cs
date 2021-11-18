using SecretMission.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretMission.DB
{
    public interface IDBCommunicator
    {
        public Task<List<AgentMission>> GetAgentMissionListAsync();
        public Task AddAgentMissionAsync<AgentMission>(AgentMission newRecord);
    }
}
