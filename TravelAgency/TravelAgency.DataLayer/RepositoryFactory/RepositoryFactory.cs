using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.DataLayer.Repositories;

namespace TravelAgency.DataLayer.RepositoryFactory
{
	public static class RepositoryFactory
	{
		public static HotelRepository GetHotelRepository()
		{
			return new HotelRepository();
		}

        public static ArrangementRepository GetArrangementRepository()
        {
            return new ArrangementRepository();
        }

        public static DestinationRepository GetDestinationRepository()
        {
            return new DestinationRepository();
        }

        public static ReservationRepository GetReservationRepository()
        {
            return new ReservationRepository();
        }

        public static AdminRepository GetUserRepository()
        {
            return new AdminRepository();
        }

		public static CustomerRepository GetCustomerRepository()
		{
			return new CustomerRepository();
		}
	}
}
