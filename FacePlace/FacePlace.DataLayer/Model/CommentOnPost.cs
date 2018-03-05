using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Model
{
    public class CommentOnPost
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime TimePosted { get; set; }

        [JsonIgnore]
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
