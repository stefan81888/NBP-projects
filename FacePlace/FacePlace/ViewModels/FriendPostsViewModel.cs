using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacePlace.ViewModels
{
    public class FriendPostsViewModel
    {
        public User User { get; set; }
        public User Friend { get; set; }
        public List<Post> Posts { get; set; }
        public List<Place> RecomendedPlaces { get; set; }
        public List<User> RecomendedFriends { get; set; }
        public bool IsFriend { get; set; }

        public FriendPostsViewModel()
        {
            Posts = new List<Post>();
            RecomendedPlaces = new List<Place>();
            RecomendedFriends = new List<User>();
        }
    }
}