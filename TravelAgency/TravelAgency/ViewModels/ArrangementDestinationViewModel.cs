using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelAgency.DataLayer.Model;

namespace TravelAgency.ViewModels
{
	public class ArrangementDestinationViewModel
	{
		public Arrangement Arrangement { get; set; }
		public Destination Destination { get; set; }
        public Hotel Hotel { get; set; }

        public String HotelId { get; set; }
        public String DestinationId { get; set; }
	}
}