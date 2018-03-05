using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TravelAgency.DataLayer.Model;
using TravelAgency.DataLayer.Utilities;
using TravelAgency.DataLayer.Config;
using MongoDB.Driver.Builders;

namespace TravelAgency.DataLayer.Repositories
{
    public class ArrangementRepository : Repository<Arrangement>
    {
        public ArrangementRepository()
        {
			GetDatabaseObjects();
			string indexedField = ArrangementPropertiesNames.Price;
			CreateIndex(indexedField);
        }        

		public override void GetDatabaseObjects()
		{
			DatabaseObjectsFactory<Arrangement> factory = new DatabaseObjectsFactory<Arrangement>();
			string collectionName = CollectionsNames.ArrangementsCollection;
			DatabaseObjects<Arrangement> objects = factory.GetDatabaseObjects(collectionName);
			collection = objects.Collection;
			database = objects.Database;
		}

		public Hotel GetHotel(MongoDBRef reference)
		{
			return database.FetchDBRefAs<Hotel>(reference);
		}

		public Destination GetDestination(MongoDBRef reference)
		{
			return database.FetchDBRefAs<Destination>(reference);
		}

        public List<Arrangement> GetArrangementsByDestinationId(ObjectId destinationId)
        {
            var query = Query.EQ("Destination.$id", destinationId);

            List<Arrangement> list = collection.Find(query).ToList();

            return list;
        }

        public List<Arrangement> GetArrangementsByHotelId(ObjectId hotelId)
        {
            var query = Query.EQ("Hotel.$id", hotelId);

            List<Arrangement> list = collection.Find(query).ToList();

            return list;
        }		
    }
}
