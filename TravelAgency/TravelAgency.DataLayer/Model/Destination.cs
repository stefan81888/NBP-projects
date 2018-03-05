using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Model
{
    public class Destination
    {
        public ObjectId Id { get; set; }
        public String Country { get; set; }
        public String PlaceName { get; set; }
        public String Description { get; set; }
        //public String Pictures { get; set; }
    }
}
