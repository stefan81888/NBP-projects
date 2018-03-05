using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Configuration
{
    public class LocalConfig : IConfig
    {
        private string address;
        private string username;
        private string password;

        private Uri uri;

        public LocalConfig()
        {
            address = "http://localhost:7474/db/data";
            username = "neo4j";
            password = "neo4j!";
            uri = new Uri(address);
        }

        public GraphClient GetNeo4JClient()
        {
            GraphClient client = new GraphClient(uri, username, password);
            client.Connect();

            return client;
        }
    }
}
