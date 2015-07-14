using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shelf.Models
{
    public class Movie : Media
    {
        public Movie()
        {

        }

        public Movie(dynamic RawResults)
        {
            id = RawResults["id"].ToString();
            name = RawResults["title"].ToString();
            poster_path = RawResults["poster_path"].ToString();
            type = 1;
            try { release_date = DateTime.Parse(RawResults["release_date"].ToString()); }
            catch {}
            Owners = new List<FacebookUser>();
            Actions = new MediaActions();
        }

    }
}