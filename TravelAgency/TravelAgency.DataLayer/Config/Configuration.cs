using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace TravelAgency.DataLayer.Config
{
	public static class Configuration
	{
		private static string connectionString =  "mongodb://localhost:27017";
		
		private static MongoServer CreateClient()
		{
			return MongoServer.Create(connectionString);
		}

		public static MongoDatabase GetDatabase()
		{
			var client = CreateClient();
			string databaseName = DatabasesNames.TravelAgencyDatabase;
			return client.GetDatabase(databaseName);
		}
	}
}
