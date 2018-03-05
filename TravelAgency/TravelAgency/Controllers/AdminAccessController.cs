using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelAgency.DataLayer.BusienssLogic;
using TravelAgency.DataLayer.Config;
using TravelAgency.DataLayer.Model;
using TravelAgency.ViewModels;

namespace TravelAgency.Controllers
{
	public class AdminAccessController : Controller
	{
		// GET: AdminAccess
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult LoginPage()
		{
			return View();
		}

		public ActionResult Login(Admin admin)
		{
			BusinessLogic logic = new BusinessLogic();
			Admin logged = logic.Login(admin);

			if(logged == null)
				return RedirectToAction("LoginPage", "AdminAccess");

			return RedirectToAction("LoggedIndex", "Main", new { isLogged = true});
		}

		public ActionResult AddAdmin()
		{
			return View();
		}

		public ActionResult AddAdminToDatabase(Admin admin)
		{
			if (admin == null)
				RedirectToAction("AddAdmin", "AdminAccess");

			BusinessLogic logic = new BusinessLogic();
			logic.AddAdmin(admin);
			return RedirectToAction("LoggedIndex", "Main", new { isLogged = true });
		}

		public ActionResult Logout()
		{
			return RedirectToAction("LoggedIndex", "Main", new { isLogged = false });
		}














		

		public ActionResult AddArrangement()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SaveArrangement(ArrangementDestinationViewModel data)
		{
			BusinessLogic logic = new BusinessLogic();
			Destination destination;
			Hotel hotel;
			if (String.IsNullOrEmpty(data.DestinationId))
			{
				destination = new Destination
				{
					Description = data.Destination.Description,
					Country = data.Destination.Country,
					PlaceName = data.Destination.PlaceName
				};
				logic.CreateDestination(destination);
			}
			else
			{
				destination = logic.GetDestination(new ObjectId(data.DestinationId));
			}

			if(String.IsNullOrEmpty(data.HotelId))
			{
				hotel = new Hotel
				{
					Stars=data.Hotel.Stars,
					Location=data.Hotel.Location,
					Name = data.Hotel.Name
				};
				logic.CreateHotel(hotel);
			}
			else
			{
				hotel = logic.GetHotel(new ObjectId(data.HotelId));
			}


			Arrangement arrangement = new Arrangement
			{
				StartDate = data.Arrangement.StartDate,
				Price = data.Arrangement.Price,
				Description = data.Arrangement.Description,
				Duration = data.Arrangement.Duration,
				MaxNumberOfPassengers = data.Arrangement.MaxNumberOfPassengers,
				Type = data.Arrangement.Type,
				Hotel = new MongoDB.Driver.MongoDBRef(CollectionsNames.HotelsCollection,hotel.Id),
				Destination = new MongoDB.Driver.MongoDBRef(CollectionsNames.DestinationsCollection,destination.Id)
			};

			logic.CreateArrangement(arrangement);

			return RedirectToAction("Index", "Main");
		}
		
	}
}