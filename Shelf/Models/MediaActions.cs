using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shelf.Models
{
    public class MediaActions
    {
        public MediaActions()
        {
            Add = false;
            Remove = false;
            Info = false;
        }
        public bool Add { get; set; }
        public bool Remove { get; set; }
        public bool Info { get; set; }
    }
}