using FacePlace.CashingSystem.Utilities;
using FacePlace.DataLayer.Model;
using FacePlace.DataProcessing;
using FacePlace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacePlace.Controllers
{
    public class PostsController : Controller
    {
        BusinessLogic dataSource = new BusinessLogic();

        // GET: Posts
        [HttpGet]
        public ActionResult Index(string username)
        {
            User user = dataSource.GetUser(username);

            if (user == null)
                return RedirectToAction("Index", "StartPage");

            HomePageViewModel vmodel = new HomePageViewModel()
            {
                User = user,
                RecomendedFriends = dataSource.GetRecomendedFriends(user.Username),
                RecomendedPlaces = dataSource.GetRecomendedPlaces(user.Username),
                Posts = dataSource.NewsFeedRedis(user.Username)
            };
            if(vmodel.Posts.Count<10)
            {
                vmodel.Posts = dataSource.NewsFeedNeo(user.Username);
            }


            return View(vmodel);
        }


        public ActionResult AddPost(string username)
        {
            BusinessLogic dataSource = new BusinessLogic();

            Post p = new Post
            {
                User = new DataLayer.Model.User
                {
                    Username = username
                }
            };

            return View(p);
        }

        [HttpPost]
        public ActionResult SavePost(Post post)
        {
            BusinessLogic dataSource = new BusinessLogic();

            Post p = new Post
            {
                User = dataSource.GetUser(post.User.Username),
                Rating = post.Rating,
                Comment = post.Comment,
                Place = post.Place,
                Time = DateTime.Now
            };

            dataSource.CreatePost(post);

            return RedirectToAction("Index",new { username = p.User.Username });
        }

        public ActionResult FriendsPost(string friend, string username)
        {
            User friendUser = dataSource.GetUser(friend);
            User user = dataSource.GetUser(username);

            FriendPostsViewModel viewModel = new FriendPostsViewModel()
            {
                User = user,
                Friend = friendUser,
                RecomendedFriends = dataSource.GetRecomendedFriends(friend), 
                RecomendedPlaces = dataSource.GetRecomendedPlaces(username),
                Posts = dataSource.GetRecentUserPosts(friendUser),
                IsFriend = dataSource.IsFriend(username, friend)
            };

            return View("~/Views/Posts/FriendsProfile.cshtml", model: viewModel);
        }

        public ActionResult PlacePosts(string placeId,string activeUserUsername)
        {

            Place place = dataSource.GetPlace(placeId);

            PlacePostsViewModel vmodel = new PlacePostsViewModel()
            {
                Place = place,
                User = dataSource.GetUser(activeUserUsername),
                RecomendedFriends = dataSource.GetRecomendedFriends(activeUserUsername),
                RecomendedPlaces = dataSource.GetRecomendedPlaces(activeUserUsername),
                AllPosts = dataSource.GetRecentPlacePosts(placeId)
            };

            return View(vmodel);
        }

        public ActionResult EditUserProfile(string username)
        {
            BusinessLogic dataSource = new BusinessLogic();

            User user = dataSource.GetUser(username);

            return View("EditUserForm",user);
        }

        [HttpPost]
        public ActionResult SaveEditedUser(User user)
        {
            BusinessLogic dataSoucre = new BusinessLogic();

            dataSource.UpdateUser(user);

            return RedirectToAction("Index", new { username = user.Username });
        }

        public ActionResult MyProfile (string username)
        {
            User user = dataSource.GetUser(username);

            MyPostsModelView viewModel = new MyPostsModelView()
            {
                User = user,
                RecomendedFriends = dataSource.GetRecomendedFriends(username), 
                RecomendedPlaces = dataSource.GetRecomendedPlaces(username),
                Posts = dataSource.GetRecentUserPosts(user)
            };

            return View(viewModel);
        }     

        [HttpGet]
        public ActionResult SearchPlaces(string criteria, string activeUserUsername)
        {
            User user = dataSource.GetUser(activeUserUsername);

            PlacePostsViewModel viewModel = new PlacePostsViewModel();

            viewModel.AllPosts = dataSource.SearchPlaces(criteria);

            if(viewModel.AllPosts.Count == 0)
                return RedirectToAction("Index", "Posts", new { username = activeUserUsername });

            viewModel.RecomendedFriends = dataSource.GetRecomendedFriends(activeUserUsername);
            viewModel.RecomendedPlaces = dataSource.GetRecomendedPlaces(activeUserUsername);
            viewModel.User = user;

            ViewBag.Posts = viewModel.AllPosts;

            return View("~/Views/Posts/SearchedPlaces.cshtml", model: viewModel);
        }

        public ActionResult AddFriend(string username, string friend)
        {
            dataSource.AddFriend(username, friend);

            return RedirectToAction("Index", new { username = username });
        }
    }
}