using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Model
{
    public class Post
    {
        public string Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }

        public User User { get; set; }
        public Place Place { get; set; }

        public List<Picture> Pictures { get; set; }
        public List<CommentOnPost> Comments { get; set; }

        public Post()
        {
            Pictures = new List<Picture>();
            Comments = new List<CommentOnPost>();
        }
    }
}
