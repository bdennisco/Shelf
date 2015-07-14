using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shelf.Models
{
    public class Media
    {
        public Media()
        {
        }

        public string id { get; set; }
        public string name { get; set; }
        public string poster_path { get; set; }
        public int type { get; set; }
        public DateTime release_date { get; set; }
        public string Trailer { get; set; }
        public int? number_of_seasons { get; set; }
        public int? Season { get; set; }
        public List<FacebookUser> Owners { get; set; }
        public MediaActions Actions { get; set; }
        public int? order { get; set; }
    }

    // Allows using Intersect on two lists of Media items to find the common ones
    class MediaComparer : IEqualityComparer<Media>
    {
        // Products are equal if their names and product numbers are equal. 
        public bool Equals(Media x, Media y)
        {
            // Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            // Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            // Check whether the products' properties are equal. 
            return x.id == y.id;
        }

        // If Equals() returns true for a pair of objects, 
        // GetHashCode must return the same value for these objects. 

        public int GetHashCode(Media media)
        {
            // Check whether the object is null. 
            if (Object.ReferenceEquals(media, null)) return 0;

            // Get the hash code for the id field. 
            int hashProductCode = media.id.GetHashCode();

            // Calculate the hash code for the product. 
            return hashProductCode;
        }

    }
}