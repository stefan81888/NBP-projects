using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.DataLayer.Model;
using ServiceStack.Text;

namespace FacePlace.CashingSystem.Utilities
{
    public static class ObjectSerializer
    {
        public static string SerializePlace(Place place)
        {
            return JsonSerializer.SerializeToString<Place>(place);
        }

        public static string SerializeUser(User user)
        {
            return JsonSerializer.SerializeToString<User>(user);
        }

        public static string SerializeRating(Post rating)
        {
            return JsonSerializer.SerializeToString<Post>(rating);
        }

        public static string SerializePicture(Picture picture)
        {
            return JsonSerializer.SerializeToString<Picture>(picture);
        }

        public static string SerializeComment(CommentOnPost comment)
        {
            return JsonSerializer.SerializeToString<CommentOnPost>(comment);
        }
    }
}
