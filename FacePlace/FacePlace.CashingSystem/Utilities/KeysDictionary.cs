using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.CashingSystem.Utilities
{
    public static class KeysDictionary
    {
        public static string PlaceIdKey()
        {
            return "placeIdKey";
        }

        public static string PostIdKey()
        {
            return "postIdKey";
        }

        public static string PlacesVisitedKey(string username)
        {
            return username + ":" + "visited";
        }

        public static string PlaceRecentVisitors(string placeId)
        {
            return "place:" + placeId + ":visited"; 
        }

        public static string UserInformation(string username)
        {
            return "user:" + username + ":information";
        }

        public static string UserPosts(string username)
        {
            return "user:" + username + ":posts";
        }

        public static string PlacePosts(string placeId)
        {
            return "place:" + placeId + ":posts";
        }

        public static string PlaceGallery(string placeId)
        {
            return "place:" + placeId + ":gallery";
        }

        public static string CommentIdKey()
        {
            return "commentIdKey";
        }

        //needed for comment to be attached to post
        public static string PostKey(string postId)
        {
            return "post:" + postId + ":key";
        }

        public static string UserNewsFeed(string username)
        {
            return "user:" + username + ":newsfeed";
        }
    }
}
