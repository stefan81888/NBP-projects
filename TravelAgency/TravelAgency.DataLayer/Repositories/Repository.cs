using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Repositories
{
	public abstract class Repository<Type> where Type: class
	{
		protected MongoCollection<Type> collection;
		protected MongoDatabase database;

		public abstract void GetDatabaseObjects();

		public bool Create(Type instance)
		{
			if (collection == null)
				return false;

			long collectionCount = collection.Count();

			collection.Insert(instance);
			return collectionCount != collection.Count();
		}

		public void Delete(ObjectId id)
		{
			if (collection == null)
				return;

			IMongoQuery query = MatchByIdQuery(id);
			collection.Remove(query);
		}

		public Type Get(ObjectId id)
		{
			if (collection == null)
				return null;

			IMongoQuery query = MatchByIdQuery(id);
			return collection.FindOne(query);
		}

		public List<Type> GetAll()
		{
			if (collection == null)
				return null;

			List<Type> instances = collection.FindAllAs<Type>().ToList();
			return instances;
		}

		public void Update(Type instance)
		{
			if (collection == null)
				return;

			collection.Save(instance);
		}		

		public IMongoQuery MatchByIdQuery(ObjectId id)
		{
			IMongoQuery query = Query.EQ("_id", id);
			return query;
		}

		public BsonValue CreateBsonValue(object value)
		{
			BsonValue bsonValue = BsonValue.Create(value);
			return bsonValue;
		}

		protected void CreateUniqueIndex(string indexedField)
		{
			if (collection == null)
				return;

			if (collection.Count() != 0)
				return;

			collection.EnsureIndex(new IndexKeysBuilder().Ascending(indexedField), IndexOptions.SetUnique(true));
		}
		
		protected void CreateIndex(string indexedField)
		{
			if (collection == null)
				return;

			if (collection.Count() != 0)
				return;

			collection.EnsureIndex(new IndexKeysBuilder().Ascending(indexedField));
		}
	}
}
