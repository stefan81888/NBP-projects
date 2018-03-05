using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }   
        public string Password { get; set; }
        public string ProfilePictureURL { get; set; }

        public List<User> Friends { get; set; }
        public List<Place> PlacesVisited { get; set; }
       // public List<PlaceRating> PlacesRated { get; set; }  mislim da bi bolje bilo ovako umesto List<Place>

        public User()
        {
            Friends = new List<User>();
            PlacesVisited = new List<Place>();
        }
    }
}
