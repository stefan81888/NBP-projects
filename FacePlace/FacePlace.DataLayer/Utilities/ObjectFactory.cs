using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.DataLayer.DataLayerService;

namespace FacePlace.DataLayer.Utilities
{
    public static class ObjectFactory
    {
        public static DataLayerService.DataLayerService GetDataLayerService()
        {
            return new DataLayerService.DataLayerService();
        }
        
    }
}
