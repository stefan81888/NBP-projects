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
    public class PlaceRepositiry : IRepository<Place>
    {
        private GraphClient client;

        public PlaceRepositiry(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public Place Create(Place typeInstance)
        {
            Dictionary<string, object> queryDictionary = new Dictionary<string, object>();
            queryDictionary.Add("Name", typeInstance.Name);
            queryDictionary.Add("Location", typeInstance.Location);
            queryDictionary.Add("Description", typeInstance.Description);       

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Place {Name:'" + typeInstance.Name + "', Description:'" + typeInstance.Description
                                                            + "', Location:'" + typeInstance.Location
                                                            + "', Id:'" + typeInstance.Id + "'}) return n",
                                                            queryDictionary, CypherResultMode.Set);

            List<Place> places = ((IRawGraphClient)client).ExecuteGetCypherResults<Place>(query).ToList();

            Place place = places.Find(x => x.Name == typeInstance.Name);

            return place;
        }

        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (place:Place {Name:'" + identifier + "'}) OPTIONAL MATCH(place) -[relationship]- () DELETE place, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public Place Get(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Place) and exists(n.Name) and n.Name =~'" + identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Place> places = ((IRawGraphClient)client).ExecuteGetCypherResults<Place>(query).ToList();

            Place place = places.Find(x => x.Name == identifier);

            return place;
        }

        public List<Place> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (place:Place) return place",
                                                            queryDict, CypherResultMode.Set);

            List<Place> places = ((IRawGraphClient)client).ExecuteGetCypherResults<Place>(query).ToList();


            return places;
        }

        public Place Update(Place typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", typeInstance.Name);
            queryDict.Add("Description", typeInstance.Description);
            queryDict.Add("Location", typeInstance.Location);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Place) and exists(n.Name) and n.Name =~ '" + typeInstance.Name + "' set n.Name = '" + typeInstance.Name + "', n.Location = '" + typeInstance.Location + "', n.Description = '" + typeInstance.Description + "' return n",
                                                             queryDict, CypherResultMode.Set);

            List<Place> places = ((IRawGraphClient)client).ExecuteGetCypherResults<Place>(query).ToList();

            Place place = places.Find(x => x.Name == typeInstance.Name);

            return place;
        }

        public List<User> GetVisitors(string name)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", name);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (user:User) -[relationship:VISITED]-(place:Place {Name:'" + name + "'}) return user",
                                                           queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            return users;
        }     

    

        public List<Post> SearchPlaces(string criteria)
        {         

            var query = client.Cypher
                 .Match("(user:User)-[posted:POSTED]-(post: Post) -[ forplace:FOR_PLACE] - (place: Place) WHERE place.Name CONTAINS'" + criteria + "'")
                 .OptionalMatch("(post)-[pic:POST_PICTURE]->(picture:Picture)")
                 .Return((user, post, place, picture) => new Post
                 {
                     Id = post.As<Post>().Id,
                     Time = post.As<Post>().Time,
                     Rating = post.As<Post>().Rating,
                     Comment = post.As<Post>().Comment,
                     User = user.As<User>(),
                     Place = place.As<Place>(),
                     Pictures = (List<Picture>)picture.CollectAsDistinct<Picture>()
                 });

            var result = query.Results;
            return result.ToList();
        }

        public List<Post> GetAllPosts(string placeId)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Id", placeId);

            var query = new Neo4jClient.Cypher.CypherQuery("match (p:Place{Id:'"+placeId+"'})-[:FOR_PLACE]-(post:Post) return post",
                                                           queryDict, CypherResultMode.Set);

            List<Post> posts = ((IRawGraphClient)client).ExecuteGetCypherResults<Post>(query).ToList();

            return posts;
        }
    }
}
