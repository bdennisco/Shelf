using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shelf.Models
{
    public class FacebookUser
    {
        public FacebookUser()
        {

        }
        public FacebookUser(dynamic me)
        {
            Name = me.name;
            Id = me.id;
            ImageUrl = me.picture.data.url;
            Installed = me.installed ?? false;
        }
        public FacebookUser(dynamic me, dynamic friends)
        {
            Name = me.name;
            Id = me.id;
            ImageUrl = me.picture.data.url;
            Installed = me.installed ?? false;
            Friends = new List<FacebookUser>();

            var friendlist = friends;
            foreach (var friend in friendlist)
            {
                FacebookUser user = new FacebookUser(friend);

                if (user.Installed)
                {
                    Friends.Add(user);
                }
            }
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public bool Installed { get; set; }
        public List<FacebookUser> Friends { get; set; }
    }
}