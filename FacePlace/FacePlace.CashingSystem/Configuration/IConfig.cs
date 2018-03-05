using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.CashingSystem.Configuration
{
    public interface IConfig
    {
        RedisClient GetRedisClient();
    }
}
