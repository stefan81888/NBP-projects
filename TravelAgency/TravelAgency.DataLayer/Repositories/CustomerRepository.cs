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

namespace TravelAgency.DataLayer.Repositories
{
	public class CustomerRepository : Repository<Customer>
	{
		public CustomerRepository()
		{
			GetDatabaseObjects();
			string indexedField = Utilities.CustomerPropertiesNames.PassportNumber;
			CreateUniqueIndex(indexedField);
		}	

		public override void GetDatabaseObjects()
		{
			DatabaseObjectsFactory<Customer> factory = new DatabaseObjectsFactory<Customer>();
			string collectionName = CollectionsNames.CustomersCollection;
			DatabaseObjects<Customer> objects = factory.GetDatabaseObjects(collectionName);
			collection = objects.Collection;
			database = objects.Database;
		}

        public Customer GetCustomerByPassportNumber(String passport)
        {
            var passportField = CustomerPropertiesNames.PassportNumber;

            var query = Query.EQ(passportField, passport);

            Customer customer = collection.FindOne(query);

            return customer;
        }
	}
}
