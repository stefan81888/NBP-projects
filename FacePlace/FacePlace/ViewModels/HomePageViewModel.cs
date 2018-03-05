using FacePlace.DataLayer.Model;
using FacePlace.DataProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacePlace.ViewModels
{
    public class HomePageViewModel
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
        public List<Place> RecomendedPlaces { get; set; }
        public List<User> RecomendedFriends { get; set; }

        public HomePageViewModel()
        {
            Posts = new List<Post>();
            RecomendedFriends = new List<User>();
            RecomendedPlaces = new List<Place>();
        }
    }
}