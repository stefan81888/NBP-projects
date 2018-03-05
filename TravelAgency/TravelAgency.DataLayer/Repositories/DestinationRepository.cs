using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using TravelAgency.DataLayer.Model;
using MongoDB.Driver;
using TravelAgency.DataLayer.Config;
using TravelAgency.DataLayer.Utilities;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization;

namespace TravelAgency.DataLayer.Repositories
{
	public class DestinationRepository : Repository<Destination>
	{
		public DestinationRepository()
		{
			GetDatabaseObjects();
			string indexedField = DestinationPropertiesNames.Country;
			CreateIndex(indexedField);
			indexedField = DestinationPropertiesNames.PlaceName;
			CreateIndex(indexedField);
		}		

		public override void GetDatabaseObjects()
		{
			DatabaseObjectsFactory<Destination> factory = new DatabaseObjectsFactory<Destination>();
			string collectionName = CollectionsNames.DestinationsCollection;
			DatabaseObjects<Destination> objects = factory.GetDatabaseObjects(collectionName);
			collection = objects.Collection;
			database = objects.Database;
		}

		public List<Destination> SearchDestinations(string criteria)
		{
			string country = DestinationPropertiesNames.Country;
			string place = DestinationPropertiesNames.PlaceName;

			var query = Query.Or(Query.Matches(country, BsonRegularExpression.Create(new Regex(criteria, RegexOptions.IgnoreCase))),
								Query.Matches(place, BsonRegularExpression.Create(new Regex(criteria, RegexOptions.IgnoreCase))));			

			List<Destination> destinations = collection.Find(query).ToList();
			return destinations;
		}

		public List<Destination> GetDestination(string country, string placeName)
		{
			string countryField = DestinationPropertiesNames.Country;
			string placeField = DestinationPropertiesNames.PlaceName;

			country = country != null ? country : string.Empty;
			placeName = placeName != null ? placeName : string.Empty;


			var query = Query.Or(Query.EQ(countryField, country),
								Query.EQ(placeField, placeName));

			List<Destination> destinations = collection.Find(query).ToList();
			return destinations;
		}

		public List<String> GetPlacesOfDestinationCountry(String country)
		{
			List<String> places = new List<String>();
			string countryField = DestinationPropertiesNames.Country;

			var query = Query.EQ(countryField, country);
			List<Destination> destinations = collection.Find(query).ToList();

			foreach (Destination d in destinations)
			{
				places.Add(d.PlaceName);
			}

			return places;
		}

        public List<Destination> GetFirstN(int count)
        {
            if (collection == null)
                return null;

            List<Destination> instances = collection.FindAll().SetLimit(count).ToList();
            return instances;
        }

		public List<Destination> GetAllCountriesDistinct(int count)
		{
			var group = new BsonDocument
			{
				{ "$group",
					new BsonDocument
						{
							{ "_id", new BsonDocument
									{
										{"Country","$Country"},
										{"Description","$Description"}
									}
										   
							}
						}
				}
			};
			var limit = new BsonDocument
			{
				{
					"$limit", count
				}
			};
			var project = new BsonDocument
				{
					{
						"$project",
						new BsonDocument
							{
								{"_id", 0},
								{"Country","$_id.Country"},
								{"Description", "$_id.Description"},
							}
					}
				};
			var pipeline = new BsonDocument[] { group, limit };
			var result = collection.Aggregate(pipeline);

			var matchingExamples = result.ResultDocuments.ToList();

			List<Destination> destinations = new List<Destination>();
			foreach (var example in matchingExamples)
			{
				foreach (var el in example)
				{
					destinations.Add(BsonSerializer.Deserialize<Destination>(el.Value.AsBsonDocument));
				}
			}

			return destinations;

		}

        public List<ObjectId> GetDestinationIds(String criteria)
        {
            string destinationField = DestinationPropertiesNames.Country;
            List<ObjectId> destinationIds = new List<ObjectId>();

            var query = Query.Matches(destinationField, BsonRegularExpression.Create(new Regex(criteria, RegexOptions.IgnoreCase)));
            List<Destination> destinations = collection.Find(query).ToList();

            foreach (Destination destination in destinations)
            {
               destinationIds.Add(destination.Id);
            }

            return destinationIds;
        }
    }
}
