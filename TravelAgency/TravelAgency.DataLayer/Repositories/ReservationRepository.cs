using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using TravelAgency.DataLayer.Model;
using MongoDB.Driver;
using TravelAgency.DataLayer.Config;
using MongoDB.Bson.Serialization;

namespace TravelAgency.DataLayer.Repositories
{
	public class ReservationRepository : Repository<Reservation>
	{
		public ReservationRepository()
		{
			GetDatabaseObjects();
		}		

		public override void GetDatabaseObjects()
		{
			DatabaseObjectsFactory<Reservation> factory = new DatabaseObjectsFactory<Reservation>();
			string collectionName = CollectionsNames.ReservationsCollections;
			DatabaseObjects<Reservation> objects = factory.GetDatabaseObjects(collectionName);
			collection = objects.Collection;
			database = objects.Database;
		}	

		public Arrangement GetArrangement(MongoDBRef reference)
		{
			return database.FetchDBRefAs<Arrangement>(reference);
		}

		public Customer GetCustomer(MongoDBRef reference)
		{
			return database.FetchDBRefAs<Customer>(reference);
		}

        public List<Arrangement> GetPopularArrangements(int count)
        {
            var group = new BsonDocument
            {
                { "$group",new BsonDocument
                    {
                        { "_id", "$Arrangement"},
                        { "count",new BsonDocument{ { "$sum", 1 } } }
                    }
                }
            };
            var sort = new BsonDocument
            {
                { "$sort",new BsonDocument
                    {
                        {"count", -1 }
                    }
                }
            };
            var projection = new BsonDocument
            {
                {
                    "$project" , new BsonDocument
                    {
                        { "count",0 }
                    }
                }
            };
            var limit = new BsonDocument
            {
                {
                    "$limit", count
                }
            };

            var pipeline = new BsonDocument[] { group, sort, projection, limit };
            var result = collection.Aggregate(pipeline);

			if (result == null)
				return null;

            var matchingExamples = result.ResultDocuments.ToList();

            List<MongoDBRef> refs = new List<MongoDBRef>();
            foreach (var example in matchingExamples)
            {
                foreach (var el in example)
                {
                    refs.Add(BsonSerializer.Deserialize<MongoDBRef>(el.Value.AsBsonDocument));
                }
            }

            List<Arrangement> arrangements = new List<Arrangement>();
            foreach (var a in refs)
            {
                arrangements.Add(this.GetArrangement(a));
            }

            return arrangements;
        }
    }
}
