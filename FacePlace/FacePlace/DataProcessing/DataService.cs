using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FacePlace.DataLayer.Utilities;
using FacePlace.DataLayer.DataLayerService;
using FacePlace.CashingSystem.CashingService;

using FacePlace.CashingSystem.Utilities;
using FacePlace.DataLayer.Model;

namespace FacePlace.DataProcessing
{
    public class DataService
    {
        private DataLayerService dataLayerService;
        private CashingService cashingService;

        public DataService()
        {
            dataLayerService = DataLayer.Utilities.ObjectFactory.GetDataLayerService();
            cashingService = CashingSystem.Utilities.ObjectFactory.GetCashingService();
        }

        public User GetUserFromCash(string username)
        {
            User user = cashingService.UserCash.GetUser(username);
            return user;
        }

        public void SaveUser(User user)
        {
            user = dataLayerService.UserRepository.Create(user);
            cashingService.UserCash.AddUserInformation(user);
        }

        public void AddFriend(string username, string friend)
        {
            dataLayerService.UserRepository.AddFriend(username, friend);
        }

        public string CreatePostId()
        {
            return cashingService.UserCash.CreatePostId();
        }

        public void SavePostToCash(Post post)
        {
            string username = post.User.Username;
            cashingService.UserCash.AddUserPost(post);
            cashingService.PlaceCash.AddPlacePost(post);
            cashingService.PlaceCash.AddVisitor(post);
            cashingService.PlaceCash.AddPostPicturesToGallery(post);

            foreach (var friend in GetFriends(post.User.Username))
            {
                cashingService.UserCash.AddPostToNewsFeed(friend.Username, post);
            }
        }

        public void StorePostToStorage(Post post)
        {
            dataLayerService.UserRepository.CreatePost(post);
        }

        public List<Post> GetRecentUserPost(User user)
        {
            string username = user.Username;
            return cashingService.UserCash.GetRecentPosts(username, 5);
        }

        public List<Post> GetRecentPlacePost(string placeId)
        {
            return cashingService.PlaceCash.GetRecentPlacePosts(placeId, 10);
        }

        public List<User> WhoWasHereRecently(Place place)
        {
            string placeId = place.Id;
            List<Post> posts = cashingService.PlaceCash.GetRecentPlacePosts(placeId, 10);

            List<User> result = new List<User>();
            foreach (Post post in posts)
            {
                result.Add(post.User);
            }

            return result;
        }

        public List<User> GetFriends(string username)
        {
            return dataLayerService.UserRepository.FriendsOfAUser(username);
        }

        public List<User> SearchUsers(string criteria)
        {
            return dataLayerService.UserRepository.SearchUsers(criteria);
        }

        public List<Post> SearchPlaces(string criteria)
        {
            return dataLayerService.PlaceRepositiry.SearchPlaces(criteria);
        }

        public List<Picture> PlaceGallery(string placeId)
        {
            return cashingService.PlaceCash.GetPlaceGallery(placeId);
        }

        public bool UsernameExists(string username)
        {
            return dataLayerService.UserRepository.UsernameExist(username);
        }

        public bool EmailExists(string email)
        {
            return dataLayerService.UserRepository.EmailExist(email);
        }

        public bool UserExist(string username)
        {
            return cashingService.UserCash.GetUser(username) != null;
        }

        public Place PlaceExists(Place createPlace)
        {
            Place place = dataLayerService.PlaceRepositiry.Get(createPlace.Name);

            if (place != null)
                return place;

            createPlace.Id = cashingService.PlaceCash.CreatePlaceId();
            place = dataLayerService.PlaceRepositiry.Create(createPlace);

            return place;
        }

        public List<Post> NewsFeedRedis(string username)
        {
            return cashingService.UserCash.GetNewsFeed(username);
        }

        public List<Post> NewsFeedNeo(string username)
        {
            string lastInRedisId = cashingService.UserCash.GetOldestPostId(username);

            return dataLayerService.UserRepository.GetNewsFeed(username, lastInRedisId, 20);
        }

        public List<User> GetRecomendedFriends(string username)
        {
            return dataLayerService.UserRepository.GetRecomendedFriends(username);
        }

        public List<Place> GetRecomendedPlaces(string username)
        {
            return dataLayerService.UserRepository.GetRecomendedPlaces(username);
        }

        public User GetUser(string username)
        {
            return cashingService.UserCash.GetUser(username);
        }

        public Place GetPlace(string placeId)
        {
            return dataLayerService.PlaceRepositiry.Get(placeId);
        }

        public List<Post> GetPlacesPosts(string placeId)
        {
            return dataLayerService.PlaceRepositiry.GetAllPosts(placeId);
        }

        public void UpdateUser(User user)
        {
            dataLayerService.UserRepository.Update(user);
            cashingService.UserCash.EditUserInformation(user);
        }

        public bool IsFriend(string username, string potentialFriend)
        {
            User user = dataLayerService.UserRepository.Get(potentialFriend);
            List<User> userFriends = dataLayerService.UserRepository.FriendsOfAUser(username);

            return Utility.IsFriend(user, userFriends);
        }
    }
}