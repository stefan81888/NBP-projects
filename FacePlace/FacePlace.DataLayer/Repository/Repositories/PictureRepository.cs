using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.DataLayer.Repository.Interfaces;
using FacePlace.DataLayer.Model;
using Neo4jClient.Cypher;
using Neo4jClient;
using FacePlace.DataLayer.Configuration;

namespace FacePlace.DataLayer.Repository.Repositories
{
    public class PictureRepository : IRepository<Picture>
    {
        private GraphClient client;

        public PictureRepository(IConfig config)
        {
            client = config.GetNeo4JClient();
        }

        public Picture Create(Picture typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Url", typeInstance.PictureURL);

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Picture {PictureURL:'" + typeInstance.PictureURL + "', PlaceName:'" + typeInstance.PlaceName + "'}) return n",
                                                           queryDict, CypherResultMode.Set);


            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            Picture picture = pictures.Find(x => x.PictureURL == typeInstance.PictureURL);

            return picture;
        }

        public void Delete(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (picture:Picture {PictureURL:'" + identifier + "'}) OPTIONAL MATCH(picture) -[relationship]- () DELETE picture, relationship",
                                                queryDict, CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public Picture Get(string identifier)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", identifier);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Picture) and exists(n.PictureURL) and n.PictureURL =~'" + identifier + "' return n",
                                                            queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            Picture picture = pictures.Find(x => x.PictureURL == identifier);

            return picture;
        }

        public List<Picture> GetAll()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (picture:Picture) return picture",
                                                            queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();


            return pictures;
        }

        public Picture Update(Picture typeInstance)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PlaceName", typeInstance.PlaceName);
            queryDict.Add("PictureURL", typeInstance.PictureURL);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Picture) and exists(n.PictureURL) and n.PictureURL =~ '" + typeInstance.PictureURL + "' set n.PlaceName = '" + typeInstance.PlaceName + "' return n",
                                                             queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            Picture updatedPicture = pictures.Find(x => x.PictureURL == typeInstance.PictureURL);

            return updatedPicture;
        }

        public List<Picture> GetGallery(string placeName)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", placeName);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (place:Place {Name:'" + placeName + "'}) -[relationship:PLACE_PICTURE]-(picture:Picture) return picture",
                                                           queryDict, CypherResultMode.Set);

            List<Picture> pictures = ((IRawGraphClient)client).ExecuteGetCypherResults<Picture>(query).ToList();

            return pictures;
        }

        public void StoreImageUrl(Picture picture)
        {
            this.Create(picture);
            this.LinkToPlace(picture);           
        }

        public void LinkToPlace(Picture picture)
        {
            string url = picture.PictureURL;
            string placeName = picture.PlaceName;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("PictureURL", url);
            queryDict.Add("Name", placeName);

            var newQuery = new Neo4jClient.Cypher.CypherQuery("MATCH (place:Place {Name:'" + placeName + "'}), (picture:Picture {PictureURL:'" + url + "'})CREATE(place)-[:GALLERY]->(picture)",
                                                          queryDict, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(newQuery);
        }        
    }
}
