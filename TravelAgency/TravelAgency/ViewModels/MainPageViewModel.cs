using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelAgency.DataLayer.Model;
using TravelAgency.DataLayer.Utilities;

namespace TravelAgency.ViewModels
{
	public class MainPageViewModel
	{
		public List<ArrangementDestinationViewModel> TopArrangements { get; set; }
		public List<Destination> Destinations { get; set; }
		public List<Destination> SearchedDestinations { get; set; }
		public List<Destination> Countries { get; set; }
        public List<ArrangementDestinationViewModel> Arrangements { get; set; }
        public List<ArrangementDestinationViewModel> SearchedArrangements { get; set; }
		public SearchArrangements SearchCriterias { get; set; }

        public ArrangementDestinationViewModel Arrangement { get; set; }
        public Customer Customer { get; set; }
        public Reservation Reservation { get; set; }

		public bool IsLogged { get; set; }

		public MainPageViewModel()
		{
			TopArrangements = new List<ArrangementDestinationViewModel>();
			Destinations = new List<Destination>();
			SearchedDestinations = new List<Destination>();
			Countries = new List<Destination>();
			SearchedArrangements = new List<ArrangementDestinationViewModel>();
            Arrangements = new List<ArrangementDestinationViewModel>();
            Arrangement = new ArrangementDestinationViewModel();
			IsLogged = false;
        }
	}
}