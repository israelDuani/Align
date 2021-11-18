using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretMission.Helpers
{
    public static class Constants
    {
        public const string DB_NAME = "SecretMission";
        public const string COLLECTION_NAME = "AgentMissions";
        public const string LOCAL_DB_CONNECTION_STRING = "mongodb://localhost:27017"; 
        public const string DOCKER_DB_CONNECTION_STRING = "mongodb://db-mongo:27017";
    }
}
