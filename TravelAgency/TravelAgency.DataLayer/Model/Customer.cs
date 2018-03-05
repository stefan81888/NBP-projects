using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.DataLayer.Model
{
	public class Customer
	{
		public ObjectId Id { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public String PassportNumber { get; set; }
		public DateTime DateOfBirth { get; set; }
	}
}
