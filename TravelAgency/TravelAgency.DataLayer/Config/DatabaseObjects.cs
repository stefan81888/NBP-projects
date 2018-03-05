using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Config
{
	public class DatabaseObjects<Type>
	{
		public MongoCollection<Type> Collection { get; set; }
		public MongoDatabase Database { get; set; }
	}
}
