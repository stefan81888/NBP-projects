using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Model
{
    public class Reservation
    {
        public ObjectId Id { get; set; }
        public List<FellowTraveler> Company { get; set; }

        public MongoDBRef Arrangement { get; set; }
        public MongoDBRef Customer { get; set; }

        public Reservation()
        {
            this.Company = new List<FellowTraveler>();
        }
    }
}
