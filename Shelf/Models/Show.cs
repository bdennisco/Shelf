using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shelf.Models
{
    public class Show : Media
    {
        public Show()
        {

        }

        public Show(dynamic RawResults)
        {
            id = RawResults["id"].ToString();
            name = RawResults["name"].ToString();
            poster_path = RawResults["poster_path"].ToString();
            type = 2;
            try { release_date = DateTime.Parse(RawResults["first_air_date"].ToString()); }
            catch { }
            try { number_of_seasons = RawResults["number_of_seasons"]; }
            catch { }
            Owners = new List<FacebookUser>();
            Actions = new MediaActions();
        }

        public Show(dynamic RawResults, dynamic SeasonRawResults)
        {
            id = RawResults["id"].ToString();
            name = RawResults["name"].ToString();
            poster_path = SeasonRawResults["poster_path"] ?? RawResults["poster_path"];
            type = 2;
            try { release_date = DateTime.Parse(RawResults["first_air_date"].ToString()); }
            catch { }
            try { number_of_seasons = RawResults["number_of_seasons"]; }
            catch { }
            Owners = new List<FacebookUser>();
            Actions = new MediaActions();
        }
    }
}