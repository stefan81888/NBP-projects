using FacePlace.DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacePlace.DataProcessing
{
    public static class Utility
    {
        public static DataService GetDataService()
        {
            return new DataService();
        }

        public static BusinessLogic GetBusinessLogic()
        {
            return new BusinessLogic();
        }

        public static bool UsernameExists(User user)
        {
            return user.Username != string.Empty;
        }

        public static bool EmailExists(User user)
        {
            return user.Email != string.Empty;
        }

        public static bool UserExists(User user)
        {
            return user != null;
        }

        public static bool CorrectPassword(string input, string password)
        {
            return input == password;
        }

        public static bool IsFriend(User user, List<User> users)
        {
            foreach (var item in users)
            {
                if (item.Username == user.Username)
                    return true;
            }
            return false;
        }
    }
}