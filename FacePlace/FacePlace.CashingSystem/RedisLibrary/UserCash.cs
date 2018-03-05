using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using FacePlace.DataLayer.Model;
using FacePlace.CashingSystem.Utilities;
using ServiceStack.Text;
using FacePlace.CashingSystem.Configuration;

namespace FacePlace.CashingSystem.RedisLibrary
{
    public class UserCash
    {
        private readonly RedisClient redis;

        public UserCash(IConfig config)
        {
            redis = config.GetRedisClient();
        }

        public string CreatePostId()
        {
            string postIdKey = KeysDictionary.PostIdKey();
            long id = redis.Incr(postIdKey);
            return id.ToString();
        }

        public void StorePlacesVisited(string username, List<Place> places)
        {
            string listId = KeysDictionary.PlacesVisitedKey(username);

            foreach (var place in places)
            {
                string serializedPlace = ObjectSerializer.SerializePlace(place);
                redis.PushItemToList(listId, serializedPlace);
            }
        }

        public void AddVisitedPlace(string username, Place place)
        {
            string serializedPlace = ObjectSerializer.SerializePlace(place);
            string listId = KeysDictionary.PlacesVisitedKey(username);
            redis.PushItemToList(listId, serializedPlace);
        }

        public List<Place> GetRecentVisitedPlaces(string username, int numberOfPlaces)
        {
            List<Place> places = new List<Place>();
            string listId = KeysDictionary.PlacesVisitedKey(username);

            int listCount = (int)redis.GetListCount(listId);
            int startIndex = listCount - numberOfPlaces;

            foreach (string placeString in redis.GetRangeFromList(listId, startIndex, listCount - 1))
            {
                Place place = ObjectDeserializer.DeserializePlace(placeString);
                places.Add(place);
            }
            
            return places;
        }

        public void StoreUserInformation(List<User> users)
        {
            foreach (var user in users)
            {
                string listId = KeysDictionary.UserInformation(user.Username);
                string serializedUser = ObjectSerializer.SerializeUser(user);
                redis.PushItemToList(listId, serializedUser);
            }
        }

        public void AddUserInformation(User user)
        {
            string serializedUser = ObjectSerializer.SerializeUser(user);
            string listId = KeysDictionary.UserInformation(user.Username);
            redis.Add(listId, serializedUser);
        }

        public void EditUserInformation(User user)
        {
            string serializedUser = ObjectSerializer.SerializeUser(user);
            string userKey = KeysDictionary.UserInformation(user.Username);
            redis.Remove(userKey);
            redis.Add(userKey, serializedUser);
        }

        public User GetUser(string username)
        {
            string listId = KeysDictionary.UserInformation(username);
            string serializedUser = redis.Get<string>(listId);

            User user = ObjectDeserializer.DeserializeUser(serializedUser);

            return user;            
        }

        public void StoreUserPosts(string username, List<Post> posts)
        {
            string listId = KeysDictionary.UserPosts(username);

            foreach (var post in posts)
            {
                string serializedPost = ObjectSerializer.SerializeRating(post);
                redis.PushItemToList(listId, serializedPost);
            }
        }

        public void AddUserPost(Post post)
        {
            string serializedPost = ObjectSerializer.SerializeRating(post);
            string username = post.User.Username;
            string listId = KeysDictionary.UserPosts(username);
            redis.PushItemToList(listId, serializedPost);
        }

        public List<Post> GetRecentPosts(string username, int numberOfPosts)
        {
            List<Post> posts = new List<Post>();
            string listId = KeysDictionary.UserPosts(username);

            int listCount = (int)redis.GetListCount(listId);
            int startIndex = listCount - numberOfPosts;

            foreach (string postString in redis.GetRangeFromList(listId, startIndex, listCount - 1))
            {
                Post post = ObjectDeserializer.DeserializeRating(postString);
                posts.Add(post);
            }

            return posts;
        }

        private int NewsFeedLength
        {
            get { return 20; }
            set { NewsFeedLength = value; }
        }

        public void AddPostToNewsFeed(string friendUsername, Post post)
        {
            string serializedPost = ObjectSerializer.SerializeRating(post);
            string username = post.User.Username;
            string listId = KeysDictionary.UserNewsFeed(friendUsername);
            redis.PushItemToList(listId, serializedPost);

            int listCount = (int)redis.GetListCount(listId);
            int startIndex = listCount - NewsFeedLength;
            if (startIndex < 0) startIndex = 0;

            redis.TrimList(listId, startIndex, listCount);
        }

        public void StoreToUserNewsFeed(string username, List<Post> posts)
        {
            string listId = KeysDictionary.UserNewsFeed(username);

            foreach (var post in posts)
            {
                string serializedPost = ObjectSerializer.SerializeRating(post);
                redis.PushItemToList(listId, serializedPost);
            }

            int listCount = (int)redis.GetListCount(listId);
            int startIndex = listCount - NewsFeedLength;
            if (startIndex < 0) startIndex = 0;

            redis.TrimList(listId, startIndex, listCount);
        }

        public List<Post> GetNewsFeed(string username)
        {
            List<Post> posts = new List<Post>();
            string listId = KeysDictionary.UserNewsFeed(username);


            foreach (string postString in redis.GetRangeFromList(listId, 0, NewsFeedLength - 1))
            {
                Post post = ObjectDeserializer.DeserializeRating(postString);
                posts.Add(post);
            }

            return posts;
        }

        public string GetOldestPostId(string username)
        {
            string listId = KeysDictionary.UserNewsFeed(username);
            string obj = redis.GetItemFromList(listId, 0);
            Post p = ObjectDeserializer.DeserializeRating(obj);
            if (p != null)
                return p.Id;
            else
                return Int32.MaxValue.ToString();
        }

        public string GetNextCommentId()
        {
            string commentId = KeysDictionary.CommentIdKey();
            long id = redis.Incr(commentId);
            return id.ToString();
        }

        public void AddCommentOnPost(CommentOnPost c)
        {
            string postKey = KeysDictionary.PostKey(c.Post.Id);
            string serializedComment = ObjectSerializer.SerializeComment(c);
            redis.PushItemToList(postKey, serializedComment);
        }

        public List<CommentOnPost> GetCommentsLastN(string postId, int n)
        {
            string postKey = KeysDictionary.PostKey(postId);
            int listCount = (int)redis.GetListCount(postId);
            int startIndex = listCount - n;

            List<CommentOnPost> result = new List<CommentOnPost>();
            List<string> list = redis.GetRangeFromList(postKey, startIndex, n);

            foreach (string s in list)
            {
                result.Add(ObjectDeserializer.DeserializeComment(s));
            }

            return result;
        }
    }
}
