using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace FacePlace.DataLayer.Configuration
{
    public class RemoteConfig : IConfig
    {
        private string address;
        private string username;
        private string password;

        private Uri uri;

        public RemoteConfig()
        {
            address = "";
            username = "";
            password = "!";
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
