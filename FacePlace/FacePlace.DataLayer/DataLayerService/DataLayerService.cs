using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacePlace.DataLayer.Repository.Repositories;
using FacePlace.DataLayer.Configuration;

namespace FacePlace.DataLayer.DataLayerService
{
    public class DataLayerService
    {
        private PictureRepository pictureRepository;
        private PlaceRepositiry placeRepositiry;
        private UserRepository userRepository;

        private IConfig config;

        public DataLayerService()
        {
            config = new LocalConfig();

            pictureRepository = new PictureRepository(config);
            placeRepositiry = new PlaceRepositiry(config);
            userRepository = new UserRepository(config);
        }

        public PictureRepository PictureRepository
        {
            get
            {
                return pictureRepository;
            }
        }

        public PlaceRepositiry PlaceRepositiry
        {
            get
            {
                return placeRepositiry;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                return userRepository;
            }
        }

    }
}
