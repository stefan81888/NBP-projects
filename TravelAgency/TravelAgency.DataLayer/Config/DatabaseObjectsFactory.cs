using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Config
{
	public class DatabaseObjectsFactory<Type>
	{
		public DatabaseObjects<Type> GetDatabaseObjects(string collectionName)
		{
			var database = Configuration.GetDatabase();
			var collection = database.GetCollection<Type>(collectionName);

			DatabaseObjects<Type> databaseObjects = new DatabaseObjects<Type>();
			databaseObjects.Collection = collection;
			databaseObjects.Database = database;

			return databaseObjects;
		}
	}
}
