using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace FacePlace.CashingSystem.Configuration
{
    public class RemoteConfig : IConfig
    {
        public const bool IgnoreLongTests = true;

        public string SingleHost
        {
            get { return ""; }
        }
        public readonly string[] MasterHosts = new[] { "" };
        public readonly string[] SlaveHosts = new[] { "" };

        public const int RedisPort = 63790;

        public string SingleHostConnectionString
        {
            get
            {
                return SingleHost;
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
