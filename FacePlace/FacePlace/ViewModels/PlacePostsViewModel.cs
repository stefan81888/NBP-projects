using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacePlace.ViewModels
{
    public class PlacePostsViewModel
    {
        public Place Place { get; set; }
        public User User { get; set; }      //u _FacePlaceLayout za AddPost se trazi aktivan user
        public List<Post> AllPosts { get; set; }
        public List<Place> RecomendedPlaces { get; set; }
        public List<User> RecomendedFriends { get; set; }

        public PlacePostsViewModel()
        {
            AllPosts = new List<Post>();
            RecomendedPlaces = new List<Place>();
            RecomendedFriends = new List<User>();
        }
    }
}