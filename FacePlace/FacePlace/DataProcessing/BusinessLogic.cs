using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacePlace.DataProcessing
{
    public class BusinessLogic
    {
        private DataService dataService;

        public BusinessLogic()
        {
            dataService = Utility.GetDataService();             
        }

        public User Register(User registrationUser)
        {
            string username = registrationUser.Username;

            if (dataService.UsernameExists(username))
                return null;

            string email = registrationUser.Email;

            if (dataService.EmailExists(email))
                return null;

            dataService.SaveUser(registrationUser);
            return registrationUser;
        }
        public User Login(User loginUser)
        {
            string username = loginUser.Username;

            if (!dataService.UserExist(username))
                return null;

            User user = dataService.GetUserFromCash(username);           

            if (!Utility.CorrectPassword(loginUser.Password, user.Password))
                return null;

            return user;
        }
        public Post CreatePost(Post createdPost)
        {
            createdPost.Id = dataService.CreatePostId();
            createdPost.Place = dataService.PlaceExists(createdPost.Place);
            dataService.SavePostToCash(createdPost);
            dataService.StorePostToStorage(createdPost);

            return createdPost;
        } 
        public List<Post> GetRecentUserPosts(User user)
        {
            return dataService.GetRecentUserPost(user);
        }
        public List<Post> GetRecentPlacePosts(string placeId)
        {
            return dataService.GetRecentPlacePost(placeId);
        }
        public List<User> WhoWasHereRecently(Place place)
        {
            return dataService.WhoWasHereRecently(place);
        }
        public List<User> SearchUsers(string criteria)
        {
            return dataService.SearchUsers(criteria);
        }
        public List<Post> SearchPlaces(string criteria)
        {
            return dataService.SearchPlaces(criteria);
        }
        public List<Picture> PlaceGallery(string placeId)
        {
            return dataService.PlaceGallery(placeId);
        }
        public List<Post> NewsFeedRedis(string username)
        {
            return dataService.NewsFeedRedis(username);
        }
        public List<Post> NewsFeedNeo(string username)
        {
            return dataService.NewsFeedNeo(username);
        }                    
        public void AddFriend(string username, string friend)
        {
            dataService.AddFriend(username, friend);
        }
        public List<User> GetRecomendedFriends(string username)
        {
            return dataService.GetRecomendedFriends(username);
        }
        public List<Place> GetRecomendedPlaces(string username)
        {
            return dataService.GetRecomendedPlaces(username);
        }
        public User GetUser(string username)
        {
            return dataService.GetUser(username);
        }
        public Place GetPlace(string placeId)
        {
            return dataService.GetPlace(placeId);
        }
        public List<Post> GetPlacesPosts(string placeId)
        {
            return dataService.GetPlacesPosts(placeId);
        }
        public void UpdateUser (User user)
        {
            dataService.UpdateUser(user);
        }
        public bool IsFriend(string username, string potentialFriend)
        {
            return dataService.IsFriend(username, potentialFriend);
        }
    }
}