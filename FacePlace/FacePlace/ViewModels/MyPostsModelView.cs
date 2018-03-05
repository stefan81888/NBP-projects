using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacePlace.ViewModels
{
    public class MyPostsModelView
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
        public List<Place> RecomendedPlaces { get; set; }
        public List<User> RecomendedFriends { get; set; }

        public MyPostsModelView()
        {
            Posts = new List<Post>();
            RecomendedFriends = new List<User>();
            RecomendedPlaces = new List<Place>();
        }
    }
}