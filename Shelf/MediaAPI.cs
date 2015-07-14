using Shelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Shelf
{
    public class MediaAPI
    {
        //******************* Properties *******************//
        // Constructor
        public MediaAPI()
        {
            library = "https://api.themoviedb.org/3/";
            apiKey = "api_key=c3c880a81a589570f8a0a15fad890351";
            image_base_url = "http://image.tmdb.org/t/p/";
            secure_image_base_url = "https://image.tmdb.org/t/p/";
            poster_size = "w185";
            default_filters = "&include_adult=false";
        }
        // Properties
        public string library { get; set; }
        public string apiKey { get; set; }
        public string image_base_url { get; set; }
        public string secure_image_base_url { get; set; }
        public string poster_size { get; set; }
        public string default_filters { get; set; }

        // Ownership entities
        private Shelf.Models.shelfDBEntities db = new Models.shelfDBEntities();

        // Custom WebClient
        public class DecompressedWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                return request;
            }
        }





        //******************* Methods *******************//
        // Gets data from url
        public Movie GetMovie(string MediaID)
        {
            // Get Raw Movie Data
            string FullPath = library + "movie/" + MediaID + "?" + apiKey + default_filters;
            dynamic RawResults = parseJson(FullPath);
            // Construct movie
            Movie movie = new Movie(RawResults);
            return movie;
        }
        public Show GetShow(string MediaID)
        {
            // Get Raw Show Data
            string FullPath = library + "tv/" + MediaID + "?" + apiKey + default_filters;
            dynamic RawResults = parseJson(FullPath);
            // Construct show
            Show show = new Show(RawResults);
            return show;
        }
            public Show GetShow(string MediaID, int Season)
            {
                // Get Raw Show Data
                string FullPath = library + "tv/" + MediaID + "?" + apiKey + default_filters;
                dynamic RawResults = parseJson(FullPath);
                // Get Raw Season Data for poster
                string SeasonFullPath = library + "tv/" + MediaID + "/season/" + Season + "?" + apiKey + default_filters;
                dynamic SeasonRawResults = parseJson(SeasonFullPath);

                // Construct show
                Show show = new Show(RawResults, SeasonRawResults);
                return show;
            }

        // Searches TMDB, both movies and tv. Returns a concatenanted list of media items
        public List<Media> Search(string query)
        {
            string FullPathMovie = library + "search/movie" + "?" + apiKey + "&query=" + query + default_filters; 
            string FullPathTV = library + "search/tv" + "?" + apiKey + "&query=" + query + default_filters;
            dynamic RawResultsMovie = parseJson(FullPathMovie);
            dynamic RawResultsTV = parseJson(FullPathTV);

            List<Media> SearchResults = new List<Media>();
            foreach (var m in RawResultsMovie["results"])
            {
                try
                {
                    if (m["popularity"] > 1)
                    {
                        Movie movie = new Movie(m);
                        SearchResults.Add((Media)movie);
                    }
                }
                catch (KeyNotFoundException e) {}
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {}
            }
            foreach (var m in RawResultsTV["results"])
            {
                try
                {
                    if (m["popularity"] > 1)
                    {
                        Show show = new Show(m);
                        SearchResults.Add((Media)show);
                    }
                }
                catch (KeyNotFoundException e) { }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {}
            }
            List<Media> SortedSearchResults = SearchResults.OrderByDescending(o=>o.release_date).ToList();

            return SortedSearchResults;
        }
        
        // Get Media List
        public List<Media> GetUserShelf(string UserID)
        {
            var OwnedMedia = db.Ownerships.Where(x => x.UserID == UserID);
            var MediaList = new List<Media>();

            foreach (var item in OwnedMedia)
            {
                try
                {
                    // init media
                    Media media;
                    if (item.TypeID == 2)
                    {
                        if (item.Season != null){
                            media = GetShow(item.MediaID, (int)item.Season);
                        }
                        else
                        {
                            media = GetShow(item.MediaID);
                        }
                    }
                    else
                    {
                        media = GetMovie(item.MediaID);
                    }

                    MediaList.Add(media);
                }
                catch { }
            }

            return MediaList;
        }
        
        // Builds Image URL
        public string GetPoster(string poster_path)
        {
            string url = secure_image_base_url + poster_size + poster_path;
            return url;
        }

        // downloads json from a url and parses it
        public dynamic parseJson(string fullQuery)
        {
            string jsonResults = "";
            using (DecompressedWebClient webClient = new DecompressedWebClient())
            {
                webClient.Encoding = System.Text.Encoding.UTF8;
                // try/catch loop to download json
                bool success = false;
                int attempts = 0;
                do
                {
                    try
                    {
                        attempts++;
                        jsonResults = webClient.DownloadString(fullQuery).ToString();
                        success = true;
                    }
                    catch (System.Net.WebException e)
                    {
                        
                    }
                } while (!success || (!success && attempts <= 3));
            }
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            dynamic parsedResults = jss.Deserialize<dynamic>(jsonResults);
            return parsedResults;
        }
    }
}