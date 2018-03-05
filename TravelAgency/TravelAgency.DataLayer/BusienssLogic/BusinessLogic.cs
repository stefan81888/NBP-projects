using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.DataLayer.RepositoryFactory;
using TravelAgency.DataLayer.Repositories;
using TravelAgency.DataLayer.Model;
using TravelAgency.DataLayer.Utilities;
using System.Globalization;
using MongoDB.Bson;

namespace TravelAgency.DataLayer.BusienssLogic
{
	public class BusinessLogic
	{
		private ArrangementRepository arrangementRepository;
		private DestinationRepository destinationRepository;
		private HotelRepository hotelRepository;
		private ReservationRepository reservationRepository;
		private AdminRepository adminRepository;
		private CustomerRepository customerRepository;

		private delegate void RemoveArrangements(ref List<Arrangement> arrangements, string criteria);
		private Dictionary<string, RemoveArrangements> removeFunctionsDictionary = new Dictionary<string, RemoveArrangements>();

		public BusinessLogic()
		{
			InitializeRepositories();
			InitializeSearchFilters();			
		}

		private void InitializeRepositories()
		{
			arrangementRepository = RepositoryFactory.RepositoryFactory.GetArrangementRepository();
			destinationRepository = RepositoryFactory.RepositoryFactory.GetDestinationRepository();
			hotelRepository = RepositoryFactory.RepositoryFactory.GetHotelRepository();
			reservationRepository = RepositoryFactory.RepositoryFactory.GetReservationRepository();
			adminRepository = RepositoryFactory.RepositoryFactory.GetUserRepository();
			customerRepository = RepositoryFactory.RepositoryFactory.GetCustomerRepository();
		}

		private void InitializeSearchFilters()
		{
			string price = SearchArrangementsPropertiesNames.PriceProperty;
			string type = SearchArrangementsPropertiesNames.TypeProperty;
			string numberOfPassangers = SearchArrangementsPropertiesNames.MaxNumberOfPassengersProperty;
			string duration = SearchArrangementsPropertiesNames.DurationProperty;
			string startDate = SearchArrangementsPropertiesNames.StartDateProperty;

			removeFunctionsDictionary = new Dictionary<string, RemoveArrangements>();
			removeFunctionsDictionary.Add(price, RemoveArrangmentHigherPrice);
			removeFunctionsDictionary.Add(type, RemoveArrangmentType);
			removeFunctionsDictionary.Add(numberOfPassangers, RemoveArrangmentLessPassangers);
			removeFunctionsDictionary.Add(duration, RemoveArrangmentDuration);
			removeFunctionsDictionary.Add(startDate, RemoveArrangmentStartDate);
		}


        public Admin Login(Admin admin)
		{
			string username = admin.Username;
			string password = admin.Password;

			Admin loggedAdmin = adminRepository.FindByUsername(username);

			if (loggedAdmin == null)
				return null;

			if (loggedAdmin.Password != password)
				return null;

			return loggedAdmin;
		}

		public bool AddAdmin(Admin admin)
		{
			return adminRepository.Create(admin);
		}

		public List<Arrangement> SearchArrangements(SearchArrangements criterias)
		{
			if (criterias.Country == null && criterias.PlaceName == null)
				return new List<Arrangement>();

			string criteria = criterias.Country != null ? criterias.Country : criterias.PlaceName;

			List<Destination> destinations = this.SearchDestinations(criteria);
			destinations.OrderBy(x => x.Country);
			List<Arrangement> arrangements = new List<Arrangement>();

			foreach (var destination in destinations)
			{
				arrangements.AddRange(arrangementRepository.GetArrangementsByDestinationId(destination.Id));
			}

			RemoveUnsuitableArrangements(ref arrangements, criterias);

			return arrangements;
		}

		private void RemoveUnsuitableArrangements(ref List<Arrangement> arrangements, SearchArrangements criteria)
		{
			string price = SearchArrangementsPropertiesNames.PriceProperty;
			string type = SearchArrangementsPropertiesNames.TypeProperty;
			string numberOfPassangers = SearchArrangementsPropertiesNames.MaxNumberOfPassengersProperty;
			string duration = SearchArrangementsPropertiesNames.DurationProperty;
			string startDate = SearchArrangementsPropertiesNames.StartDateProperty;

			removeFunctionsDictionary[price].Invoke(ref arrangements, criteria.Price);
			removeFunctionsDictionary[numberOfPassangers].Invoke(ref arrangements, criteria.MaxNumberOfPassengers);
			removeFunctionsDictionary[duration].Invoke(ref arrangements, criteria.Duration);
			removeFunctionsDictionary[startDate].Invoke(ref arrangements, criteria.StartDate);
			removeFunctionsDictionary[type].Invoke(ref arrangements, criteria.Type);
		}

		private void RemoveArrangmentHigherPrice(ref List<Arrangement> arrangements, string price)
		{
			if (price == string.Empty || price == null)
				return;

			float arrangementPrice = float.Parse(price);
			arrangements.RemoveAll(x => x.Price > arrangementPrice);
		}

		private void RemoveArrangmentLessPassangers(ref List<Arrangement> arrangements, string numberOfPassangers)
		{
			if (numberOfPassangers == string.Empty || numberOfPassangers == null)
				return;

			int requiredNumberOfPassangers = int.Parse(numberOfPassangers);
			arrangements.RemoveAll(x => x.MaxNumberOfPassengers < requiredNumberOfPassangers);
		}

		private void RemoveArrangmentDuration(ref List<Arrangement> arrangements, string duration)
		{
			if (duration == string.Empty || duration == null)
				return;

			int minimumDuration = int.Parse(duration);
			arrangements.RemoveAll(x => x.Duration < minimumDuration);
		}

		private void RemoveArrangmentType(ref List<Arrangement> arrangements, string type)
		{
			if (type == string.Empty || type == null)
				return;

			arrangements.RemoveAll(x => x.Type != type);
		}

		private void RemoveArrangmentStartDate(ref List<Arrangement> arrangements, string date)
		{
			if (date == string.Empty || date == null)
				return;

			DateTime startDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
			arrangements.RemoveAll(x => x.StartDate < startDate);
		}

        public List<Arrangement> SearchHotels(String criteria)
        {
            List<Arrangement> arrangements = new List<Arrangement>();
            List<ObjectId> hotelsids = hotelRepository.GetHotelIds(criteria);

            foreach (ObjectId id in hotelsids)
            {
                arrangements.AddRange(arrangementRepository.GetArrangementsByHotelId(id));
            }

            return arrangements != null? arrangements : new List<Arrangement>();
        }

        public List<Arrangement> GetArrangementsForDestination(String criteria)
        {
            List<Arrangement> arrangements = new List<Arrangement>();
            List<ObjectId> destinationIds = destinationRepository.GetDestinationIds(criteria);

            foreach (ObjectId id in destinationIds)
            {
                arrangements.AddRange(arrangementRepository.GetArrangementsByDestinationId(id));
            }

            return arrangements != null ? arrangements : new List<Arrangement>();
		}

        public List<Arrangement> TopArrangements()
		{
			return reservationRepository.GetPopularArrangements(3);
		}

		public List<Destination> GetDestinations()
		{
			return destinationRepository.GetAll();
		}

        public List<Destination> GetFirstNDestinations(int count)
        {
            return destinationRepository.GetFirstN(count);
        }

		public Destination GetArrangementDestination(Arrangement arrangement)
		{
			return arrangementRepository.GetDestination(arrangement.Destination);
		}

		public List<Destination> SearchDestinations(string criteria)
		{
			return destinationRepository.SearchDestinations(criteria);
		}

        public List<Destination> GetCountries (int count)
        {
            return destinationRepository.GetAllCountriesDistinct(count);
        }

        public Arrangement GetArrangemetById(String arrangementId)
        {
            Arrangement arrangement =  arrangementRepository.Get(new ObjectId(arrangementId));
            return arrangement;
        }

        public bool CreateReservation(Reservation reservation)
        {
            return reservationRepository.Create(reservation);
        }

        public bool CreateCustomer(Customer customer)
        {
            return customerRepository.Create(customer);
        }

        public Customer GetCustomerByPassportNumber(String passport)
        {
            return customerRepository.GetCustomerByPassportNumber(passport);
        }

        public bool CreateDestination(Destination destination)
        {
            return destinationRepository.Create(destination);
        }

        public bool CreateArrangement(Arrangement arrangement)
        {
            return arrangementRepository.Create(arrangement);
        }

        public bool CreateHotel(Hotel hotel)
        {
            return hotelRepository.Create(hotel);
        }

        public Destination GetDestination(ObjectId objectId)
        {
            return destinationRepository.Get(objectId);
        }

        public Hotel GetHotel(ObjectId objectId)
        {
            return hotelRepository.Get(objectId);
        }
    }
}
