using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Configuration
{
    public interface IConfig
    {
        GraphClient GetNeo4JClient();
    }
}
