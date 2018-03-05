using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using TravelAgency.DataLayer.Model;
using MongoDB.Driver;
using TravelAgency.DataLayer.Config;
using MongoDB.Driver.Builders;

namespace TravelAgency.DataLayer.Repositories
{
	public class AdminRepository : Repository<Admin>
	{
		public AdminRepository()
		{
			GetDatabaseObjects();
			string indexedField = Utilities.AdminPropertiesNames.Username;
			CreateUniqueIndex(indexedField);
		}	

		public override void GetDatabaseObjects()
		{
			DatabaseObjectsFactory<Admin> factory = new DatabaseObjectsFactory<Admin>();
			string collectionName = CollectionsNames.AdminsCollection;
			DatabaseObjects<Admin> objects = factory.GetDatabaseObjects(collectionName);
			collection = objects.Collection;
			database = objects.Database;
		}		

		public Admin FindByUsername(string username)
		{
			if (username == string.Empty)
				return null;

			string criteria = Utilities.AdminPropertiesNames.Username;
			var query = Query.EQ(criteria, CreateBsonValue(username));

			return collection.FindOneAs<Admin>(query);
		}		
	}
}
