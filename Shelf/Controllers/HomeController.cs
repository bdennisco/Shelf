using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Shelf.Models;

namespace Shelf.Controllers
{
    public class HomeController : Controller
    {
        private shelfDBEntities db = new shelfDBEntities();

        [FacebookAuthorize]
        public ActionResult Index()
        {
            FacebookUser me = (FacebookUser)Session["me"];

            return View(me);
        }

        [FacebookAuthorize]
        public ActionResult Edit()
        {
            FacebookUser me = (FacebookUser)Session["me"];

            return View(me);
        }

        [FacebookAuthorize]
        public ActionResult Browse()
        {
            FacebookUser me = (FacebookUser)Session["me"];
            
            return View(me);
        }

        [FacebookAuthorize]
        public ActionResult Friend(string FriendID)
        {
            FacebookUser me = (FacebookUser)Session["me"];
            FacebookUser friend = me.Friends.Where(f => f.Id == FriendID).FirstOrDefault();

            return View(friend);
        }

        [FacebookAuthorize]
        public PartialViewResult Add(string MediaID)
        {
            FacebookUser me = (FacebookUser)Session["me"];

            Media item = (Media)TempData[MediaID];

            if (db.Ownerships.Any(o => o.MediaID == item.id & o.UserID == me.Id) == false)
            {
                Ownership ownership = new Ownership();
                ownership.MediaID = item.id;
                ownership.TypeID = item.type;
                ownership.UserID = me.Id;
                ownership.Season = item.Season;
                db.Ownerships.Add(ownership);

                db.SaveChanges();
            }

            return PartialView("~/Views/Shared/_RenderMedia.cshtml", item);
        }

        [FacebookAuthorize]
        public void AddFromSearch(string MediaId, string MediaType)
        {
            FacebookUser me = (FacebookUser)Session["me"];

            Ownership ownership = new Ownership();
            ownership.MediaID = MediaId;
            ownership.Order = db.Ownerships.Where(x => x.UserID == me.Id).Max(y => y.Order) + 1;
            ownership.UserID = me.Id;

            if (MediaType == "movie")
            {
                ownership.TypeID = 1;
            }
            else if (MediaType == "tv")
            {
                ownership.TypeID = 2;
            }


            db.Ownerships.Add(ownership);

            db.SaveChanges();
        }

        [FacebookAuthorize]
        public void UpdateOrder(string MediaID, int NewOrder)
        {
            FacebookUser me = (FacebookUser)Session["me"];

            var MyOwnerships = db.Ownerships.Where(u => u.UserID == me.Id);
            Ownership ThisMedia = db.Ownerships.Where(u => u.UserID == me.Id).Where(m => m.MediaID == MediaID).FirstOrDefault();

            // figures out the smaller and larger values for range use later
                int a;
                int b;

                if (NewOrder < ThisMedia.Order)
                {
                    a = NewOrder;
                    b = ThisMedia.Order ?? 0;
                }
                else
                {
                    a = ThisMedia.Order ?? 0;
                    b = NewOrder;
                }

            var OrdersToChange = MyOwnerships.Where(x => x.Order > a).Where(x => x.Order < b).OrderBy(x=>x.Order);

            foreach (var ownership in OrdersToChange)
            {
                ownership.Order = ownership.Order - 1;
            }

            ThisMedia.Order = NewOrder;

            db.SaveChanges();
        }

        [FacebookAuthorize]
        public void Remove(string MediaID)
        {
            FacebookUser me = (FacebookUser)Session["me"];

            Ownership ownership = db.Ownerships.Where(u => u.UserID == me.Id).Where(m => m.MediaID == MediaID).FirstOrDefault();
            db.Ownerships.Remove(ownership);

            db.SaveChanges();
        }

        [FacebookAuthorize]
        public PartialViewResult Search(string query = "")
        {
            FacebookUser me = (FacebookUser)Session["me"];
            
            // Init lists
            List<Media> Results = new List<Media>();
            List<Media> APIResults = new List<Media>();
            List<Media> FriendsAll = new List<Media>();
            List<Media> FriendsResults = new List<Media>();
            
            try {
                // Search API for media to add
                APIResults = new MediaAPI().Search(query);

                // Search Friends' shelves
                foreach (var friend in me.Friends)
                {
                    foreach (var media in new MediaAPI().GetUserShelf(friend.Id))
                    {
                        media.Owners.Add(friend);
                        FriendsAll.Add(media);
                    }
                }

                // Add my media to owners
                foreach (var media in new MediaAPI().GetUserShelf(me.Id))
                {
                    media.Owners.Add(me);
                    FriendsAll.Add(media);
                }

                // Search FriendsResults for owners of APIResults
                FriendsResults = FriendsAll.Intersect(APIResults, new MediaComparer()).ToList();

                // Join results into one list
                Results = FriendsResults.Union(APIResults, new MediaComparer()).Take(6).ToList();

                return PartialView("_SearchResults", Results);
            }
            catch
            {
                return PartialView("_SearchResults", Results);
            }
        }

        public PartialViewResult Info(string MediaID, int Type = 1)
        {
            FacebookUser me = (FacebookUser)Session["me"];

            Media media;
            if (Type == 1) {
                media = new MediaAPI().GetMovie(MediaID);
            } else if (Type == 2) {
                media = new MediaAPI().GetShow(MediaID);
            } else {
                media = new Media();
            }

            return PartialView("_Info", media);
        }
    }
}