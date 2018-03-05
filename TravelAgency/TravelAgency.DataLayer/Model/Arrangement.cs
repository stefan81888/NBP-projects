using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Model
{
    public class Arrangement
    {
        public ObjectId Id { get; set; }
        public float Price { get; set; }
        public int MaxNumberOfPassengers { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public String Type { get; set; }
        public String Description { get; set; }

        public MongoDBRef Hotel { get; set; }
        public MongoDBRef Destination { get; set; }
    }

    public static class ArrangementType
    {
        public static String HalfBoard
        {
            get
            {
                return "Half Board";
            }
        }
        public static String FullBoard
        {
            get
            {
                return "Full Board";
            }
        }
        public static String AllIncluded
        {
            get
            {
                return "All Included";
            }
        }
    }
}
