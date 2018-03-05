using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.CashingSystem.RedisLibrary;
using FacePlace.CashingSystem.CashingService;

namespace FacePlace.CashingSystem.Utilities
{
    public static class ObjectFactory
    {
        public static CashingService.CashingService GetCashingService()
        {
            return new CashingService.CashingService();
        }
    }
}
