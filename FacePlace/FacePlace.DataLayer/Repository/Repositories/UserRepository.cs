using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.DataLayer.Repository.Interfaces;
using FacePlace.DataLayer.Model;
using Neo4jClient;
using Neo4jClient.Cypher;
using FacePlace.DataLayer.Configuration;

namespace FacePlace.DataLayer.Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private GraphClient client;

        public UserRepository(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public User Create(User typeInstance)
        {
            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("DateOfBirth", typeInstance.DateOfBirth);
            queryDictionary.Add("Email", typeInstance.Email);
            queryDictionary.Add("FirstName", typeInstance.FirstName);
            queryDictionary.Add("LastName", typeInstance.LastName);
            queryDictionary.Add("Password", typeInstance.Password);
            queryDictionary.Add("Username", typeInstance.Username);

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:User {DateOfBirth:'" + typeInstance.DateOfBirth + "', Email:'" + typeInstance.Email
                                                            + "', FirstName:'" + typeInstance.FirstName + "', LastName:'" + typeInstance.LastName
                                                            + "', Password:'" + typeInstance.Password + "', Username:'" + typeInstance.Username
                                                            + "', ProfilePictureURL:'" + typeInstance.ProfilePictureURL + "'}) return n",
                                                            queryDictionary, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            User user = users.Find(x => x.Username == typeInstance.Username);

            return user;
        }

        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User {Username:'"+ identifier + "'}) OPTIONAL MATCH(user) -[relationship]- () DELETE user, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public User Get(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:User) and exists(n.Username) and n.Username =~'"+ identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            User user = users.Find(x => x.Username == identifier);

            return user;
        }

        public List<User> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) return user",
                                                            queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();


            return users;
        }

        public User Update(User typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", typeInstance.Username);
            queryDict.Add("DateOfBirth", typeInstance.DateOfBirth);
            queryDict.Add("FirstName", typeInstance.FirstName);
            queryDict.Add("LastName", typeInstance.LastName);
            queryDict.Add("Password", typeInstance.Password);
            queryDict.Add("ProfilePictureURL", typeInstance.ProfilePictureURL);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:User{Username:'" + typeInstance.Username + "'}) set n.ProfilePictureURL='" + typeInstance.ProfilePictureURL + "', n.DateOfBirth = '" + typeInstance.DateOfBirth + "', n.FirstName = '" + typeInstance.FirstName + "', n.LastName = '" + typeInstance.LastName + "', n.Password = '" + typeInstance.Password + "' return n",
                                                             queryDict, CypherResultMode.Set);

            User user = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).First();

            return user;
        }        

        public void AddFriend(string userUsername, string friendUsername)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("User", userUsername);
            queryDict.Add("Friend", friendUsername);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User {Username:'" + userUsername + "'}), (p:User {Username:'" + friendUsername + "'})CREATE(u) -[:IS_FRIEND]->(p)",
                                                queryDict, CypherResultMode.Projection);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void RemoveFriend(string userUsername, string friendUsername)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("User", userUsername);
            queryDict.Add("Friend", friendUsername);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User {Username:'" + userUsername + "'}) -[r:IS_FRIEND]-(p:User {Username:'" + friendUsername + "'}) DELETE r",
                                                queryDict, CypherResultMode.Projection);

            ((IRawGraphClient)client).ExecuteCypher(query);        
        }

        public List<User> FriendsOfAUser(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", username);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User {Username:'" + username + "'}) -[relationship:IS_FRIEND]-(friend:User) return friend",
                                                           queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            return users;
        }

        public List<Place> PlacesVisited(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", username); 

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User {Username:'" + username + "'}) -[relationship:VISITED]-(place:Place) SET place.DateVisited = relationship.DateVisited RETURN place",
                                                          queryDict, CypherResultMode.Set);

            List<Place> places = ((IRawGraphClient)client).ExecuteGetCypherResults<Place>(query).ToList();        

            return places;
        }       
        
        public User GetUserProfile(string username)
        {
            User user = this.Get(username);
            user.PlacesVisited = this.PlacesVisited(username);
            user.Friends = this.FriendsOfAUser(username);

            return user;
        }

        public bool UsernameExist(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) WHERE user.Username = '" + username + "' return user",
                                                          queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            return users.Count != 0;
        }

        public bool EmailExist(string email)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) WHERE user.Email = '" + email + "' return user",
                                                          queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            return users.Count != 0;
        }

        public List<User> SearchUsers(string criteria)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) WHERE user.Username CONTAINS '" + criteria +
                                                        "' OR user.FirstName CONTAINS '" + criteria + "'" +
                                                        "OR user.LastName CONTAINS '" + criteria + "' return user",
                                                           queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            return users;
        }

        public void DropDatabase()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var drop = new Neo4jClient.Cypher.CypherQuery("MATCH (node) -[relationship]- (relatedNode) DELETE user, relationship, relatedNode",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(drop);            
        }

        public void CreatePost(Post r)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", r.User.Username);
            queryDict.Add("Place", r.Place.Name);
            queryDict.Add("Rating", r.Rating);
            queryDict.Add("Comment", r.Comment);

            string queryText = "MATCH(u:User { Username:'" + r.User.Username + "'}) MERGE(p:Place{Name:'" + r.Place.Name + "', Id:'" + r.Place.Id + "'}) CREATE (u)-[s:POSTED]->(r:Post{Id:'" + r.Id + "',Rating:" + r.Rating + ",Comment:'" + r.Comment + "',Time:'" + r.Time + "'})-[:FOR_PLACE]->(p) ";
            queryText += "CREATE (u)-[:VISITED]->(p) ";

            foreach (Picture p in r.Pictures)
            {
                queryText += "CREATE (r)-[:POST_PICTURE]->(:Picture{PictureUrl:'" + p.PictureURL + "',PlaceName:'" + p.PlaceName + "'})<-[:PLACE_PICTURE]-(p) ";
            }

            var query = new Neo4jClient.Cypher.CypherQuery(queryText, queryDict, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);

        }

        public Post GetPost(string userName, string placeName)
        {
            var query = client.Cypher
                  .Match("(user:User{Username:'" + userName + "'})-->(r:Post)-->(place:Place{Name:'" + placeName + "'})")
                  .OptionalMatch("(r)-[:POST_PICTURE]->(pic:Picture)")
                  .Where((User user) => user.Username == userName)
                  .AndWhere((Place place) => place.Name == placeName)
                  .Return((user, r, place, pic) => new Post
                  {
                      Id = r.As<Post>().Id,
                      Time = r.As<Post>().Time,
                      Rating = r.As<Post>().Rating,
                      Comment = r.As<Post>().Comment,
                      User = user.As<User>(),
                      Place = place.As<Place>(),
                      Pictures = (List<Picture>)pic.CollectAsDistinct<Picture>()
                  });

            var result = query.Results.Single();

            return result;
        }

        public List<User> GetRecomendedFriends(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();


            var newQuery = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User) return u limit 1;"
                                                        , queryDict, CypherResultMode.Set);

            User notFriendOfAFriend = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(newQuery).ToList().First();

            queryDict.Add("Username", username);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User{Username:'" + username + "'})-[:IS_FRIEND]-(f:User)-[:IS_FRIEND]-(ff:User)" +
                                                        " WHERE NOT (u)-[:IS_FRIEND]-(ff) and not u.Username = ff.Username and not u.Username = '"
                                                        + notFriendOfAFriend.Username + "' RETURN DISTINCT ff limit 2;"
                                                           , queryDict, CypherResultMode.Set);

            List<User> result = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            result.Add(notFriendOfAFriend); 

            return result;
        }

        public List<Place> GetRecomendedPlaces(string username)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Username", username);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (u:User{Username:'" + username + "'})-[:IS_FRIEND]-(f:User)-[:VISITED]-(p:Place) WHERE NOT (u)-[:VISITED]-(p) RETURN DISTINCT p;"
                                                           , queryDict, CypherResultMode.Set);

            List<Place> result = ((IRawGraphClient)client).ExecuteGetCypherResults<Place>(query).ToList();

            return result;
        }

        public List<Post> GetNewsFeed(string username)
        {
            List<Post> result = new List<Post>();

            var query = client.Cypher
                  .Match("(user:User{Username:'" + username + "'})-[:IS_FRIEND]-(f:User)-[:POSTED]->(post:Post)-->(place:Place) ")
                  .OptionalMatch("(post)-[:POST_PICTURE]->(pic:Picture)")
                  .Where((User user) => user.Username == username)
                  .Return((f, post, place, pic) => new Post
                  {
                      Id = post.As<Post>().Id,
                      Time = post.As<Post>().Time,
                      Rating = post.As<Post>().Rating,
                      Comment = post.As<Post>().Comment,
                      User = f.As<User>(),
                      Place = place.As<Place>(),
                      Pictures = (List<Picture>)pic.CollectAsDistinct<Picture>()
                  });

            result = (List<Post>)query.Results;

            return result;
        }

        public List<Post> GetNewsFeed(string username,string fromId,int limit)
        {
            List<Post> result = new List<Post>();

            var query = client.Cypher
                  .Match("(user:User{Username:'" + username + "'})-[:IS_FRIEND]-(f:User)-[:POSTED]->(post:Post)-->(place:Place) ")
                  .Where("post.Id < '"+fromId+"' ")
                  .OptionalMatch("(post)-[:POST_PICTURE]->(pic:Picture)")
                  .Return((f, post, place, pic) => new Post
                  {
                      Id = post.As<Post>().Id,
                      Time = post.As<Post>().Time,
                      Rating = post.As<Post>().Rating,
                      Comment = post.As<Post>().Comment,
                      User = f.As<User>(),
                      Place = place.As<Place>(),
                      Pictures = (List<Picture>)pic.CollectAsDistinct<Picture>()
                  })
                  .Limit(limit);

            result = (List<Post>)query.Results;

            return result;
        }

        public void AddCommentOnPost(CommentOnPost comment)
        {
            client.Cypher
            .Match("(p:Post),(u:User)")
            .Where((Post p) => p.Id == comment.Post.Id)
            .AndWhere((User u) => u.Username == comment.User.Username)
            .Create("(p)<-[:COMMENT_ON]-(c:Comment{Id:'" + comment.Id + "',Text:'" + comment.Text + "',TimePosted:'" + comment.TimePosted + "'})<-[:COMMENTED]-(u)")
            .ExecuteWithoutResults();
        }

        public List<CommentOnPost> GetPostComments(string postId)
        {
            var query = client.Cypher
                .Match("(u:User)-[:POSTED]->(p:Post)<-[:COMMENT_ON]-(comment:Comment)")
                .Where((Post p) => p.Id == postId)
                .Return((comment, u, p) => new CommentOnPost
                {
                    Id = comment.As<CommentOnPost>().Id,
                    Text = comment.As<CommentOnPost>().Text,
                    TimePosted = comment.As<CommentOnPost>().TimePosted,
                    Post = p.As<Post>(),
                    User = u.As<User>()
                });

            List<CommentOnPost> result = (List<CommentOnPost>)query.Results;

            return result;
        }


    }
}
