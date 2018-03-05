using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.CashingSystem.Configuration
{
    public class LocalConfig : IConfig
    {
        public const bool IgnoreLongTests = true;

        public string SingleHost
        {
            get { return "localhost"; }
        }
        public readonly string[] MasterHosts = new[] { "localhost" };
        public readonly string[] SlaveHosts = new[] { "localhost" };

        public const int RedisPort = 6379;

        public string SingleHostConnectionString
        {
            get
            {
                return SingleHost + ":" + RedisPort;
            }
        }

        public BasicRedisClientManager BasicClientManger
        {
            get
            {
                return new BasicRedisClientManager(new[] {
                    SingleHostConnectionString
                });
            }
        }

        public RedisClient GetRedisClient()
        {
            return new RedisClient(SingleHost);
        }
    }
}
