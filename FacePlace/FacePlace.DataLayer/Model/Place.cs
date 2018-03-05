using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Model
{
    public class Place
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public float AverageRating { get; set; }
        public int NumberOfVotes { get; set; }

        public List<Picture> Pictures { get; set; }
        public List<User> Visitors { get; set; }

        public Place()
        {
            Pictures = new List<Picture>();
            Visitors = new List<User>();
        }
    }
}
