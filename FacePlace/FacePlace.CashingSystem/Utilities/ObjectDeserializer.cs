using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.DataLayer.Model;

namespace FacePlace.CashingSystem.Utilities
{
    public static class ObjectDeserializer
    {
        public static Place DeserializePlace(string placeString)
        {
            return (Place)JsonSerializer.DeserializeFromString(placeString, typeof(Place));
        }

        public static User DeserializeUser(string userString)
        {
            return (User)JsonSerializer.DeserializeFromString(userString, typeof(User));
        }

        public static Post DeserializeRating(string ratingString)
        {
            return (Post)JsonSerializer.DeserializeFromString(ratingString, typeof(Post));
        }

        public static Picture DeserializePicture(string pictureString)
        {
            return (Picture)JsonSerializer.DeserializeFromString(pictureString, typeof(Picture));
        }

        public static CommentOnPost DeserializeComment(string commentString)
        {
            return (CommentOnPost)JsonSerializer.DeserializeFromString(commentString, typeof(CommentOnPost));
        }
    }
}
