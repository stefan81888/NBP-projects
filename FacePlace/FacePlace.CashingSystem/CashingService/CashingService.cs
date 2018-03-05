using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.CashingSystem.RedisLibrary;
using FacePlace.CashingSystem.Configuration;

namespace FacePlace.CashingSystem.CashingService
{
    public class CashingService
    {
        private PlaceCash placeCash;
        private UserCash userCash;

        private IConfig config;

        public CashingService()
        {
            config = new LocalConfig();

            placeCash = new PlaceCash(config);
            userCash = new UserCash(config);
        }

        public PlaceCash PlaceCash
        {
            get
            {
                return placeCash;
            }
        }

        public UserCash UserCash
        {
            get
            {
                return userCash;
            }
        }        
    }
}
