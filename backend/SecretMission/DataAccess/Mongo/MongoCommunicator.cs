using SecretMission.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretMission.DB
{
    public class MongoCommunicator : IDBCommunicator
    {
        private static MongoCommunicator MongoInstance = null;
        private static MongoBasicActions MongoBase = null;
        
        
        private MongoCommunicator() { }

        public static MongoCommunicator Instance()
        {
            if (MongoInstance == null)
            {
                MongoInstance = new MongoCommunicator();
                MongoBase = MongoBasicActions.Instance();
            }
            return MongoInstance;
        }

        public async Task<List<AgentMission>> GetAgentMissionListAsync()
        {
            return await MongoBase.GetAllObjectsAsync<AgentMission>();
        }

        public async Task AddAgentMissionAsync<AgentMission>(AgentMission newRecord)
        {
            await MongoBase.AddNewObjectAsync(newRecord);
        }
    }
}
