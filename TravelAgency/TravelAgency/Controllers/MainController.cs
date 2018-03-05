using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelAgency.DataLayer.BusienssLogic;
using TravelAgency.DataLayer.Model;
using TravelAgency.DataLayer.Utilities;
using TravelAgency.ViewModels;

namespace TravelAgency.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
			BusinessLogic logic = new BusinessLogic();
			MainPageViewModel model = new MainPageViewModel();
			model.Destinations = logic.GetFirstNDestinations(7);
            model.Countries = logic.GetCountries(10);

			List<Arrangement> topArrangements = logic.TopArrangements();			

			foreach (var topArrangement in topArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = topArrangement;
				arrangement.Destination = logic.GetArrangementDestination(topArrangement);
				model.TopArrangements.Add(arrangement);
			}

            return View(model);
        }

		public ActionResult LoggedIndex(bool isLogged)
		{
			BusinessLogic logic = new BusinessLogic();
			MainPageViewModel model = new MainPageViewModel();
			model.Destinations = logic.GetFirstNDestinations(7);
			model.Countries = logic.GetCountries(10);
			model.IsLogged = isLogged;

			List<Arrangement> topArrangements = logic.TopArrangements();

			foreach (var topArrangement in topArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = topArrangement;
				arrangement.Destination = logic.GetArrangementDestination(topArrangement);
				model.TopArrangements.Add(arrangement);
			}

			return View(model);
		}

		public ActionResult SearchDestinations(string criteria, bool isLogged)
		{
			BusinessLogic logic = new BusinessLogic();
			MainPageViewModel model = new MainPageViewModel();

			model.Destinations = logic.GetDestinations();
			model.SearchedDestinations = logic.SearchDestinations(criteria);
			model.IsLogged = isLogged;

			List<Arrangement> topArrangements = logic.TopArrangements();

			foreach (var topArrangement in topArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = topArrangement;
				arrangement.Destination = logic.GetArrangementDestination(topArrangement);
				model.TopArrangements.Add(arrangement);
			}

			return View(model);
		}

        public ActionResult CountryArrangements(String country, bool isLogged)
        {
            BusinessLogic logic = new BusinessLogic();
            MainPageViewModel model = new MainPageViewModel();

            model.Destinations = logic.GetDestinations();
			model.IsLogged = isLogged;

            List<Arrangement> topArrangements = logic.TopArrangements();
            foreach (var topArrangement in topArrangements)
            {
                ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
                arrangement.Arrangement = topArrangement;
                arrangement.Destination = logic.GetArrangementDestination(topArrangement);
                model.TopArrangements.Add(arrangement);
            }

            List<Arrangement> arrangements = logic.GetArrangementsForDestination(country);
            foreach (var arrangement in arrangements)
            {
                ArrangementDestinationViewModel arrangementVM = new ArrangementDestinationViewModel();
                arrangementVM.Arrangement = arrangement;
                arrangementVM.Destination = logic.GetArrangementDestination(arrangement);
                model.Arrangements.Add(arrangementVM);
            }

			SearchArrangements searchArrangements = new SearchArrangements()
			{
				Country = country
			};

			List<Arrangement> searchedArrangements = logic.SearchArrangements(searchArrangements);
			foreach (var searched in searchedArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = searched;
				arrangement.Destination = logic.GetArrangementDestination(searched);
				model.SearchedArrangements.Add(arrangement);
			}

			return View("Arrangements", model);
        }

        public ActionResult PlaceArrangements(string placeName, bool isLogged)
        {
			BusinessLogic logic = new BusinessLogic();
			MainPageViewModel model = new MainPageViewModel();
			SearchArrangements criteria = new SearchArrangements()
			{				
				PlaceName = placeName
			};

			model.IsLogged = isLogged;
			model.Destinations = logic.GetFirstNDestinations(7);
			List<Arrangement> searchedArrangements = new List<Arrangement>();
			searchedArrangements = logic.SearchArrangements(criteria);
			List<Arrangement> topArrangements = logic.TopArrangements();

			foreach (var topArrangement in topArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = topArrangement;
				arrangement.Destination = logic.GetArrangementDestination(topArrangement);
				model.TopArrangements.Add(arrangement);
			}

			foreach (var searched in searchedArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = searched;
				arrangement.Destination = logic.GetArrangementDestination(searched);
				model.SearchedArrangements.Add(arrangement);
			}

			return View(model);
        }

		public ActionResult AdvanceSearch(MainPageViewModel criteria)
		{
			BusinessLogic logic = new BusinessLogic();
			MainPageViewModel model = new MainPageViewModel();			

			model.Destinations = logic.GetFirstNDestinations(7);
			List<Arrangement> searchedArrangements = logic.SearchArrangements(criteria.SearchCriterias);
			List<Arrangement> topArrangements = logic.TopArrangements();

			foreach (var topArrangement in topArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = topArrangement;
				arrangement.Destination = logic.GetArrangementDestination(topArrangement);
				model.TopArrangements.Add(arrangement);
			}

			foreach (var searched in searchedArrangements)
			{
				ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
				arrangement.Arrangement = searched;
				arrangement.Destination = logic.GetArrangementDestination(searched);
				model.SearchedArrangements.Add(arrangement);
			}

			return View(model);
		}
    }
}