using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TravelAgency.DataLayer.Model;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using TravelAgency.DataLayer.Config;
using TravelAgency.DataLayer.Utilities;
using System.Text.RegularExpressions;

namespace TravelAgency.DataLayer.Repositories
{
	public class HotelRepository : Repository<Hotel>
	{
		public HotelRepository()
		{
			GetDatabaseObjects();
			string indexedField = HotelPropertiesNames.Stars;
			CreateIndex(indexedField);
		}
		
		public override void GetDatabaseObjects()
		{
			DatabaseObjectsFactory<Hotel> factory = new DatabaseObjectsFactory<Hotel>();
			string collectionName = CollectionsNames.HotelsCollection;
			DatabaseObjects<Hotel> objects = factory.GetDatabaseObjects(collectionName);
			collection = objects.Collection;
			database = objects.Database;
		}

        public List<ObjectId> GetHotelIds(String criteria)
        {
            string hotelFiled = HotelPropertiesNames.Name;
            List<ObjectId> hotelIds = new List<ObjectId>();

            var query = Query.Matches(hotelFiled, BsonRegularExpression.Create(new Regex(criteria, RegexOptions.IgnoreCase)));
            List<Hotel> hotels = collection.Find(query).ToList();

            foreach (Hotel hotel in hotels)
            {
                hotelIds.Add(hotel.Id);
            }

            return hotelIds;
        }
    }
}
