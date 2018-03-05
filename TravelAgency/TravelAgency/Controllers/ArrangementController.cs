using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelAgency.DataLayer.BusienssLogic;
using TravelAgency.DataLayer.Config;
using TravelAgency.DataLayer.Model;
using TravelAgency.DataLayer.Utilities;
using TravelAgency.ViewModels;

namespace TravelAgency.Controllers
{
    public class ArrangementController : Controller
    {
        // GET: Arrangement
        public ActionResult Index(String arrangementId, bool isLogged)
        {
            BusinessLogic logic = new BusinessLogic();
            MainPageViewModel model = new MainPageViewModel();
			model.IsLogged = isLogged;

            model.Destinations = logic.GetDestinations();

            List<Arrangement> topArrangements = logic.TopArrangements();
            foreach (var topArrangement in topArrangements)
            {
                ArrangementDestinationViewModel arrangement = new ArrangementDestinationViewModel();
                arrangement.Arrangement = topArrangement;
                arrangement.Destination = logic.GetArrangementDestination(topArrangement);
                model.TopArrangements.Add(arrangement);
            }

            model.Arrangement.Arrangement = logic.GetArrangemetById(arrangementId);
            model.Arrangement.Destination = logic.GetArrangementDestination(model.Arrangement.Arrangement);


            return View(model);
            
        }

        [HttpPost]
        public ActionResult Reservation(MainPageViewModel model, bool isLogged)
        {
            BusinessLogic logic = new BusinessLogic();
            Customer customer = model.Customer;

            logic.CreateCustomer(customer);
		  
            //in order to get id from database
            customer = logic.GetCustomerByPassportNumber(customer.PassportNumber);

            List<FellowTraveler> company = new List<FellowTraveler>();
            foreach(FellowTraveler traveler in model.Reservation.Company)
            {
                if (traveler.FirstName != null)
                    company.Add(traveler);
            }

            Reservation reservation = new Reservation
            {
                Customer = new MongoDB.Driver.MongoDBRef(CollectionsNames.CustomersCollection, customer.Id),
                Arrangement = new MongoDB.Driver.MongoDBRef(CollectionsNames.ArrangementsCollection, new ObjectId(model.Arrangement.Arrangement.Description)), //HACK: ne moze da se namapira ObjectId kao string..
                Company = company
            };

            logic.CreateReservation(reservation);

			return RedirectToAction("LoggedIndex", "Main", new { isLogged = isLogged });
		}
    }
}