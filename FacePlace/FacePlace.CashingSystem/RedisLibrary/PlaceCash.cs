using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using FacePlace.CashingSystem.Utilities;
using FacePlace.DataLayer.Model;
using FacePlace.CashingSystem.Configuration;

namespace FacePlace.CashingSystem.RedisLibrary
{
    public class PlaceCash
    {
        private readonly RedisClient redis;

        public PlaceCash(IConfig config)
        {
            redis = config.GetRedisClient();
        }

        public string CreatePlaceId()
        {
            string placeIdKey = KeysDictionary.PlaceIdKey();
            long id = redis.Incr(placeIdKey);
            return id.ToString();
        }

        public void AddVisitor(Post post)
        {
            string placeId = post.Place.Id;
            User user = post.User;   
            string serializedUser = ObjectSerializer.SerializeUser(user);
            string listId = KeysDictionary.PlaceRecentVisitors(placeId);
            redis.PushItemToList(listId, serializedUser);
        }

        //deprecated
        public List<User> GetRecentVisitors(string placeId, int numberOfVisitors)
        {
            List<User> users = new List<User>();
            string listId = KeysDictionary.PlaceRecentVisitors(placeId);

            int listCount = (int)redis.GetListCount(listId);
            int startIndex = listCount - numberOfVisitors;

            foreach (string userString in redis.GetRangeFromList(listId, startIndex, listCount - 1))
            {
                User user = ObjectDeserializer.DeserializeUser(userString);
                users.Add(user);
            }

            return users;
        }   

        public void AddPlacePost(Post post)
        {
            string serializedPost = ObjectSerializer.SerializeRating(post);
            string placeId = post.Place.Id;
            string listId = KeysDictionary.PlacePosts(placeId);
            redis.PushItemToList(listId, serializedPost);
        }

        public List<Post> GetRecentPlacePosts(string placeId, int numberOfPosts)
        {
            List<Post> posts = new List<Post>();
            string listId = KeysDictionary.PlacePosts(placeId);

            int listCount = (int)redis.GetListCount(listId);
            int startIndex = listCount - numberOfPosts;

            foreach (string postString in redis.GetRangeFromList(listId, startIndex, listCount - 1))
            {
                Post post = ObjectDeserializer.DeserializeRating(postString);
                posts.Add(post);
            }

            return posts;
        }

        public void AddPostPicturesToGallery(Post post)
        {
            string placeId = post.Place.Id;
            string listId = KeysDictionary.PlaceGallery(placeId);

            foreach (var picture in post.Pictures)
            {
                string serializedPicture = ObjectSerializer.SerializePicture(picture);
                redis.PushItemToList(listId, serializedPicture);
            }
        }

        public List<Picture> GetPlaceGallery(string placeId)
        {
            string listId = KeysDictionary.PlaceGallery(placeId);
            int listCount = (int)redis.GetListCount(listId);

            List<Picture> pictures = new List<Picture>();

            foreach (string placeString in redis.GetRangeFromList(listId, 0, listCount - 1))
            {
                Picture picture = ObjectDeserializer.DeserializePicture(placeString);
                pictures.Add(picture);
            }

            return pictures; 
        }
    }
}
