using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Model
{
	public class Hotel
	{
		public ObjectId Id { get; set; }
		public string Location { get; set; }
		public string Name { get; set; }
		public int Stars { get; set; }
	}
}
